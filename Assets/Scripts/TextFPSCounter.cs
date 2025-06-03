using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextFPSCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool show = false;

    private const int targetFPS =
#if UNITY_ANDROID // GEARVR
        60;
#else
        75;
#endif
    private const float updateInterval = 0.5f;

    private int framesCount;
    private float framesTime;

    void Start()
    {
        // No text object set? See if our game object has one to use
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            show = !show;
        }

        // Monitoring frame counter and the total time
        framesCount++;
        framesTime += Time.unscaledDeltaTime;

        // Measuring interval ended, so calculate FPS and display on Text
        if (framesTime > updateInterval)
        {
            if (text != null)
            {
                if (show)
                {
                    float fps = framesCount / framesTime;
                    text.text = System.String.Format("{0:F2} FPS", fps);
                    text.color = (fps > (targetFPS - 5) ? Color.green :
                                 (fps > (targetFPS - 30) ? Color.yellow :
                                  Color.red));

                    // Debug.Log($"FPS: {fps:F2}");
                }
                else
                {
                    text.text = "";
                }
            }

            // Reset for the next interval to measure
            framesCount = 0;
            framesTime = 0;
        }
    }
}
