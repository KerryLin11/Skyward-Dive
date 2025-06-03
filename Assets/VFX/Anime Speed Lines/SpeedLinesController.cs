using UnityEngine;

public class SpeedLinesController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Material speedLinesMaterial;
    private string maskScaleProperty = "_MaskScale";

    private float minVelocity = 0f;
    private float maxVelocity = 150f;
    private float minMaskScale = 0.8f;
    private float maxMaskScale = 2f;

    [SerializeField] private float updateInterval = 0.1f;

    void Start()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();
        InvokeRepeating(nameof(UpdateMaskScale), 0f, updateInterval);
    }

    void UpdateMaskScale()
    {
        float currentVelocity = playerRigidbody.velocity.magnitude;

        float t = Mathf.InverseLerp(minVelocity, maxVelocity, currentVelocity);
        float maskScale = Mathf.Lerp(minMaskScale, maxMaskScale, 1 - t);

        speedLinesMaterial.SetFloat(maskScaleProperty, maskScale);
    }
}
