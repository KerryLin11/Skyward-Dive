using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void Restart()
    {
        BroAudio.Play(SFXManager.Instance.restart);
        GameManager.Instance.ResetLevel();
    }
}
