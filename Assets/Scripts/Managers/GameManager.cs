using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject playerGameObject;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private int currentLevel;
    public int GetCurrentLevel() => currentLevel;

    private void Start()
    {
        InitializeDDOL();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Scene index: " + currentLevel);
    }


    private void InitializeReferences()
    {
        playerGameObject = GameObject.FindWithTag("Player");
    }


    //! Whenever a scene is loaded, call OnSceneLoaded
    //! sceneLoaded is subscribed to OnSceneLoaded (which atm just initializes all singletons and their components)
    [Button("Reset Level")]
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [Button("InitializeDDOL")]
    public void InitializeDDOL()
    {


        InitializeReferences();
        BGMManager.Instance.gameObject.GetComponent<RunningSfxController>().InitializeReferences();
        BGMManager.Instance.gameObject.GetComponent<WindAmbienceController>().InitializeReferences();
        SettingsManager.Instance.InitializeReferences();
        GrapplingGunManager.Instance.InitializeReferences();

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop any active BGM, reset any low-pass filters
        BGMManager.Instance.StopBGM();
        BroAudio.SetEffect(Effect.ResetLowPass(1f));

        // Reset the fucking timeScale -2 hours absolutely fantastic
        Time.timeScale = 1f;

        // Kill all active tweens 
        DOTween.KillAll();

        InitializeDDOL();

        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    [Button]
    private void StartCountdown()
    {
        CountdownController.Instance.StartCountdown();
    }

    [Button]
    private void FinishLevel()
    {
        FinishLine.Instance.Finish();
    }

}
