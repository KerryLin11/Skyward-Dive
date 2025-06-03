using System.Collections;
using Ami.BroAudio;
using UnityEngine;

public class RunningSfxController : MonoBehaviour
{
    [SerializeField] private SoundSource runningSfx; // Assuming running sound effect is handled by a SoundSource
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private float volumeSmoothTime = 0.05f;
    [SerializeField] private PlayerMovement playerMovement; // Reference to PlayerMovement script

    private float maxVelocity;
    private float currentVolume;
    private float volumeVelocity;

    private void Awake()
    {
        InitializeReferences();
    }

    public void InitializeReferences()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        playerRb = playerGameObject.GetComponentInChildren<Rigidbody>();
        playerMovement = playerGameObject.GetComponentInChildren<PlayerMovement>();
        runningSfx = GameObject.FindGameObjectWithTag("Feet").GetComponent<SoundSource>();

        if (playerRb == null)
        {
            Debug.LogWarning("RunningSfxController: playerRb reference is missing");
        }

        if (runningSfx == null)
        {
            Debug.LogWarning("RunningSfxController: runningSfx reference is missing");
        }

        if (playerMovement == null)
        {
            Debug.LogWarning("RunningSfxController: playerMovement reference is missing");
        }


        runningSfx.SetPitch(0f);
        runningSfx.SetVolume(0f);
    }

    private void Start()
    {
        if (playerMovement == null || runningSfx == null || playerRb == null)
        {
            Debug.LogError("RunningSfxController: Missing references.");
            return;
        }

        maxVelocity = playerMovement.MaxVelocity - 10f;

        // Start by muting RunningSfx
        runningSfx.SetPitch(0f);
        runningSfx.SetVolume(0f);

        StartCoroutine(UpdateRunningSfx());
    }

    private IEnumerator UpdateRunningSfx()
    {

        while (true)
        {
            if (playerMovement.IsGrounded())
            {
                float velocityMagnitude = playerRb.velocity.magnitude;
                float targetVolume = Mathf.Clamp01(velocityMagnitude / (maxVelocity / 2));
                currentVolume = Mathf.SmoothDamp(currentVolume, targetVolume, ref volumeVelocity, volumeSmoothTime);
                runningSfx.SetVolume(currentVolume);
            }
            else
            {
                runningSfx.SetVolume(0f);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
