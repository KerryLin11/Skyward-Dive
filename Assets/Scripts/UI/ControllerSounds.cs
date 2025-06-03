using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ami.BroAudio;
public class ControllerSounds : MonoBehaviour
{
    public void Hover()
    {
        BroAudio.Play(SFXManager.Instance.hoverOverButton);
    }

    public void Click()
    {
        BroAudio.Play(SFXManager.Instance.clickedButton);
    }

}
