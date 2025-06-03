using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using HeneGames.Sceneloader;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Image aboutPanel;
    public TextMeshProUGUI aboutDescription;
    public Image exitPanel;
    public Image optionsPanel;
    public Image optionsMenu;
    public Image startPanel;
    public Image backPanel;
    public GameObject loadingScreen;

    private Vector3 originalAboutScale, originalExitScale, originalOptionsScale, originalStartScale, originalBackScale;

    private void Start()
    {
        originalAboutScale = aboutPanel.transform.localScale;
        originalExitScale = exitPanel.transform.localScale;
        originalOptionsScale = optionsPanel.transform.localScale;
        originalStartScale = startPanel.transform.localScale;
        originalBackScale = backPanel.transform.localScale;

        // Initialize panels and menu scales
        backPanel.transform.localScale = Vector3.zero;
        optionsMenu.transform.localScale = Vector3.zero; // Set optionsMenu to zero scale

        aboutDescription.color = new Color(aboutDescription.color.r, aboutDescription.color.g, aboutDescription.color.b, 0);
    }

    public void AboutClicked()
    {
        exitPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);
        optionsPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);
        startPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);

        aboutPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);

        aboutDescription.DOFade(1, 1f);

        backPanel.transform.DOScale(originalBackScale, 0.5f).SetEase(Ease.OutQuad).SetDelay(0.3f);
    }

    public void OptionsClicked()
    {
        backPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);

        exitPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        startPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        aboutPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        optionsPanel.transform.DOScale(originalOptionsScale, 0.2f).SetEase(Ease.OutBounce)
    .OnComplete(() => optionsPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

        DOVirtual.DelayedCall(0.5f, () =>
        {
            optionsMenu.transform.DOScale(new Vector3(2.5f, 1.2f, 1f), 0.5f).SetEase(Ease.OutBounce);

        });
    }



    public void StartClicked()
    {
        exitPanel.transform.DORotate(new Vector3(0, 0, 360), 0.5f).SetEase(Ease.InOutBack)
            .OnComplete(() => exitPanel.transform.DOScale(Vector3.zero, 0.5f));

        optionsPanel.transform.DORotate(new Vector3(0, 0, 360), 0.5f).SetEase(Ease.InOutBack)
            .OnComplete(() => optionsPanel.transform.DOScale(Vector3.zero, 0.5f));

        aboutPanel.transform.DORotate(new Vector3(0, 0, 360), 0.5f).SetEase(Ease.InOutBack)
            .OnComplete(() => aboutPanel.transform.DOScale(Vector3.zero, 0.5f));

        startPanel.transform.DOScale(originalStartScale, 0.5f).SetEase(Ease.OutCubic)
            .OnComplete(() => startPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

        backPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad)
            .SetDelay(0.3f)
            .OnComplete(() =>
            {
                //! Delay before loading scene
                DOVirtual.DelayedCall(0.5f, () => loadingScreen.GetComponent<LoadingScreen>().LoadScene(1));
            });
    }

    public void BackClicked()
    {
        exitPanel.transform.DOScale(originalExitScale, 0.5f).SetEase(Ease.OutQuad);
        optionsPanel.transform.DOScale(originalOptionsScale, 0.5f).SetEase(Ease.OutQuad);
        startPanel.transform.DOScale(originalStartScale, 0.5f).SetEase(Ease.OutQuad);
        aboutPanel.transform.DOScale(originalAboutScale, 0.5f).SetEase(Ease.OutQuad);

        aboutDescription.DOFade(0, 0.3f);

        backPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad).SetDelay(0.05f);
    }

    public void OptionsBackClicked()
    {
        SettingsManager.Instance.LoadSettingsFromScriptableObject();
        SettingsManager.Instance.LoadSettingsToGame();

        optionsMenu.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            optionsPanel.transform.DOScale(originalOptionsScale, 0.5f).SetEase(Ease.OutQuad);

            exitPanel.transform.DOScale(originalExitScale, 0.5f).SetEase(Ease.OutQuad);
            startPanel.transform.DOScale(originalStartScale, 0.5f).SetEase(Ease.OutQuad);
            aboutPanel.transform.DOScale(originalAboutScale, 0.5f).SetEase(Ease.OutQuad);

            backPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad).SetDelay(0.05f);
        });
    }


    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
