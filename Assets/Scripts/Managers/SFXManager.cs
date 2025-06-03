using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ami.BroAudio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Audio References")]
    [Header("Gameplay Sounds")]
    public SoundID starElapsed_2;
    public SoundID starElapsed_3;
    public SoundID starTimer;
    public SoundID doubleJump;
    public SoundID droppedGrapple;
    public SoundID dualGrapplesLatched;
    public SoundID dualTugSuccessful;
    public SoundID grappleMissed;
    public SoundID grappleSuccessful;
    public SoundID jump;
    public SoundID landedOnGround;
    public SoundID levelComplete_1;
    public SoundID levelComplete_2;
    public SoundID pickedUpGrapple;
    public SoundID tugSuccessful;
    public SoundID ungrapple;
    public SoundID wind;
    public SoundID wallRunDetected;
    public SoundID suck;

    [Header("UI Sounds")]
    public SoundID clickedButton;
    public SoundID hoverOverButton;
    public SoundID levelSelected;
    public SoundID restart;
    public SoundID star_1;
    public SoundID star_2;
    public SoundID star_3;

    [Header("Countdown Timer")]
    public SoundID _321;
    public SoundID go;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
