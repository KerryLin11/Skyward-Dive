using System;
using System.Collections;
using TMPro;
using UnityEngine;
using NaughtyAttributes;
using Ami.BroAudio;

public class TimerController : MonoBehaviour
{
    public static TimerController Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI timeCounter;
    public TextMeshProUGUI TimeCounter => timeCounter;

    [SerializeField] private float Star3WarningDuration = 2f;
    [SerializeField] private float Star2WarningDuration = 1f;
    private TimeSpan timePlaying;
    private bool timerGoing;
    private bool isPaused;

    private float elapsedTime;

    private StarTimeManager starTimeManager;
    private GameManager gameManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        starTimeManager = StarTimeManager.Instance;
        gameManager = GameManager.Instance;

        timeCounter.text = "Time: 00:00.00";
        timerGoing = false;
        isPaused = false;


        // Threshold reached timer
        threeStarWarningReached = false;
        twoStarWarningReached = false;

        threeStarPassed = false;
        twoStarPassed = false;

    }


    [Button]
    public void BeginTimer()
    {
        if (!timerGoing)
        {
            timerGoing = true;
            elapsedTime = 0f;
            StartCoroutine(UpdateTimer());

            isPaused = false;

            // Threshold reached timer
            threeStarWarningReached = false;
            twoStarWarningReached = false;

            threeStarPassed = false;
            twoStarPassed = false;
        }
    }


    [Button]
    public void EndTimer()
    {
        timerGoing = false;
    }


    [Button]
    public void PauseTimer()
    {
        if (timerGoing && !isPaused)
        {
            isPaused = true;
        }
    }

    [Button]
    public void ResumeTimer()
    {
        if (timerGoing && isPaused)
        {
            isPaused = false;
            StartCoroutine(UpdateTimer());
        }
    }

    [Button]
    public void ResetTimer()
    {
        timerGoing = false;
        isPaused = false;
        elapsedTime = 0f;
        timeCounter.text = "Time: 00:00.00";

        threeStarWarningReached = false;
        twoStarWarningReached = false;

        threeStarPassed = false;
        twoStarPassed = false;
    }


    private bool threeStarPassed;
    private bool twoStarPassed;

    public bool ThreeStarPassed => threeStarPassed;
    public bool TwoStarPassed => twoStarPassed;

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            if (!isPaused)
            {
                elapsedTime += Time.deltaTime;
                timePlaying = TimeSpan.FromSeconds(elapsedTime);
                string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
                timeCounter.text = timePlayingStr;


                float threeStarTime = starTimeManager.data.GetTime(gameManager.GetCurrentLevel(), 3);
                float twoStarTime = starTimeManager.data.GetTime(gameManager.GetCurrentLevel(), 2);

                // Start 3 star warning //! Note: Warning timers are NOT star times
                if (!threeStarWarningReached && !twoStarWarningReached && elapsedTime >= threeStarTime - Star3WarningDuration)
                {
                    threeStarWarningReached = true;
                    StartCoroutine(HandleStarTimer(3));
                }

                // Star 2 star warning
                if (!twoStarWarningReached && elapsedTime >= twoStarTime - Star2WarningDuration)
                {
                    twoStarWarningReached = true;
                    StartCoroutine(HandleStarTimer(2));
                }



                // Check if the 3-star and 2-star times have been passed
                threeStarPassed = elapsedTime >= threeStarTime;
                twoStarPassed = elapsedTime >= twoStarTime;
            }

            yield return null;
        }
    }

    private IEnumerator HandleStarTimer(int starLevel)
    {
        BroAudio.Play(SFXManager.Instance.starTimer);

        if (starLevel == 3)
        {
            yield return new WaitForSeconds(Star3WarningDuration);
            BroAudio.Play(SFXManager.Instance.starElapsed_3);
        }
        else
        {
            yield return new WaitForSeconds(Star2WarningDuration);
            BroAudio.Play(SFXManager.Instance.starElapsed_2);
        }

        BroAudio.Stop(SFXManager.Instance.starTimer);
    }

    private bool threeStarWarningReached;

    private bool twoStarWarningReached;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
