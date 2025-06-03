using System;
using System.Collections;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrapplingGunManager : MonoBehaviour
{
    public static GrapplingGunManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GrappleSettings grappleSettings;
    [SerializeField] private Rigidbody rb; // Player rb
    private SFXManager sfxManager;

    [Header("Hand Controllers")]
    [SerializeField] private Transform leftHandControllerTransform;
    [SerializeField] private Transform rightHandControllerTransform;

    [Header("XRBase HandControllers")]
    [SerializeField] private XRBaseController XRLeftController;
    [SerializeField] private XRBaseController XRRightController;

    private ControllerVelocity leftControllerVelocity;
    private ControllerVelocity rightControllerVelocity;

    [Header("Current Grappling Guns")]
    [SerializeField] private GrapplingGun leftGrapplingGun;
    [SerializeField] private GrapplingGun rightGrapplingGun;

    [Header("Dual Tug")]
    private float leftTugTime = -Mathf.Infinity;
    private float rightTugTime = -Mathf.Infinity;



    // Getters
    public GrapplingGun GetLeftGrapplingGun() => leftGrapplingGun;
    public GrapplingGun GetRightGrapplingGun() => rightGrapplingGun;
    public ControllerVelocity GetLeftControllerVelocity() => leftControllerVelocity;
    public ControllerVelocity GetRightControllerVelocity() => rightControllerVelocity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        InitializeReferences();
    }


    private void Update()
    {
        // Check if both hands performed a tug within the time window
        if (Time.time - leftTugTime < grappleSettings.dualTugWindow && Time.time - rightTugTime < grappleSettings.dualTugWindow)
        {
            ApplyDualTugImpulse();
            // Reset tug times after applying the impulse
            leftTugTime = -Mathf.Infinity;
            rightTugTime = -Mathf.Infinity;
        }
    }

    #region Dual Tug

    public void OnTug(bool isLeftHand)
    {
        if (isLeftHand)
        {
            leftTugTime = Time.time;
        }
        else
        {
            rightTugTime = Time.time;
        }
    }

    private void ApplyDualTugImpulse()
    {
        Debug.Log("Applying Dual Tug Impulse");
        if (leftGrapplingGun != null && rightGrapplingGun != null)
        {
            leftGrapplingGun.PerformDualTug();
            rightGrapplingGun.PerformDualTug();
        }
    }


    #endregion






    private Coroutine hapticCoroutine;
    public float HapticFeedbackScaling { get; set; } = 1.0f;
    public void SetHapticFeedbackScaling(float scaling)
    {
        HapticFeedbackScaling = Mathf.Clamp(scaling, 0.0f, 1.0f);
    }



    public void StopLerpingHaptic()
    {
        if (hapticCoroutine != null)
        {
            StopCoroutine(hapticCoroutine);
        }
    }

    // Wait for 0.1s to ensure no overlapping between lerping haptic and regular haptic
    private IEnumerator DelayedHapticFeedback(XRBaseController controller, float intensity, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        controller.SendHapticImpulse(intensity, duration);
    }

    public void PerformHapticFeedback(bool isLeftHand, float intensity, float duration)
    {
        XRBaseController controller = isLeftHand ? XRLeftController : XRRightController;

        if (controller != null)
        {
            float scaledIntensity = intensity * HapticFeedbackScaling;
            StartCoroutine(DelayedHapticFeedback(controller, scaledIntensity, duration, 0.1f));
        }
    }



    public void StartLerpingHaptic(bool isLeftHand, float startIntensity, float endIntensity, float duration)
    {
        XRBaseController controller = isLeftHand ? XRLeftController : XRRightController;

        if (hapticCoroutine != null)
        {
            StopCoroutine(hapticCoroutine);  // Stop any ongoing haptic coroutine
        }
        hapticCoroutine = StartCoroutine(LerpHapticFeedback(controller, startIntensity, endIntensity, duration));
    }


    // Lerp haptic feedback every 0.1s. 
    // When finished lerping, apply endIntensity indefinitely
    private IEnumerator LerpHapticFeedback(XRBaseController controller, float startIntensity, float endIntensity, float duration)
    {
        float timeElapsed = 0f;
        float hapticStepDuration = 0.1f;
        bool isLerping = true;

        while (isLerping)
        {
            float lerpFactor = timeElapsed / duration;
            float currentIntensity = Mathf.Lerp(startIntensity, endIntensity, lerpFactor);

            // Apply haptic feedback
            controller.SendHapticImpulse(currentIntensity, hapticStepDuration);

            // Wait for the next 'step'
            timeElapsed += hapticStepDuration;
            yield return new WaitForSeconds(hapticStepDuration);

            // Check if the lerp is complete
            if (timeElapsed >= duration)
            {
                // Set intensity to endIntensity and stop lerping
                isLerping = false;
                controller.SendHapticImpulse(endIntensity, hapticStepDuration); // Final impulse
            }
        }

        // After lerping ends, apply endIntensity indefinitely
        while (true)
        {
            controller.SendHapticImpulse(endIntensity, hapticStepDuration);
            yield return new WaitForSeconds(hapticStepDuration);
        }
    }

    public void GunSelected(SelectEnterEventArgs args)
    {
        XRGrabInteractable interactable = args.interactableObject as XRGrabInteractable;
        if (interactable != null)
        {
            GrapplingGun gun = interactable.GetComponent<GrapplingGun>();
            if (gun != null)
            {
                gun.OnGrab();
                BroAudio.Play(sfxManager.pickedUpGrapple);

                if (args.interactorObject.transform.IsChildOf(leftHandControllerTransform))
                {
                    gun.WhichHand = "left";
                    leftGrapplingGun = gun;
                    gun.SetControllerVelocityReference(leftControllerVelocity);
                    // Debug.Log("Left Hand Grappling Gun Assigned: " + gun.name);
                }
                else if (args.interactorObject.transform.IsChildOf(rightHandControllerTransform))
                {
                    gun.WhichHand = "right";
                    rightGrapplingGun = gun;
                    gun.SetControllerVelocityReference(rightControllerVelocity);
                    // Debug.Log("Right Hand Grappling Gun Assigned: " + gun.name);
                }
            }
        }
    }


    public void GunExited(SelectExitEventArgs args)
    {
        XRGrabInteractable interactable = args.interactableObject as XRGrabInteractable;
        if (interactable != null)
        {
            GrapplingGun gun = interactable.GetComponent<GrapplingGun>();
            if (gun != null)
            {
                gun.OnRelease();

                BroAudio.Play(sfxManager.droppedGrapple);

                if (leftGrapplingGun == gun)
                {
                    leftGrapplingGun = null; // Release the reference
                    gun.SetControllerVelocityReference(null);
                    gun.WhichHand = null;
                    // Debug.Log("Left Hand Grappling Gun Released: " + gun.name);
                }
                else if (rightGrapplingGun == gun)
                {
                    rightGrapplingGun = null; // Release the reference
                    gun.SetControllerVelocityReference(null);
                    gun.WhichHand = null;
                    // Debug.Log("Right Hand Grappling Gun Released: " + gun.name);
                }
            }
        }
    }

    public bool CheckIfBothGrapplesLatched()
    {
        // Null check
        if (leftGrapplingGun == null || rightGrapplingGun == null) return false;

        // Check if both grapples are latched
        if (leftGrapplingGun.Grappling && rightGrapplingGun.Grappling) return true;
        else return false;
    }


    public void InitializeReferences()
    {
        rb = GameObject.FindWithTag("Player").GetComponentInChildren<Rigidbody>();
        sfxManager = SFXManager.Instance;

        leftHandControllerTransform = GameObject.FindGameObjectWithTag("LController").transform;
        rightHandControllerTransform = GameObject.FindGameObjectWithTag("RController").transform;

        leftControllerVelocity = leftHandControllerTransform.GetComponent<ControllerVelocity>();
        rightControllerVelocity = rightHandControllerTransform.GetComponent<ControllerVelocity>();
        XRLeftController = leftHandControllerTransform.GetComponent<XRBaseController>();
        XRRightController = rightHandControllerTransform.GetComponent<XRBaseController>();
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
