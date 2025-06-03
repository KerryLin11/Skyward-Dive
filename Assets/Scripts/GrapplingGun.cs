using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using NaughtyAttributes;
using Ami.BroAudio;
using UnityEngine.XR.Interaction.Toolkit;

public class GrapplingGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GrappleSettings grappleSettings;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ControllerVelocity controllerVelocity;
    private PlayerMovement playerMovement;

    private SpringJoint currentSpringJoint;
    private Hook bullet;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform gunTip;

    [Header("Managers")]
    private GrapplingGunManager grapplingGunManager;
    private SFXManager sfxManager;


    [Header("Layer Masks")]
    [SerializeField] private LayerMask whatIsGrappleable;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    public LineRenderer lrGrapple; // LineRenderer for grapple
    public LineRenderer lrPreview; // LineRenderer for preview line
    [SerializeField] private GameObject grapplePointIndicator; // Indicator for grapple point

    [SerializeField] private GameObject hookPrefab;
    private GameObject currentBullet;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float overshootYAxis;

    [SerializeField, NaughtyAttributes.ReadOnly] private string whichHand;
    public string WhichHand
    {
        get => whichHand;
        set => whichHand = value;
    }

    private bool isHeld = false;

    const float spherecastRadius = 0.5f;

    private Vector3 grapplePoint;

    [Header("Input")]
    private bool grappling;
    public bool Grappling
    {
        get => grappling;
        set => grappling = value;
    }

    public void SetPreviewLinesEnabled(bool isEnabled)
    {
        lrPreview.enabled = isEnabled;
    }


    private void Start()
    {
        InitializeReferences();


        // Configure the grapple line renderer
        lrGrapple.enabled = false;
        lrGrapple.startWidth = 0.02f;
        lrGrapple.endWidth = 0.02f;
        lrGrapple.startColor = Color.green;
        lrGrapple.endColor = Color.green;

        // Configure the preview line renderer
        lrPreview.startWidth = 0.02f;
        lrPreview.endWidth = 0.02f;
        lrPreview.startColor = Color.red;
        lrPreview.endColor = Color.red;

        // Configure the grapple point indicator
        if (grapplePointIndicator != null)
        {
            grapplePointIndicator.SetActive(false); // Start with indicator hidden
        }
    }

    private XRGrabInteractable grabInteractable;
    public void InitializeReferences()
    {

        // Initialize everything else
        grapplingGunManager = GrapplingGunManager.Instance;
        sfxManager = SFXManager.Instance;


        // lrGrapple = GetComponent<LineRenderer>();
        // lrPreview = gameObject.AddComponent<LineRenderer>();
        playerGameObject = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject;
        rb = playerGameObject.GetComponent<Rigidbody>();
        cam = Camera.main?.transform;
        playerMovement = playerGameObject.GetComponentInChildren<PlayerMovement>();
    }
    public void SelectEntered(SelectEnterEventArgs args)
    {
        // Ensure the Singleton is available and call the method on it
        if (GrapplingGunManager.Instance != null)
        {
            GrapplingGunManager.Instance.GunSelected(args);
        }
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        // Ensure the Singleton is available and call the method on it
        if (GrapplingGunManager.Instance != null)
        {
            GrapplingGunManager.Instance.GunExited(args);
        }
    }

    private bool performedTugOnce = false;
    private void Update()
    {
        if (grappling && controllerVelocity != null)
        {
            Vector3 velocity = controllerVelocity.Velocity;
            if (velocity.magnitude > grappleSettings.tugVelocityThreshold && performedTugOnce == false)
            {
                PerformTug();
            }
        }
    }


    private void PerformTug()
    {
        // Checks with hand transform to use
        bool isLeftHand = whichHand.Equals("left");
        GrapplingGunManager.Instance.OnTug(isLeftHand);

        // Check if controller velocity exceeds threshold
        if (controllerVelocity.Velocity.magnitude > grappleSettings.tugVelocityThreshold)
        {
            if (currentSpringJoint != null)
            {
                BroAudio.Play(sfxManager.tugSuccessful);

                // Blend 'current velocity' direction with 'player to hit point' direction
                Vector3 playerToGrappleDirection = (grapplePoint - gunTip.position).normalized;
                Vector3 currentVelocityDirection = rb.velocity.normalized;


                // https://www.youtube.com/watch?v=2PrSUK1VrKA
                float dot = Vector3.Dot(currentVelocityDirection, playerToGrappleDirection);
                Vector3 blendedVelocityDirection = (playerToGrappleDirection + currentVelocityDirection * dot).normalized;

                // Apply impulse
                Vector3 impulse = blendedVelocityDirection * grappleSettings.tugImpulseStrength;
                rb.AddForce(impulse, ForceMode.Impulse);

                // Apply haptic
                grapplingGunManager.PerformHapticFeedback(isLeftHand, grappleSettings.tugHapticIntensity, grappleSettings.tugHapticDuration);

                Debug.Log("Tug impulse applied!");

                performedTugOnce = true;
            }
        }
    }

    // Needs to be accessible to GrapplingGunManager
    public void PerformDualTug()
    {
        BroAudio.Play(sfxManager.dualTugSuccessful);

        Vector3 direction = (grapplePoint - gunTip.position).normalized;
        Vector3 impulse = direction * grappleSettings.dualTugExtraImpulse;
        rb.AddForce(impulse, ForceMode.Impulse);

        // Apply haptic
        grapplingGunManager.StopLerpingHaptic();
        grapplingGunManager.PerformHapticFeedback(whichHand.Equals("left"), grappleSettings.dualTugHapticIntensity, grappleSettings.dualTugHapticDuration);

        Debug.Log("Dual tug performed: Extra impulse applied!");
    }


    private bool wasHeld = false; // Track previous isHeld state
    private void LateUpdate()
    {
        if (isHeld)
        {
            // If held, run grapple/preview logic
            if (grappling)
            {
                UpdateLineRenderer();
                lrPreview.enabled = false; // Disable preview line when grappling
                if (grapplePointIndicator != null)
                {
                    grapplePointIndicator.SetActive(false); // Hide indicator while grappling
                }
            }
            else
            {
                UpdatePreviewLine();

                if (SettingsManager.Instance.previewLines)
                {
                    lrPreview.enabled = true;
                }
                else
                {
                    lrPreview.enabled = false;
                }

                if (grapplePointIndicator != null)
                {
                    grapplePointIndicator.SetActive(true);
                    UpdateIndicator(); // Update indicator visibility
                }
            }
        }
        else
        {
            if (wasHeld)
            {
                OnRelease();
            }
        }

        wasHeld = isHeld; // Update wasHeld flag
    }



    private void FixedUpdate()
    {
        if (grappling)
        {
            UpdateLineRenderer();
        }
    }

    private bool isLatched = false;
    public void StartGrapple()
    {
        grappling = true;


        RaycastHit[] spherecastHits;
        Vector3 closestHitPoint = Vector3.zero;
        float minDistance = Mathf.Infinity;

        // Perform a SphereCast to find potential grapple points
        spherecastHits = Physics.SphereCastAll(gunTip.position, spherecastRadius, gunTip.forward, maxGrappleDistance, whatIsGrappleable);

        bool hitGround = false;

        foreach (RaycastHit hit in spherecastHits)
        {
            // Check if the hit object is on the ground layer
            if (IsGroundLayer(hit.collider))
            {
                hitGround = true;
                break; // No need to check further if we already hit a ground layer
            }
        }

        if (hitGround)
        {
            // Perform a regular Raycast to get the precise grapple point
            RaycastHit hit;
            if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, maxGrappleDistance, whatIsGrappleable))
            {
                isLatched = true;

                grapplePoint = hit.point;

                currentBullet = Instantiate(hookPrefab, grapplePoint, gunTip.rotation * Quaternion.Euler(90, 0, 0));

                ExecuteGrapple();

                FixedJoint fixedJoint = currentBullet.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = hit.rigidbody;
                fixedJoint.autoConfigureConnectedAnchor = false;
                fixedJoint.connectedAnchor = grapplePoint;

                // Set the break force and torque to prevent disconnection
                fixedJoint.breakForce = Mathf.Infinity;
                fixedJoint.breakTorque = Mathf.Infinity;
            }
            else
            {
                isLatched = false;
                BroAudio.Play(sfxManager.grappleMissed);

                grapplePoint = gunTip.position + gunTip.forward * maxGrappleDistance;
                StopGrapple();
            }
        }
        else
        {
            // Find the closest hit point (not the furthest)
            foreach (RaycastHit hit in spherecastHits)
            {
                Vector3 directionToHit = hit.point - gunTip.position;
                float distanceToHit = directionToHit.magnitude;

                if (distanceToHit < minDistance)
                {
                    minDistance = distanceToHit;
                    closestHitPoint = hit.point - (directionToHit.normalized * spherecastRadius); // Adjust hit point by spherecast radius
                }
            }

            if (minDistance < Mathf.Infinity)
            {
                isLatched = true;

                grapplePoint = closestHitPoint;
                currentBullet = Instantiate(hookPrefab, grapplePoint, gunTip.rotation * Quaternion.Euler(90, 0, 0));
                ExecuteGrapple();


                // Attach FixedJoint to hitpoint
                FixedJoint fixedJoint = currentBullet.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = spherecastHits[0].rigidbody; // Use the first hit's rigidbody
                fixedJoint.autoConfigureConnectedAnchor = false;
                fixedJoint.connectedAnchor = grapplePoint;

                // Set the break force and torque to prevent disconnection
                fixedJoint.breakForce = Mathf.Infinity;
                fixedJoint.breakTorque = Mathf.Infinity;
            }
            else
            {
                isLatched = false;
                BroAudio.Play(sfxManager.grappleMissed);

                grapplePoint = gunTip.position + gunTip.forward * maxGrappleDistance;
                StopGrapple();
            }
        }

        UpdateLineRenderer();
        lrGrapple.enabled = true;
    }

    public void SetControllerVelocityReference(ControllerVelocity velocityReference)
    {
        controllerVelocity = velocityReference;
    }


    public void OnGrab()
    {
        isHeld = true;
    }

    public void OnRelease()
    {
        lrGrapple.enabled = false;
        lrPreview.enabled = false;
        grapplePointIndicator.SetActive(false);

        isHeld = false;
    }



    private bool IsGroundLayer(Collider collider)
    {
        // Check if the collider's layer is part of the groundLayerMask
        return (groundLayerMask == (groundLayerMask | (1 << collider.gameObject.layer)));
    }

    private bool IsWallLayer(Collider collider)
    {
        // Check if the collider's layer is part of the wallLayerMask
        return (wallLayerMask == (wallLayerMask | (1 << collider.gameObject.layer)));
    }



    public void ExecuteGrapple()
    {
        playerMovement.ResetJumps();

        if (grapplingGunManager.CheckIfBothGrapplesLatched())
        {
            BroAudio.Play(sfxManager.dualGrapplesLatched);
        }
        else
        {
            BroAudio.Play(sfxManager.grappleSuccessful);
        }

        grapplingGunManager.PerformHapticFeedback(whichHand.Equals("left"), grappleSettings.startGrappleHapticDuration, grappleSettings.startGrappleHapticIntensity);

        // Debug.Log(currentBullet);


        if (currentBullet == null)
        {
            Debug.LogError("Current bullet is null. Cannot attach SpringJoint.");
            return;
        }

        Rigidbody bulletRigidbody = currentBullet.GetComponent<Rigidbody>();

        if (bulletRigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on current bullet.");
            return;
        }

        currentSpringJoint = playerGameObject.AddComponent<SpringJoint>();
        currentSpringJoint.connectedBody = bulletRigidbody;
        currentSpringJoint.autoConfigureConnectedAnchor = false;
        currentSpringJoint.connectedAnchor = Vector3.zero;
        currentSpringJoint.anchor = Vector3.zero;

        float distanceFromPoint = Vector3.Distance(playerGameObject.transform.position, grapplePoint);

        currentSpringJoint.maxDistance = distanceFromPoint * grappleSettings.maxDistanceMultiplier;
        currentSpringJoint.minDistance = distanceFromPoint * grappleSettings.minDistanceMultiplier;

        currentSpringJoint.damper = grappleSettings.initialDamper;
        currentSpringJoint.spring = grappleSettings.initialSpring;

        springForceCoroutine = StartCoroutine(GraduallyIncreaseSpringForce());
    }

    private Coroutine springForceCoroutine;

    private IEnumerator GraduallyIncreaseSpringForce()
    {
        float timeElapsed = 0f;
        float duration = grappleSettings.rampUpDuration;

        float targetSpring = grappleSettings.spring;
        float targetDamper = grappleSettings.damper;

        while (timeElapsed < duration)
        {
            if (currentSpringJoint == null)
            {
                // Exit the coroutine if the SpringJoint is destroyed or removed
                yield break;
            }

            // Gradually increase the spring force and adjust the damper value
            currentSpringJoint.spring = Mathf.Lerp(grappleSettings.initialSpring, targetSpring, timeElapsed / duration);
            currentSpringJoint.damper = Mathf.Lerp(grappleSettings.initialDamper, targetDamper, timeElapsed / duration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // Set final values
        if (currentSpringJoint != null)
        {
            currentSpringJoint.spring = targetSpring;
            currentSpringJoint.damper = targetDamper;
        }
    }

    public void StopGrapple()
    {
        // Stop lerping haptic
        grapplingGunManager.StopLerpingHaptic();

        // SFX
        if (isLatched)
        {
            BroAudio.Play(sfxManager.ungrapple);
        }

        // Don't allow multiple tugs on 1 grapple.
        performedTugOnce = false;


        // Stop the coroutine to avoid accessing destroyed SpringJoint
        if (springForceCoroutine != null)
        {
            StopCoroutine(springForceCoroutine);
            springForceCoroutine = null;  // Clear the reference
        }

        Destroy(currentBullet);
        Destroy(currentSpringJoint);

        grappling = false;
        lrGrapple.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    private void UpdateLineRenderer()
    {
        lrGrapple.SetPosition(0, gunTip.position);
        lrGrapple.SetPosition(1, grapplePoint);
    }

    private void UpdatePreviewLine()
    {
        lrPreview.SetPosition(0, gunTip.position);
        lrPreview.SetPosition(1, gunTip.position + gunTip.forward * maxGrappleDistance);
    }

    private void UpdateIndicator()
    {
        if (grapplePointIndicator == null) return;

        RaycastHit hit;
        bool isGrapplable = false;

        // Step 1: Check for walls first using SphereCast
        if (Physics.SphereCast(gunTip.position, spherecastRadius, gunTip.forward, out hit, maxGrappleDistance, wallLayerMask))
        {
            // Prioritize walls
            isGrapplable = true;
            Vector3 directionToHit = hit.point - gunTip.position;
            directionToHit.Normalize();
            grapplePointIndicator.transform.position = hit.point - (directionToHit * spherecastRadius); // Adjust for spherecast radius
        }
        // Step 2: If no wall, check for ground layer using Raycast
        else if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, maxGrappleDistance, groundLayerMask))
        {
            // If no wall, check ground layers
            isGrapplable = true;
            grapplePointIndicator.transform.position = hit.point; // No need to adjust for radius
        }
        else
        {
            // No valid grapple point found, set indicator at max grapple distance
            grapplePointIndicator.transform.position = gunTip.position + gunTip.forward * maxGrappleDistance;
        }

        // Step 3: Update indicator visibility based on whether a grapple point was found
        grapplePointIndicator.SetActive(isGrapplable);
    }




    private void OnDrawGizmos()
    {
        if (currentSpringJoint == null) return;

        if (currentSpringJoint.connectedBody != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(currentSpringJoint.transform.position, currentSpringJoint.connectedBody.transform.position);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(currentSpringJoint.transform.position, currentSpringJoint.connectedAnchor);
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
