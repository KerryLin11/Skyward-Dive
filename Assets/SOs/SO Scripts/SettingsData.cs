using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "SettingsData")]
public class SettingsData : ScriptableObject
{
    public enum RBMovementType
    {
        Default,
        Slippery,
        Burst,
    }

    // VIMS Testing Options
    public float vignetting;

    public bool rotationalVignetting;
    public bool velocityVignetting;

    public float screenShake;
    public bool previewLines;
    public float hapticFeedbackScaling;

    public RBMovementType rbMovementType;
    public bool antiAliasing;

}