using Ami.BroAudio;
using UnityEngine;

public class SpeedBoostTube : MonoBehaviour
{
    public float boostForce = 30f;
    public Vector3 tubeDirection = Vector3.forward;
    public bool maintainPlayerControl = false;

    private Rigidbody playerRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerRb = other.GetComponentInParent<Rigidbody>();
            if (playerRb != null)
            {
                BroAudio.Play(SFXManager.Instance.suck);
                BoostPlayer(playerRb);
            }
            else
            {
                Debug.LogWarning("SpeedBoostTube: playerRb reference is missing");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (playerRb != null)
            {
                BoostPlayer(playerRb);
            }
        }
    }

    private void BoostPlayer(Rigidbody playerRb)
    {
        Vector3 forceDirection = transform.TransformDirection(tubeDirection.normalized);

        if (maintainPlayerControl)
        {
            playerRb.AddForce(forceDirection * boostForce * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            playerRb.velocity = forceDirection * boostForce;
        }
    }
}
