using System;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    [Header("Movement")] [SerializeField] private float maxSpeed = 13f;

    [SerializeField] private float acceleration = 96f;

    [SerializeField] private float deceleration = 128f;

    [Space] [Header("Jump")] [SerializeField]
    private float jumpHeight = 2.5f;

    [Range(0, 1f)] [SerializeField] private float airControl = 0.5f; // 0 - 1
    [Range(0, 1f)] [SerializeField] private float airBreak = 0f; // 0 - 1

    [SerializeField] private float gravityScale = 6f;

    [Space] [Header("Camera")] [SerializeField]
    private float sensitivity = 1f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        OnHealthChanged?.Invoke(Health);
    }

    private void OnCollisionStay(Collision collision)
    {
        var contacts = collision.contacts;

        if (contacts.Any(contact => Vector3.Dot(contact.normal, Vector3.up) > 0.5f))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit()
    {
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        Move(moveInput);

        ApplyAdditionalGravity();
    }

    private void Update()
    {
        Rotate(lookInput);
    }

    private void ApplyAdditionalGravity()
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

    public void OnLook(InputAction.CallbackContext context)
    {
        var value = context.phase switch
        {
            InputActionPhase.Started or InputActionPhase.Performed => context.ReadValue<Vector2>(),
            _ => Vector2.zero,
        };

        lookInput = value;
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

    private void Rotate(Vector2 lookInput)
    {
        xRotation -= lookInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        var localRotation = virtualCamera.transform.localRotation;

        localRotation = Quaternion.Euler(xRotation, localRotation.eulerAngles.y, localRotation.eulerAngles.z);
        virtualCamera.transform.localRotation = localRotation;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, lookInput.x * sensitivity, 0f));
    }

    private void Jump()
    {
        if (!isGrounded) return;

        var jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
        rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
    }

    public int Health { get; set; } = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        OnHealthChanged?.Invoke(Health);
        if (Health <= 0)
        {
            Die();
        }
    }

    public Action<int> OnHealthChanged { get; set; }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}