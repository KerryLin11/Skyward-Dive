using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Ami.BroAudio;
using NaughtyAttributes;
using UnityEngine.XR.Interaction.Toolkit;

public class CountdownController : MonoBehaviour
{
    public static CountdownController Instance { get; private set; }
    [SerializeField] private int initialCountdownTime = 3;
    private int currentTime;

    [SerializeField] private float countdownSpeed = 1f;  // How fast the timer counts down in seconds. (Default 1s = 1s)
    [SerializeField] private TextMeshProUGUI countdownDisplay;

    [SerializeField] private Image panelImage;
    [SerializeField] private PlayerMovement playerMovement;
    private SFXManager sfXManager;


    private float initialPanelImageAlpha;



    private void Start()
    {
        Instance = this;
        sfXManager = SFXManager.Instance;

        initialPanelImageAlpha = panelImage.color.a;

        playerMovement = GameObject.Find("Player").GetComponentInChildren<PlayerMovement>();
        playerMovement.enabled = false;
        FindObjectOfType<XRInteractionManager>().enabled = false;
        StartCountdown();
    }

    [Button]
    public void StartCountdown()
    {
        StartCoroutine(CountdownToStart(countdownSpeed));
    }

    public IEnumerator CountdownToStart(float speed)
    {
        TimerController.Instance.EndTimer();
        TimerController.Instance.ResetTimer();

        currentTime = initialCountdownTime;
        countdownDisplay.gameObject.SetActive(true);
        panelImage.gameObject.SetActive(true);
        while (currentTime > 0)
        {
            BroAudio.Play(sfXManager._321);

            countdownDisplay.text = currentTime.ToString();

            yield return new WaitForSeconds(speed);

            currentTime--;
        }

        // When the timer is up
        BroAudio.Play(sfXManager.go);

        BGMManager.Instance.StartBGM();
        TimerController.Instance.BeginTimer();
        playerMovement.enabled = true;
        FindObjectOfType<XRInteractionManager>().enabled = true;

        countdownDisplay.text = "GO!";
        panelImage.DOFade(0f, 1f);
        countdownDisplay.DOFade(0f, 0.8f);

        yield return new WaitForSeconds(speed);

        panelImage.DOFade(initialPanelImageAlpha, 0f);
        countdownDisplay.DOFade(1f, 0f);

        panelImage.gameObject.SetActive(false);
        countdownDisplay.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
