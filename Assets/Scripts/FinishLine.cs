using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public static FinishLine Instance { get; private set; }

    [SerializeField] private StarTimeData starTimeData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Button]
    public void Finish()
    {
        BroAudio.Play(SFXManager.Instance.levelComplete_1);

        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerMovement>().enabled = false;

        TimerController.Instance.EndTimer();

        StartCoroutine(WaitAndExecuteNextAction());
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Player") & (1 << other.gameObject.layer)) != 0)
        {
            // DOVirtual.DelayedCall(0.5f, () => BroAudio.SetEffect(Effect.LowPass(3000f, 1f)));

            BroAudio.Play(SFXManager.Instance.levelComplete_1);

            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerMovement>().enabled = false;

            TimerController.Instance.EndTimer();

            StartCoroutine(WaitAndExecuteNextAction());
        }
    }

    private IEnumerator WaitAndExecuteNextAction()
    {
        yield return new WaitForSeconds(1f);
        FinishLineTransition();
    }

    public static event Action OnFinishLineReached;

    private void FinishLineTransition()
    {
        //TODO: Transition to finish level UI in game manager

        OnFinishLineReached?.Invoke();
    }

}
