using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PauseSettings : MonoBehaviour
{
    [SerializeField] private GameObject finishLevelUICanvas;
    [SerializeField] private Image optionsMenu;
    [SerializeField] private InputActionProperty rightB_Button;

    private void Start()
    {
        optionsMenu.transform.localScale = Vector3.zero;

        FinishLevelUI finishLevelUIComponent = FindObjectOfType<FinishLevelUI>();
        if (finishLevelUIComponent != null)
        {
            finishLevelUICanvas = finishLevelUIComponent.gameObject;
        }
        else
        {
            Debug.LogWarning("FinishLevelUI not found");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space to test the animation
        {
            optionsMenu.transform.DOScale(new Vector3(2.5f, 1.2f, 1f), 0.5f)
                .SetEase(Ease.OutBounce)
                .OnStart(() => Debug.Log("Test Animation started"))
                .OnComplete(() => Debug.Log("Test Animation completed"));
        }
    }

    public void BackButtonClicked()
    {
        SettingsManager.Instance.LoadSettingsFromScriptableObject();
        SettingsManager.Instance.LoadSettingsToGame();

        HidePauseSettings();

    }

    public void HidePauseSettings()
    {
        finishLevelUICanvas.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
        optionsMenu.transform.localScale = Vector3.zero;

        rightB_Button.action.Enable();
    }

    public void ShowPauseSettings()
    {
        finishLevelUICanvas.transform.localScale = Vector3.zero;
        optionsMenu.transform.localScale = new Vector3(2.5f, 1.2f, 1f);

        rightB_Button.action.Disable();
    }
}
