using UnityEngine;
using TMPro;
using PolyverseSkiesAsset;

public class SpeedCounterUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private PolyverseSkies polyverseSkies;

    [Header("Settings")]
    [SerializeField] private float UIUpdateInterval = 0.1f;

    private bool increasing = true;

    [Header("Speed Thresholds")]
    [SerializeField] private float slowThreshold = 50f;
    [SerializeField] private float mediumThreshold = 100f;
    [SerializeField] private float fastThreshold = 150f;
    [SerializeField] private float hyperThreshold = float.MaxValue; // No upper limit for hyper speed

    [SerializeField] private float hyperSkyBoxSpeed = 0.01f;

    private void Start()
    {
        InvokeRepeating("UpdateSpeed", 0f, UIUpdateInterval);
    }

    void UpdateSpeed()
    {
        Vector3 velocity = playerRigidbody.velocity;

        // Get the largest rounded absolute value from the x, y, or z velocity components. This is the player's speed.
        float speed = Mathf.Round(Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y), Mathf.Abs(velocity.z)));

        speedText.text = $"{speed:F0}";

        // Change color based on speed thresholds
        if (speed <= slowThreshold)
        {
            speedText.color = Color.green;  // Green for speed <= 50
        }
        else if (speed <= mediumThreshold)
        {
            speedText.color = Color.yellow;  // Yellow for speed > 50 and <= 100
        }
        else if (speed <= fastThreshold)
        {
            speedText.color = Color.red;  // Red for speed > 100 and <= 150
        }
        else
        {
            speedText.color = Color.black;  // Black for speed > 150
        }

        // Update skybox if in hyper speed range
        if (speed > hyperThreshold)
        {
            if (polyverseSkies != null)
            {
                if (increasing)
                {
                    if (polyverseSkies.timeOfDay >= 1)
                    {
                        increasing = false;
                    }

                    polyverseSkies.timeOfDay += hyperSkyBoxSpeed;
                }
                else
                {
                    if (polyverseSkies.timeOfDay <= 0)
                    {
                        increasing = true;
                    }

                    polyverseSkies.timeOfDay -= hyperSkyBoxSpeed;
                }
            }
        }
    }
}
