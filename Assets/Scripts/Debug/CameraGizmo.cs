using UnityEngine;

[ExecuteAlways]
public class CameraGizmo : MonoBehaviour
{
    public float fov = 60f;
    public float range = 10f;
    public Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3 forward = transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + forward * range);

        Vector3 rightBoundary = Quaternion.Euler(0, fov / 2, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -fov / 2, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * range);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * range);
    }
}
