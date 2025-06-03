using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    // Reference to UI sliders and toggles
    public Scrollbar vignettingSlider;
    public Toggle rotationalVignettingToggle;
    public Toggle velocityVignettingToggle;
    public Scrollbar screenShakeSlider;
    public Scrollbar hapticFeedbackSlider;

    public Toggle previewLinesToggle;
    public TMP_Dropdown rbMovementTypeDropdown;
    public Toggle antiAliasingToggle;

    public SettingsData defaultSettingsData;

    private void Start()
    {
        // Load current settings into UI elements
        vignettingSlider.value = SettingsManager.Instance.settingsData.vignetting;
        rotationalVignettingToggle.isOn = SettingsManager.Instance.settingsData.rotationalVignetting;
        velocityVignettingToggle.isOn = SettingsManager.Instance.settingsData.velocityVignetting;

        screenShakeSlider.value = SettingsManager.Instance.settingsData.screenShake;
        hapticFeedbackSlider.value = SettingsManager.Instance.settingsData.hapticFeedbackScaling;

        previewLinesToggle.isOn = SettingsManager.Instance.settingsData.previewLines;

        rbMovementTypeDropdown.value = (int)SettingsManager.Instance.settingsData.rbMovementType;
        antiAliasingToggle.isOn = SettingsManager.Instance.settingsData.antiAliasing;

        // Add listeners to sliders and toggles
        vignettingSlider.onValueChanged.AddListener(OnVignettingSliderChanged);
        rotationalVignettingToggle.onValueChanged.AddListener(OnRotationalVignettingToggleChanged);
        velocityVignettingToggle.onValueChanged.AddListener(OnVelocityVignettingToggleChanged);

        screenShakeSlider.onValueChanged.AddListener(OnScreenShakeSliderChanged);
        hapticFeedbackSlider.onValueChanged.AddListener(OnHapticFeedbackSliderChanged);

        previewLinesToggle.onValueChanged.AddListener(OnPreviewLinesToggleChanged);
        rbMovementTypeDropdown.onValueChanged.AddListener(OnRbMovementTypeDropdownChanged);
        antiAliasingToggle.onValueChanged.AddListener(OnAntiAliasingToggleChanged);
    }

    public void OnVignettingSliderChanged(float value)
    {
        SettingsManager.Instance.SetVignetting(value);
    }

    public void OnRotationalVignettingToggleChanged(bool value)
    {
        SettingsManager.Instance.SetRotationalVignetting(value);
    }

    public void OnVelocityVignettingToggleChanged(bool value)
    {
        SettingsManager.Instance.SetVelocityBasedVignetting(value);
    }

    public void OnScreenShakeSliderChanged(float value)
    {
        SettingsManager.Instance.SetScreenShake(value);
    }

    public void OnPreviewLinesToggleChanged(bool value)
    {
        SettingsManager.Instance.SetPreviewLines(value);
    }

    public void OnHapticFeedbackSliderChanged(float value)
    {
        SettingsManager.Instance.SetHapticFeedbackScaling(value);
    }

    public void OnRbMovementTypeDropdownChanged(int index)
    {
        // Convert the dropdown index to the corresponding enum
        SettingsData.RBMovementType selectedType = (SettingsData.RBMovementType)index;
        SettingsManager.Instance.SetRbMovementType(selectedType);
    }

    public void OnAntiAliasingToggleChanged(bool value)
    {
        SettingsManager.Instance.SetAntiAliasing(value);
    }

    // Load current settings into UI
    private void ResetToDefaultSettingsUI()
    {
        vignettingSlider.value = SettingsManager.Instance.settingsData.vignetting;
        rotationalVignettingToggle.isOn = SettingsManager.Instance.settingsData.rotationalVignetting;
        velocityVignettingToggle.isOn = SettingsManager.Instance.settingsData.velocityVignetting;
        screenShakeSlider.value = SettingsManager.Instance.settingsData.screenShake;
        hapticFeedbackSlider.value = SettingsManager.Instance.settingsData.hapticFeedbackScaling;
        previewLinesToggle.isOn = SettingsManager.Instance.settingsData.previewLines;
        rbMovementTypeDropdown.value = (int)SettingsManager.Instance.settingsData.rbMovementType;
        antiAliasingToggle.isOn = SettingsManager.Instance.settingsData.antiAliasing;
    }

    public void ResetDefaultSettings()
    {
        SettingsManager.Instance.ResetDefaultSettings();

        // Update the UI to reflect the default settings
        ResetToDefaultSettingsUI();
    }
}
