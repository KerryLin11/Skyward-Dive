using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using NaughtyAttributes;

public class LevelSelect : MonoBehaviour
{
    public Button[] levelButtons;
    public Button playLevelButton;
    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;
    private int selectedLevelIndex = -1; // No level selected initially

    void Start()
    {
        if (!DOTween.IsTweening(null))
        {
            DOTween.Init();
        }

        InitializeLevelSelect();
    }

    public void InitializeLevelSelect()
    {
        Debug.Log("Initializing Level Select");

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].transform.localScale = new Vector3(.722f, .722f, 1f); // Reset scale
            levelButtons[i].GetComponent<Image>().color = defaultColor; // Reset color
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[i].onClick.RemoveAllListeners(); // Remove existing listeners
            levelButtons[i].onClick.AddListener(() => SelectLevel(index)); // Add listener to select level
        }

        // Remove and re-add listener for play button
        playLevelButton.onClick.RemoveAllListeners();
        playLevelButton.onClick.AddListener(PlayLevel);

        // Reset play button
        playLevelButton.interactable = false;
        playLevelButton.GetComponent<Image>().color = defaultColor;
    }


    void SelectLevel(int index)
    {
        if (selectedLevelIndex >= 0)
        {
            AnimateLevelButton(levelButtons[selectedLevelIndex], false);
        }

        selectedLevelIndex = index;
        AnimateLevelButton(levelButtons[selectedLevelIndex], true);

        playLevelButton.GetComponent<Image>().color = selectedColor;
        playLevelButton.interactable = true;
    }

    void AnimateLevelButton(Button button, bool isSelected)
    {
        if (isSelected)
        {
            button.transform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBounce);
            button.GetComponent<Image>().DOColor(Color.yellow, 0.2f);
        }
        else
        {
            button.transform.DOScale(.722f, 0.2f);
            button.GetComponent<Image>().DOColor(Color.white, 0.2f);
        }
    }

    public void PlayLevel()
    {
        if (selectedLevelIndex >= 0)
        {
            // Scene 0 = Main Menu
            // Scene 1 = Level Select
            // Scene 2 = Tutorial Scene
            // Scene 3 = Level 1
            // Scene 4 = Level 2
            // Scene 5 = Level 3... etc
            SceneManager.LoadScene(selectedLevelIndex + 4);
        }
    }
}
