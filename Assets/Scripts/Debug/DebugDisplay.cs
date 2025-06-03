using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    private Dictionary<string, List<string>> debugLogs = new Dictionary<string, List<string>>();
    [SerializeField] private TextMeshProUGUI display;

    [SerializeField] private int maxLogCount = 7;


    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logTypeString = type.ToString();
        string formattedLog = $"[{logTypeString}] {logString}";

        if (!debugLogs.ContainsKey(logTypeString))
        {
            debugLogs[logTypeString] = new List<string>();
        }

        debugLogs[logTypeString].Add(formattedLog);
        if (debugLogs[logTypeString].Count > maxLogCount)
        {
            debugLogs[logTypeString].RemoveAt(0);
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        string displayText = "";

        foreach (KeyValuePair<string, List<string>> log in debugLogs)
        {
            displayText += $"{log.Key}:\n";
            foreach (string entry in log.Value)
            {
                displayText += entry + "\n";
            }
            displayText += "\n";
        }

        if (display != null && display.text != displayText)
        {
            display.text = displayText;
        }
        else if (display == null)
        {
            Debug.LogWarning("TextMeshProUGUI component is not assigned.");
        }
    }
}
