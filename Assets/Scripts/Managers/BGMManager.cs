using System.Collections;
using UnityEngine;
using Ami.BroAudio;
using NaughtyAttributes;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [Header("Music")]
    public SoundID bgm;
    private bool isPlaying = false;


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


    [Button("Start BGM")]
    public void StartBGM()
    {
        if (!isPlaying)
        {
            BroAudio.Play(bgm);
            isPlaying = true;
        }
    }

    [Button("Stop BGM")]
    public void StopBGM()
    {
        if (isPlaying)
        {
            BroAudio.Stop(bgm, 0.5f);
            isPlaying = false;
        }
    }
}
