using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class EventSystemChecker : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Ensure EventSystem isn't destroyed between scene loads (for some reason it does)


        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.AddComponent<XRUIInputModule>();
        }
    }
}
