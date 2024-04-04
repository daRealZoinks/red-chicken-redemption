using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 13f;

    [SerializeField] private float acceleration = 64f;

    [SerializeField] private float deceleration = 128f;

    [Space]
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 3f;
    [Range(0, 1f)]
    [SerializeField] private float airControl = 0.1f; // 0 - 1
    [Range(0, 1f)]
    [SerializeField] private float airBreak = 0f; // 0 - 1

    [SerializeField] private float gravityScale = 1.5f;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        var contacts = collision.contacts;

        var isStandingOnSurface = contacts.Any(contact => Vector3.Dot(contact.normal, Vector3.up) > 0.5f);

        isGrounded = isStandingOnSurface;
    }

    private void OnCollisionExit()
    {
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        Move(moveInput);

        AppliAdditionalGravity();
    }

    private void AppliAdditionalGravity()
    {
        if (isGrounded) return;

        rb.AddForce(Physics.gravity * (gravityScale - 1), ForceMode.Acceleration);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.phase switch
        {
            InputActionPhase.Started or InputActionPhase.Performed => context.ReadValue<Vector2>(),
            _ => Vector2.zero,
        };

        moveInput = value;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    private void Move(Vector2 moveInput)
    {
        var horizontalVelocity = new Vector3
        {
            x = rb.velocity.x,
            z = rb.velocity.z
        };

        var horizontalClampedVelocity =
            horizontalVelocity.normalized * Mathf.Clamp01(horizontalVelocity.magnitude / maxSpeed);

        var moveDirection = (moveInput.x * transform.right + moveInput.y * transform.forward).normalized;

        Vector3 finalForce;

        if (moveDirection != Vector3.zero)
        {
            var accelerationForce = moveDirection - horizontalClampedVelocity;
            accelerationForce *= acceleration * (isGrounded ? 1 : airControl);
            finalForce = accelerationForce;
        }
        else
        {
            var decelerationForce = -horizontalClampedVelocity;
            decelerationForce *= deceleration * (isGrounded ? 1 : airBreak);
            finalForce = decelerationForce;
        }

        rb.AddForce(finalForce, ForceMode.Acceleration);
    }

    private void Jump()
    {
        if (!isGrounded) return;

        var jumpVelocity = Mathf.Sqrt(jumpHeight * 2f * Mathf.Abs(Physics.gravity.y));
        rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
    }
}
