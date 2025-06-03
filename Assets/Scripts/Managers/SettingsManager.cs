using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Sigtrap.VrTunnellingPro;
using System;
using UnityEngine.VFX;
using static PlayerMovement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public SettingsData settingsData;

    public void ResetDefaultSettings()
    {
        settingsData.rbMovementType = SettingsData.RBMovementType.Default;
        settingsData.antiAliasing = true;
        settingsData.rotationalVignetting = true;
        settingsData.velocityVignetting = true;
        settingsData.vignetting = 0.75f;
        settingsData.screenShake = 0.5f;
        settingsData.hapticFeedbackScaling = 1f;
        settingsData.previewLines = true;
    }

    [Header("Settings Options")]

    [ReadOnly]
    public float vignetting; // TunnellingMobile

    [ReadOnly]
    public bool rotationalVignetting; // TunnellingMobile

    [ReadOnly]
    public bool velocityVignetting; // TunnellingMobile

    [ReadOnly]
    public float screenShake; // WindAmbienceController

    [ReadOnly]
    public bool previewLines; // GrapplingGunManager

    [ReadOnly]
    public float hapticFeedbackScaling; // GrapplingGunManager

    [ReadOnly]
    public SettingsData.RBMovementType rbMovementType; // PlayerMovement

    [ReadOnly]
    public bool antiAliasing; // Project Settings


    [Header("In-game References")]
    [SerializeField] private TunnellingMobile vignettingVelocity;
    [SerializeField] private TunnellingMobile vignettingRotational;

    [SerializeField] private WindAmbienceController windAmbienceController;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GrapplingGunManager grapplingGunManager;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettingsFromScriptableObject();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeReferences();
    }

    public void InitializeReferences()
    {
        TunnellingMobile[] tunnellingComponents = Camera.main.GetComponents<TunnellingMobile>();
        if (tunnellingComponents.Length >= 2)
        {
            if (tunnellingComponents[0].useAngularVelocity == true)
            {
                vignettingRotational = tunnellingComponents[0];
                vignettingVelocity = tunnellingComponents[1];
            }
            else
            {
                vignettingRotational = tunnellingComponents[1];
                vignettingVelocity = tunnellingComponents[0];
            }
        }

        windAmbienceController = BGMManager.Instance.gameObject.GetComponent<WindAmbienceController>();
        playerMovement = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerMovement>();
        grapplingGunManager = GrapplingGunManager.Instance;



    }




    // VIMS Testing Option methods
    public void SetVignetting(float value)
    {
        settingsData.vignetting = Mathf.Clamp01(value);
    }

    public void SetRotationalVignetting(bool value)
    {
        settingsData.rotationalVignetting = value;
    }

    public void SetVelocityBasedVignetting(bool value)
    {
        settingsData.velocityVignetting = value;
    }

    public void SetScreenShake(float value)
    {
        settingsData.screenShake = Mathf.Clamp01(value);
    }

    public void SetPreviewLines(bool value)
    {
        settingsData.previewLines = value;
    }

    public void SetHapticFeedbackScaling(float value)
    {
        settingsData.hapticFeedbackScaling = Mathf.Clamp01(value);
    }


    public void SetRbMovementType(SettingsData.RBMovementType type)
    {
        settingsData.rbMovementType = type;
    }

    public void SetAntiAliasing(bool value)
    {
        settingsData.antiAliasing = value;
    }




    public void LoadSettingsFromScriptableObject()
    {
        vignetting = settingsData.vignetting;
        screenShake = settingsData.screenShake;
        previewLines = settingsData.previewLines;
        hapticFeedbackScaling = settingsData.hapticFeedbackScaling;
        rbMovementType = settingsData.rbMovementType;
        antiAliasing = settingsData.antiAliasing;
        rotationalVignetting = settingsData.rotationalVignetting;
        velocityVignetting = settingsData.velocityVignetting;
    }


    public void SaveSettingsToScriptableObject()
    {
        settingsData.vignetting = vignetting;
        settingsData.screenShake = screenShake;
        settingsData.previewLines = previewLines;
        settingsData.hapticFeedbackScaling = hapticFeedbackScaling;
        settingsData.rbMovementType = rbMovementType;
        settingsData.antiAliasing = antiAliasing;
        settingsData.rotationalVignetting = rotationalVignetting;
        settingsData.velocityVignetting = velocityVignetting;
    }

    public void LoadSettingsToGame()
    {
        // Vignetting
        vignettingRotational.effectCoverage = vignetting;
        vignettingVelocity.effectCoverage = vignetting;
        vignettingRotational.enabled = rotationalVignetting;
        vignettingVelocity.enabled = velocityVignetting;

        // VFX SFX
        windAmbienceController.SetScreenShake(screenShake);
        grapplingGunManager.SetHapticFeedbackScaling(hapticFeedbackScaling);
        if (antiAliasing) QualitySettings.antiAliasing = 8;
        else QualitySettings.antiAliasing = 0;

        // Player Movement
        try
        {
            playerMovement.SetMovementMode((MovementMode)Enum.Parse(typeof(MovementMode), settingsData.rbMovementType.ToString()));
        }
        catch (ArgumentException)
        {
            // Handle the exception for an invalid movement mode
            Debug.LogWarning($"Invalid movement mode: {settingsData.rbMovementType}. Defaulting to MovementMode.Default.");
            playerMovement.SetMovementMode(MovementMode.Default);
        }
    }

}
