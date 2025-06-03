using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class FinishLevelUI : MonoBehaviour
{
    [SerializeField] private Text levelName;
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private RectTransform star1Rect;
    [SerializeField] private RectTransform star2Rect;
    [SerializeField] private RectTransform star3Rect;


    [SerializeField] private Text wellDoneText;
    [SerializeField] private Text timeValue;

    [Header("Pause Button")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject settingsCanvas;

    private SFXManager sfxManager;
    private TimerController timerController;
    private GameManager gameManager;

    [Header("Open Menu Button")]
    [SerializeField] private InputActionProperty rightB_Button;

    private void Start()
    {
        InitializeReferences();



        this.gameObject.transform.localScale = Vector3.zero;
        wellDoneText.text = "";
        pauseSwitch = false;
        ToggleInteraction(true);



        rightB_Button.action.performed += OnRightBButtonPressed;

    }

    private bool pauseSwitch;

    private void OnRightBButtonPressed(InputAction.CallbackContext context)
    {

        // Toggle pause switch
        pauseSwitch = !pauseSwitch;

        // Update Text
        levelName.text = "Level: " + gameManager.GetCurrentLevel();
        timeValue.text = timerController.TimeCounter.text + " (Stopped Time)";

        // Toggle pause
        PauseToggle(pauseSwitch);

    }

    public void ShowOptions()
    {
        var pauseSettings = settingsCanvas.GetComponent<PauseSettings>();
        if (pauseSettings != null)
        {
            pauseSettings.ShowPauseSettings();
        }
    }


    [Header("Pause Menu References")]
    [SerializeField] private InputActionAsset actionAsset;
    const string LEFTHAND_ACTION_MAP_NAME = "XRI LeftHand Locomotion";
    const string RIGHTHAND_ACTION_MAP_NAME = "XRI RightHand Locomotion";


    private void ToggleInteraction(bool enable)
    {
        if (!enable)
        {
            Time.timeScale = 0f;

            actionAsset.FindActionMap(LEFTHAND_ACTION_MAP_NAME).Disable();
            actionAsset.FindActionMap(RIGHTHAND_ACTION_MAP_NAME).Disable();
        }
        else
        {
            Time.timeScale = 1f;

            actionAsset.FindActionMap(LEFTHAND_ACTION_MAP_NAME).Enable();
            actionAsset.FindActionMap(RIGHTHAND_ACTION_MAP_NAME).Enable();
        }
    }

    private void PauseToggle(bool pause)
    {
        if (pause)
        {
            Debug.Log("Pause");

            BroAudio.SetEffect(Effect.LowPass(800f));

            nextButton.gameObject.SetActive(false);
            settingsButton.gameObject.SetActive(true);

            // Animate UI
            this.gameObject.transform.DOScale(0.003f, 0.5f).From(0f).SetEase(Ease.OutBack).SetUpdate(true);

            // Disable interaction
            ToggleInteraction(false);
        }
        else if (!pause)
        {
            Debug.Log("Resume");


            nextButton.gameObject.SetActive(true);
            settingsButton.gameObject.SetActive(false);

            // Animate UI
            this.gameObject.transform.DOScale(0f, 0.5f).From(0.003f).SetEase(Ease.OutBack).SetUpdate(true);

            // Enable interaction
            ToggleInteraction(true);

            // Reset audio after timescale = 1
            BroAudio.SetVolume(BGMManager.Instance.bgm, 0.5f);
            BroAudio.SetEffect(Effect.ResetLowPass(1f));
        }
    }
    private void InitializeReferences()
    {

        PauseSettings pauseSettings = FindObjectOfType<PauseSettings>();

        if (pauseSettings != null)
        {
            settingsCanvas = pauseSettings.gameObject;
        }
        else
        {
            Debug.LogWarning("PauseSettings not found in the scene.");
        }


        sfxManager = SFXManager.Instance;
        timerController = TimerController.Instance;
        gameManager = GameManager.Instance;
    }


    private void OnEnable()
    {
        FinishLine.OnFinishLineReached += DisplayFinishLevelUI;
    }

    private void OnDisable()
    {
        FinishLine.OnFinishLineReached -= DisplayFinishLevelUI;
    }

    private void OnDestroy()
    {
        // Unsubscribe when the object is destroyed to avoid memory leaks
        rightB_Button.action.performed -= OnRightBButtonPressed;
    }


    private void DisplayFinishLevelUI()
    {
        // Replace settings button with next button
        nextButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(false);

        this.gameObject.transform.DOScale(0.003f, 0.5f).SetEase(Ease.OutBack);

        // Play the SFX for when the menu pops up
        BroAudio.Play(sfxManager.levelComplete_2);

        levelName.text = "Level: " + gameManager.GetCurrentLevel();
        timeValue.text = timerController.TimeCounter.text;

        // Animate stars after 1 second delay
        DOVirtual.DelayedCall(1f, () =>
        {
            BroAudio.Play(sfxManager.star_1);
            AnimateStar(star1Rect, star1, false, 0.15f, () =>
            {
                if (!timerController.TwoStarPassed)
                {
                    BroAudio.Play(sfxManager.star_2);
                    AnimateStar(star2Rect, star2, timerController.TwoStarPassed, 0.15f, () =>
                    {
                        if (!timerController.ThreeStarPassed)
                        {
                            BroAudio.Play(sfxManager.star_3);
                            AnimateStar(star3Rect, star3, timerController.ThreeStarPassed, 0.15f, () =>
                            {
                                AnimateWellDoneText(3);
                            });
                        }
                        else
                        {
                            AnimateWellDoneText(2);
                        }
                    });
                }
                else
                {
                    AnimateWellDoneText(1);
                }
            });
        });
    }


    private void AnimateWellDoneText(int starCount)
    {
        wellDoneText.text = PickWellDoneText(starCount);
        wellDoneText.DOFade(1f, 1f).From(0f);

        wellDoneText.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            wellDoneText.transform.DOScale(1f, 0.3f);
        });
    }

    private string PickWellDoneText(int starCount)
    {
        List<string> threeStarMessages = new List<string>()
        {
            "It's not like I expected you to do well or anything… Baka.",
            "Hmph, don't get used to hearing this, but… well done.",
            "Three stars? I guess even you can surprise me sometimes.",
            "You think you're impressive? It's just a fluke!",
            "Tch, don't think I'm impressed, but... well, maybe a little.",
            "Okay, okay! I'll admit it, you did good… for once.",
            "Hmph, don't get cocky just because you got three stars.",
            "Well, I guess you're not completely useless after all...",
            "Three stars... fine, I'm a little impressed. ",
            "It's not like I expected you to do this well. But… good job.",
        };

        List<string> twoStarMessages = new List<string>()
        {
            "Two stars? You're *this* close to greatness...",
            "Well, you tried… but it's still not three stars, is it?",
            "Two stars? Pfft. You know there's a third star, right?",
            "Hey, you're good! Just not that good.",
            "Two stars! Almsot like you're great... but not really.",
            "You did well!, Just not three star well.",
            "Congrats on your three star performance! oh wait",
            "Good job! You only missed perfection by a mile.",
            "Seems you're not good enough for three stars.",
        };

        List<string> oneStarMessages = new List<string>()
        {
            "A single star? Wow, talk about setting the bar low...",
            "Blink and you'll miss it... oh wait, that was your effort.",
            "Participation trophy unlocked!",
            "ALERT! TREAD CAREFULLY AROUND THIS SUPERHUMAN SPECIMEN!!!",
            "It's advisible to wear your headset on your head.",
            "Good news: you finished. Bad news: you're bad.",
            "I've seen faster snails. Ah there's one. Hey bob!",
            "I'd say 'nice try' if I was lying.",
            "Don't worry, no one saw that.",
            "Congratulations! You've officially set the bar for failure.",
            "I didn't know 'barely trying' was a strategy. Good to know!",
            "Achievement Unlocked: Why Even Bother!",
            "One Star! Because even a broken clock is right twice a day.",
        };


        // Choose messages based on the star count
        switch (starCount)
        {
            case 3:
                return threeStarMessages[UnityEngine.Random.Range(0, threeStarMessages.Count)];
            case 2:
                return twoStarMessages[UnityEngine.Random.Range(0, twoStarMessages.Count)];
            case 1:
            default:
                return oneStarMessages[UnityEngine.Random.Range(0, oneStarMessages.Count)];
        }
    }


    private void AnimateStar(RectTransform starRect, UnityEngine.UI.Image starImage, bool starPassed, float delay, TweenCallback onComplete)
    {
        starRect.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack).SetDelay(delay).OnComplete(() =>
        {
            starRect.DOScale(1f, 0.2f);
            if (!starPassed)
            {
                starImage.DOColor(Color.yellow, 0.5f).SetDelay(0.1f).OnComplete(onComplete);
            }
        });
    }
}
