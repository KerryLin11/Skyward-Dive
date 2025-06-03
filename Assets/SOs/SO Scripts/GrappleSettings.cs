using UnityEngine;

[CreateAssetMenu(fileName = "NewGrappleSettings", menuName = "GrappleSettings")]
public class GrappleSettings : ScriptableObject
{
    [Header("Spring Settings")]
    public float initialSpring = 200f;
    public float spring = 300f;
    public float initialDamper = 0f;
    public float damper = 50f;
    public float rampUpDuration = 0.2f;


    public float maxDistanceMultiplier = 0.2f;
    public float minDistanceMultiplier = 0.1f;

    [Header("Tug Settings")]
    public float tugVelocityThreshold = 1.5f;
    public float tugImpulseStrength = 1500f;
    public float dualTugExtraImpulse = 20f;
    public float dualTugWindow = 500f;

    [Header("Haptic Feedback Settings")]

    [Header("Execute Grapple")]
    public float startGrappleHapticIntensity = 0.3f;
    public float startGrappleHapticDuration = 0.1f;

    [Space(10)]
    [Header("Execute Tug")]
    public float tugHapticIntensity = 0.6f;
    public float tugHapticDuration = 0.25f;
    [Space(10)]
    [Header("Execute Dual Tug")]
    public float dualTugHapticIntensity = 1f;
    public float dualTugHapticDuration = 0.25f;


}
