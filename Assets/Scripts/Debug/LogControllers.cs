using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LogControllers : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private XRInteractorLineVisual lineVisual;

    void Start()
    {
        // Ensure references are set
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (rayInteractor == null)
            rayInteractor = GetComponent<XRRayInteractor>();

        if (lineVisual == null)
            lineVisual = GetComponent<XRInteractorLineVisual>();
    }

    void Update()
    {
        // LogLineRenderer();
        // LogRayInteractor();
        // LogLineVisual();
    }

    private void LogLineRenderer()
    {
        if (lineRenderer != null)
        {
            Debug.Log($"LineRenderer Active: {lineRenderer.enabled}");
            Debug.Log($"LineRenderer Positions Count: {lineRenderer.positionCount}");
        }
        else
        {
            Debug.LogWarning("LineRenderer reference is missing.");
        }
    }

    private void LogRayInteractor()
    {
        if (rayInteractor != null)
        {
            Debug.Log($"RayInteractor Active: {rayInteractor.enabled}");
            Debug.Log($"RayInteractor isSelecting: {rayInteractor.isSelectActive}");
        }
        else
        {
            Debug.LogWarning("RayInteractor reference is missing.");
        }
    }

    private void LogLineVisual()
    {
        if (lineVisual != null)
        {
            Debug.Log($"LineVisual Active: {lineVisual.enabled}");
            Debug.Log($"LineVisual Line Width: {lineVisual.lineWidth}");
        }
        else
        {
            Debug.LogWarning("LineVisual reference is missing.");
        }
    }
}
