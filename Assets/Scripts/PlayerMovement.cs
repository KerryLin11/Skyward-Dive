using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // Define states for the player
    private enum PlayerState
    {
        Grappling,
        Rising,
        Falling,
        Running,
        Idle,
        WallRunning
    }

    public enum MovementMode
    {
        Burst,
        Slippery,
        Default,
    }

    [Header("Movement Mode")]
    [SerializeField] private MovementMode currentMovementMode = MovementMode.Default;

    [SerializeField] private GrappleSettings grappleSettings;

    public void SetMovementMode(MovementMode mode)
    {
        currentMovementMode = mode;
        UpdateMovementSettings();
    }

    private void UpdateMovementSettings()
    {
        switch (currentMovementMode)
        {
            case MovementMode.Burst:
                speed = 400f;
                maxVelocity = 500f;
                forceMultiplier = 600f;
                groundedMultiplier = 1f;
                airbourneMultiplier = 0.85f;
                jumpForce = 130f;
                glideSlowFactor = 2.5f;
                counterInputDecelerationSpeed = 8f;
                downwardGravityModifier = 3f;

                // Grapple Settings
                grappleSettings.initialSpring = 350f;
                grappleSettings.spring = 550f;
                grappleSettings.initialDamper = 10f;
                grappleSettings.damper = 70f;
                grappleSettings.rampUpDuration = 0.15f;
                grappleSettings.tugImpulseStrength = 2000f;
                grappleSettings.maxDistanceMultiplier = 0.25f;
                grappleSettings.minDistanceMultiplier = 0.12f;
                break;

            case MovementMode.Slippery:
                speed = 320f;
                maxVelocity = 180f;
                forceMultiplier = 400f;
                groundedMultiplier = 0.9f;
                airbourneMultiplier = 0.7f;
                jumpForce = 120f;
                glideSlowFactor = 3.5f;
                counterInputDecelerationSpeed = 6f;
                downwardGravityModifier = 2.2f;

                // Grapple Settings
                grappleSettings.initialSpring = 180f;
                grappleSettings.spring = 240f;
                grappleSettings.initialDamper = 25f;
                grappleSettings.damper = 45f;
                grappleSettings.rampUpDuration = 0.35f;
                grappleSettings.tugImpulseStrength = 1500f;
                grappleSettings.maxDistanceMultiplier = 0.12f;
                grappleSettings.minDistanceMultiplier = 0.07f;
                break;

            case MovementMode.Default:
                speed = 300f;
                maxVelocity = 160f;
                forceMultiplier = 500f;
                groundedMultiplier = 1f;
                airbourneMultiplier = 0.5f;
                jumpForce = 130f;
                glideSlowFactor = 3.25f;
                counterInputDecelerationSpeed = 7.5f;
                downwardGravityModifier = 2.5f;

                //! Grapple settings for Default
                grappleSettings.initialSpring = 0f;
                grappleSettings.spring = 100f;
                grappleSettings.initialDamper = 25f;
                grappleSettings.damper = 50f;
                grappleSettings.rampUpDuration = 0.25f;
                grappleSettings.tugImpulseStrength = 1500f;
                grappleSettings.maxDistanceMultiplier = 0.2f;
                grappleSettings.minDistanceMultiplier = 0.1f;
                break;
        }
    }


    [Header("References")]
    [SerializeField] private InputActionProperty leftJoystick;
    [SerializeField] private InputActionProperty rightJoystick;
    [SerializeField] private GrapplingGunManager grapplingGunManager;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 300f;
    [SerializeField] private float maxVelocity = 160f;
    public float MaxVelocity => maxVelocity;
    [SerializeField] private float forceMultiplier = 500f;
    [SerializeField] private LayerMask groundLayer;

    [Header("State Speed Multipliers")]
    [SerializeField] private float groundedMultiplier = 1f;
    [SerializeField] private float airbourneMultiplier = 0.5f;
    [SerializeField] private float wallRunMultiplier = 1.5f;

    [Header("Jump & Glide Settings")]
    [SerializeField] private float doubleJumpForce = 100f;
    [SerializeField] private float jumpForce = 130f;
    [SerializeField] private float glideSlowFactor = 3.25f;
    [SerializeField] private float glideFastFactor = 10.0f;
    [SerializeField] private float coyoteTimeDuration = 0.5f;

    [Header("Wallrunning Settings")]
    [SerializeField] private float wallRunRaycastDistance = 1.5f;
    [Range(-1f, 1f)][SerializeField] private float wallRunDetectionThreshold = 0.75f;
    [SerializeField] private float wallRunGlideFactor = 20f;

    [Header("Counter Input Settings")]
    [SerializeField] private float counterInputThreshold = 0.5f;
    [SerializeField] private float counterInputDecelerationSpeed = 7.5f; // Speed at which deceleration happens

    [Header("Gravity Settings")]
    [SerializeField] private float baseGravityMultiplier = 1.0f; // The base gravity strength //! Keep at 1.0
    [SerializeField] private float upwardVelocityThreshold = 10.0f; // Velocity at which upwardGravityModifier gets applied
    [SerializeField] private float upwardGravityModifier = 0.75f; // Multiple of base gravity when upwardVelocityThreshold breached
    [SerializeField] private float downwardVelocityThreshold = -10.0f;
    [SerializeField] private float downwardGravityModifier = 2.5f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private PlayerState currentState;
    private bool isGrounded;
    private bool isWallRunning = false;
    private SFXManager sfxManager;

    void Start()
    {
        grapplingGunManager = GrapplingGunManager.Instance;
        sfxManager = SFXManager.Instance;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation due to physics (should already be set in the rb)
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = false;

        cameraTransform = Camera.main.transform;
        currentState = PlayerState.Idle; // Start with Idle state
        SetMovementMode(MovementMode.Default);
    }

    private void Update()
    {
        HandleGlide();

        DetectWallRunInput();
    }


    private float timeSinceLastGrounded = 0f; // Timer to track coyote time

    void FixedUpdate()
    {
        DetectState();

        if (IsGrounded())
        {
            ResetJumps();
            timeSinceLastGrounded = 0f; // Reset coyote time when grounded
        }
        else
        {
            timeSinceLastGrounded += Time.fixedDeltaTime;
        }

        HandleJump();
        MovePlayer();
    }

    private void DetectWallRunInput()
    {
        Vector2 rightJoystickInput = rightJoystick.action.ReadValue<Vector2>();

        if (rightJoystickInput.x < -wallRunDetectionThreshold || rightJoystickInput.x > wallRunDetectionThreshold)
        {
            StartWallRun(rightJoystickInput);
        }
        else
        {
            EndWallRun();
        }
    }

    private void StartWallRun(Vector2 rightJoystickInput)
    {
        RaycastHit hit;
        Vector3 rayDirection = rightJoystickInput.x < 0 ? -cameraTransform.right : cameraTransform.right;

        if (Physics.Raycast(transform.position, rayDirection, out hit, wallRunRaycastDistance, groundLayer))
        {
            if (!isWallRunning)
            {
                BroAudio.Play(sfxManager.wallRunDetected);

                ResetJumps();

                isWallRunning = true;
                applyingGravity = false;
            }
        }
        else
        {
            EndWallRun();
        }


    }

    private void EndWallRun()
    {
        if (isWallRunning)
        {
            isWallRunning = false;
            applyingGravity = true;
        }
    }


    private void MovePlayer()
    {
        Vector2 input = leftJoystick.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;


        Vector3 forward = cameraTransform.forward;
        forward.y = 0; // Keep movement on the horizontal plane
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        float currentSpeed;

        //! State speed multipliers 
        // Fastest speed if wallrunning
        // Normal speed if grounded
        // Reduced speed when airborne
        if (currentState == PlayerState.WallRunning)
        {
            currentSpeed = speed * wallRunMultiplier;
        }
        else if (IsGrounded())
        {
            currentSpeed = speed * groundedMultiplier;
        }
        else
        {
            currentSpeed = speed * airbourneMultiplier;
        }

        Vector3 move = (forward * direction.z + right * direction.x) * currentSpeed; // Forward camera direction

        //! Counter-input handling based on velocity direction
        if (currentState != PlayerState.Grappling && currentState != PlayerState.Rising && currentState != PlayerState.Falling)
        {

            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Input Direction (horizontal axis)
            Vector3 horizontalInputDirection = new Vector3(move.x, 0, move.z).normalized; // Forward camera direction (horizontal axis)

            // Dot product: forward camera, input direction
            float dotProduct = Vector3.Dot(horizontalInputDirection, horizontalVelocity.normalized);

            if (dotProduct < 0 || Mathf.Abs(dotProduct) < counterInputThreshold)
            {
                // Apply deceleration
                float decelerationFactor = Mathf.Clamp01(counterInputDecelerationSpeed * Time.fixedDeltaTime);
                Vector3 localVelocity = rb.velocity;

                localVelocity.x = Mathf.Lerp(localVelocity.x, 0, decelerationFactor);
                localVelocity.z = Mathf.Lerp(localVelocity.z, 0, decelerationFactor);

                // Update velocity
                rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);
            }
        }


        //! Add more horizontal force if in air and input vector opposes current velocity vector

        bool isInAir = !IsGrounded();
        float horizontalForceMultiplier = forceMultiplier;


        if (isInAir && (Mathf.Abs(input.x) > 0))
        {
            // Check if input vector opposes x,z horizontal velocity (threshold = 45 degrees)
            Vector3 inputDirection = new Vector3(input.x, 0, input.y).normalized;
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
            float angle = Vector3.Angle(inputDirection, horizontalVelocity);

            if (angle > 45f)
            {
                float currentVelocityMagnitude = rb.velocity.magnitude;

                float dynamicHorizontalForceMultiplier = Mathf.Lerp(minAirStrafeMultiplier, maxAirStrafeMultiplier, currentVelocityMagnitude / maxVelocity);

                horizontalForceMultiplier *= dynamicHorizontalForceMultiplier;
            }
        }
        rb.AddForce(move * horizontalForceMultiplier * Time.fixedDeltaTime);

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        // Apply Gravity if not grappling
        if (currentState != PlayerState.Grappling)
        {
            ApplyCustomGravity();
        }

    }
    [SerializeField] private float minAirStrafeMultiplier = 2f;
    [SerializeField] private float maxAirStrafeMultiplier = 5f;

    private bool hasJumped = false;
    private bool hasDoubleJumped = false;
    private Vector2 jumpInput;

    // private bool wasAboveThreshold = false;
    private bool wasBelowThreshold = false;

    private const float jumpThreshold = 0.5f;
    private const float nearZeroThreshold = 0.1f;

    private void HandleJump()
    {
        jumpInput = rightJoystick.action.ReadValue<Vector2>();
        float verticalInput = jumpInput.y;

        if (verticalInput > jumpThreshold)
        {
            // First Jump
            if (IsGrounded() || timeSinceLastGrounded < coyoteTimeDuration)
            {
                if (!hasJumped)
                {
                    Jump(jumpForce);
                    timeSinceLastGrounded = coyoteTimeDuration + 1f; // Disable coyote time after jumped
                }
            }
            // Second Jump
            else if (wasBelowThreshold && !hasDoubleJumped)
            {
                // If currently moving down, reset y velocity and perform jump else, just jump
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                }

                Jump(doubleJumpForce);
                hasDoubleJumped = true;
            }
            // wasAboveThreshold = true;
            wasBelowThreshold = false; // Reset
        }
        else if (verticalInput < nearZeroThreshold)
        {
            wasBelowThreshold = true;
            // wasAboveThreshold = false; // Reset
        }
    }

    private void HandleGlide()
    {
        if (!IsGrounded())
        {
            float glideSlowFactorAdjusted = glideSlowFactor;
            float glideFastFactorAdjusted = glideFastFactor;

            if (currentState == PlayerState.WallRunning)
            {
                glideSlowFactorAdjusted = wallRunGlideFactor;
                glideFastFactorAdjusted = wallRunGlideFactor;
            }

            jumpInput = rightJoystick.action.ReadValue<Vector2>();
            float verticalInput = jumpInput.y;

            if (verticalInput > 0.05f)
            {
                // Glide upwards (slower fall)
                rb.velocity += Vector3.up * glideSlowFactorAdjusted * Time.deltaTime;
            }
            else if (verticalInput < -0.05f)
            {
                // Faster fall
                rb.velocity += Vector3.down * glideFastFactorAdjusted * Time.deltaTime;
            }
        }
    }

    private void Jump(float force)
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);

        if (force == jumpForce)
        {
            hasJumped = true;
            BroAudio.Play(sfxManager.jump);
        }
        else
        {
            hasDoubleJumped = true;
            BroAudio.Play(sfxManager.doubleJump);
        }
    }

    public void ResetJumps()
    {
        hasJumped = false;
        hasDoubleJumped = false;
        // wasAboveThreshold = false;
        wasBelowThreshold = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer & (1 << collision.gameObject.layer)) != 0)
        {

        }
    }



    private void DetectState()
    {
        isGrounded = IsGroundedHelper();

        // Determine current state 
        if (grapplingGunManager.GetLeftGrapplingGun()?.IsGrappling() == true || grapplingGunManager.GetRightGrapplingGun()?.IsGrappling() == true)
        {
            SetPlayerState(PlayerState.Grappling);
        }
        else if (!isGrounded)
        {
            if (isWallRunning)
            {
                SetPlayerState(PlayerState.WallRunning);
            }
            else if (rb.velocity.y > 0)
            {
                SetPlayerState(PlayerState.Rising); // Ascending
            }
            else if (rb.velocity.y < 0)
            {
                SetPlayerState(PlayerState.Falling); // Falling down
            }
        }
        else if (rb.velocity.magnitude > 0.1f)
        {
            SetPlayerState(PlayerState.Running);
        }
        else
        {
            SetPlayerState(PlayerState.Idle);
        }
    }

    private void SetPlayerState(PlayerState newState)
    {
        if (currentState != newState) // Only log state if it changes
        {
            currentState = newState;


            //? Only for debugging
            if (currentState == PlayerState.Grappling)
            {
                if (grapplingGunManager.GetLeftGrapplingGun()?.IsGrappling() == true)
                {
                    Debug.Log("Left Grappling");
                }
                else if (grapplingGunManager.GetRightGrapplingGun()?.IsGrappling() == true)
                {
                    Debug.Log("Right Grappling");
                }
            }
            else
            {
                Debug.Log(currentState);
            }
        }
    }


    private bool applyingGravity = true;

    private void ApplyCustomGravity()
    {
        if (applyingGravity)
        {
            float verticalVelocity = rb.velocity.y;
            float gravityFactor = baseGravityMultiplier;

            if (verticalVelocity > upwardVelocityThreshold)
            {
                // Player moving upwards = reduce gravity
                gravityFactor *= upwardGravityModifier;
            }
            else if (verticalVelocity < downwardVelocityThreshold)
            {
                // Player falling fast = increase gravity
                gravityFactor *= downwardGravityModifier;
            }


            // Debug.Log($"Gravity Factor: {gravityFactor}, Vertical Velocity: {verticalVelocity}");

            rb.AddForce(Vector3.down * gravityFactor * Physics.gravity.magnitude, ForceMode.Acceleration);
        }
        else
        {
            return;
        }


    }


    //! 0.55 is 0.05 above the ground. 
    private bool IsGroundedHelper()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.70f, groundLayer);
    }


    // Function that checks if the player is grounded using the enum rather than physics raycast. (Since there's already a function that sets the state according to the raycast)
    public bool IsGrounded()
    {
        return currentState == PlayerState.Idle || currentState == PlayerState.Running;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.55f);
    }
}
