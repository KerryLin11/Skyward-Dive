using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[CreateAssetMenu(fileName = "StarTimeData", menuName = "Star Time Data")]
public class StarTimeData : ScriptableObject
{
    [Tooltip("Star times for each level")]
    public List<LevelStarTimes> levels;

    public List<LevelStarTimes> GetStarTimes() => levels;

    public float GetTime(int levelIndex, int starNumber)
    {
        // Invalid input
        if (starNumber != 2 && starNumber != 3)
        {
            Debug.LogError("Invalid star number");
            return -1;
        }
        else if (levelIndex >= levels.Count || levelIndex < 0)
        {
            {
                Debug.LogError("Invalid level index");
                return -1;
            }
        }


        if (starNumber == 2)
        {
            return levels[levelIndex].two;
        }
        else if (starNumber == 3)
        {
            return levels[levelIndex].three;
        }
        else
        {
            Debug.LogError("Invalid star number");
            return -1;
        }


    }




    [System.Serializable]
    public struct LevelStarTimes
    {
        [Tooltip("Three-star time (seconds)")]
        public float two;

        [Tooltip("Two-star time (seconds)")]
        public float three;
    }
}