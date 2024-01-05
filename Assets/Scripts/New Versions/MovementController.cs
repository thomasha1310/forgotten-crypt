using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class MovementController : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private bool shouldUseEnhancedJumping = true;      // Allows the use of coyote time and jump buffer
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.05f;
    [SerializeField] private float movementSmoothing = 0.05f;

    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform groundSensor;

    [SerializeField] private float runSpeed = 40f;

    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private const float groundedRadius = 0.1f;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float horizontalMove = 0f;
    private bool shouldJump = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (shouldUseEnhancedJumping)
        {
            coyoteTimeCounter -= Time.deltaTime;
            jumpBufferCounter -= Time.deltaTime;
        }

        horizontalMove = inputManager.GetHorizontalInput() * runSpeed;
        shouldJump = inputManager.GetJumpInput();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        if (Physics2D.OverlapCircle(groundSensor.position, groundedRadius, groundLayers))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            if (wasGrounded && shouldUseEnhancedJumping)
            {
                coyoteTimeCounter = coyoteTime;
            }
        }

        Move(horizontalMove * Time.fixedDeltaTime, shouldJump);
        shouldJump = false;
    }

    private void Move(float moveAmount, bool shouldJump)
    {

        Vector3 targetVelocity = new Vector2(moveAmount * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (moveAmount > 0 && !isFacingRight || moveAmount < 0 && isFacingRight)
        {
            Flip();
        }

        if (shouldUseEnhancedJumping)
        {
            if (shouldJump)
            {
                jumpBufferCounter = jumpBufferTime;
            }

            if (jumpBufferCounter > 0 && (isGrounded || coyoteTimeCounter > 0))
            {
                isGrounded = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            }
        }
        else if (shouldJump && isGrounded)
        {
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }

    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

}
