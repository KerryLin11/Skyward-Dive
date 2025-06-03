using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTimeManager : MonoBehaviour
{
    public static StarTimeManager Instance;
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

    public StarTimeData data;
    private void Update()
    {

    }
}
