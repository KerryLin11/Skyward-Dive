using UnityEngine;
using Ami.BroAudio;
using DG.Tweening;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Player") & (1 << other.gameObject.layer)) != 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerMovement>().enabled = false;
            TimerController.Instance.EndTimer();
            BroAudio.Play(SFXManager.Instance.restart);
            DOVirtual.DelayedCall(0.5f, () => GameManager.Instance.ResetLevel()).SetUpdate(true);
        }
    }
}
