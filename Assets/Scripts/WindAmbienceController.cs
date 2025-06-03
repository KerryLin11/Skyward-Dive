using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using NaughtyAttributes;
using UnityEngine;

public class WindAmbienceController : MonoBehaviour
{
    [SerializeField] private SoundSource windSfx;
    [SerializeField] private Rigidbody playerRb;
    private GameObject playerGameObject;

    private void Awake()
    {
        InitializeReferences();
    }
    private void Start()
    {
        maxVelocity = playerGameObject.GetComponentInChildren<PlayerMovement>().MaxVelocity - 10f;

        // Start by muting the wind ambience
        windSfx.SetPitch(0f);
        windSfx.SetVolume(0f);

        StartCoroutine(UpdateWindAmbience());
    }



    [Header("Volume and Pitch Settings")]
    [SerializeField] private float windSfxUpdateInterval = 0.2f;
    [Range(0f, 10f)][SerializeField] private float minVolume = 0.0f;

    [Range(0f, 10f)][SerializeField] private float maxVolume = 1.0f;

    [Range(0f, 3f)][SerializeField] private float minPitch = 0.0f;

    [Range(0f, 3f)][SerializeField] private float maxPitch = 3.0f;

    [Range(0.01f, 1f)][SerializeField] private float volumeSmoothTime = 0.1f;

    [Range(0.01f, 1f)][SerializeField] private float pitchSmoothTime = 0.1f;

    [Header("Velocity Settings")]
    public float minVelocity = 0f;

    public float maxVelocity = 150f;

    [Range(0f, 100f)][SerializeField] private float minYVelocity = 0f;

    [Range(0f, 100f)][SerializeField] private float maxYVelocity = 10f;

    [Header("Reached Max Velocity Settings")]
    [Range(0f, 3f)][SerializeField] private float hyperSpeedVolumeFactor = 1.75f;
    [Header("Screen Shake after reaching Max Velocity")]
    [Range(0f, 1f)][SerializeField] private float screenShakeMagnitude = 0.5f;
    public void SetScreenShake(float value) => screenShakeMagnitude = value;


    // [ReadOnly]
    [SerializeField] private float currentVolume;
    private float currentPitch;
    private float volumeVelocity;
    private float pitchVelocity;
    private IEnumerator UpdateWindAmbience()
    {
        while (true)
        {
            if (playerRb != null && windSfx != null)
            {
                float velocityMagnitude = playerRb.velocity.magnitude;
                float verticalVelocity = playerRb.velocity.y;

                if (velocityMagnitude > maxVelocity)
                {
                    ScreenShakeVR.Instance.Shake(screenShakeMagnitude, windSfxUpdateInterval);

                    ApplyWindSettings(maxVolume * hyperSpeedVolumeFactor, maxVolume, minPitch, maxPitch, minVelocity, maxVelocity, verticalVelocity, velocityMagnitude);
                }
                else
                {
                    ApplyWindSettings(minVolume, maxVolume, minPitch, maxPitch, minVelocity, maxVelocity, verticalVelocity, velocityMagnitude);
                }
            }
            yield return new WaitForSeconds(windSfxUpdateInterval);
        }
    }

    // Calculates volume and pitch based on velocity
    private void ApplyWindSettings(float minVol, float maxVol, float minPitch, float maxPitch, float minVel, float maxVel, float verticalVelocity, float velocityMagnitude)
    {
        // Calculate target volume
        // Clamped from minVolume to maxVolume based on velocity magnitude
        // Clamped between minVelocity and maxVelocity
        float targetVolume = Mathf.Clamp(
            Mathf.InverseLerp(minVel, maxVel, velocityMagnitude),
            minVol,
            maxVol
        );

        // Calculate target pitch
        // Clamped from minPitch to maxPitch based on velocity.y
        // Clamped between minYVelocity and maxYVelocity
        float targetPitch = Mathf.Lerp(
            minPitch,
            maxPitch,
            Mathf.InverseLerp(minYVelocity, maxYVelocity, verticalVelocity)
        );

        // Smoothly transition the volume and pitch
        currentVolume = Mathf.SmoothDamp(currentVolume, targetVolume, ref volumeVelocity, volumeSmoothTime);
        currentPitch = Mathf.SmoothDamp(currentPitch, targetPitch, ref pitchVelocity, pitchSmoothTime);

        // Set pitch and volume
        windSfx.SetVolume(currentVolume);
        windSfx.SetPitch(currentPitch);
    }

    public void InitializeReferences()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        playerRb = playerGameObject.GetComponentInChildren<Rigidbody>();
        if (playerRb == null)
        {
            Debug.LogWarning("WindAmbienceController: playerRb reference is missing");
        }

        if (windSfx == null)
        {
            Debug.LogWarning("WindAmbienceController: windSfx reference is missing");
        }
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
