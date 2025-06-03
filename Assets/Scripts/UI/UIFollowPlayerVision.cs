using UnityEngine;

public class UIFollowPlayerVision : MonoBehaviour
{
    private Transform playerCamera;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        Vector3 directionToCamera = playerCamera.position - transform.position;
        directionToCamera.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        targetRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation = targetRotation;
    }
}
