using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Transform cameraContainer;
    private Animator animator;

    private Rigidbody rb;
    private PlayerInputActions inputActions;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool isJumpPressed = false;

    private float camCurXRot = 0f;
    public float lookSensitivity = 1f;
    public float minXLook = -60f;
    public float maxXLook = 60f;

    private Interaction interaction; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJumpPerformed;

        inputActions.Player.Look.performed += OnLookPerformed;
        inputActions.Player.Look.canceled += OnLookCanceled;

        inputActions.Player.Interaction.performed += OnInteractPerformed;

        interaction = GetComponent<Interaction>();
        animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Jump.performed -= OnJumpPerformed;

        inputActions.Player.Look.performed -= OnLookPerformed;
        inputActions.Player.Look.canceled -= OnLookCanceled;

        inputActions.Player.Interaction.performed -= OnInteractPerformed;


    }

    private void FixedUpdate()
    {
        Move();
        UpdateAnimation();
    }

    private void LateUpdate()
    {
        CameraLook();

        if(IsGrounded())
        {
            animator.SetBool("isJumping", false);
        }
    }
    private void UpdateAnimation()
    {
        bool isMoving = movementInput.magnitude > 0.1f;

        animator.SetBool("isMoving", isMoving);
    }
    private void Move()
    {
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement = transform.TransformDirection(movement) * moveSpeed;
        Vector3 velocity = rb.velocity;
        velocity.x = movement.x;
        velocity.z = movement.z;
        rb.velocity = velocity;

        if (isJumpPressed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumpPressed = false;
            animator.SetBool("isJumping", true);
        }
    }

    private void CameraLook()
    {
        camCurXRot += lookInput.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, lookInput.x * lookSensitivity, 0);

        //transform.Rotate(0, lookInput.x * lookSensitivity, 0);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    // 입력 이벤트 핸들러
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        isJumpPressed = true;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }
    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (interaction != null)
        {
            interaction.Interact();
        }
    }


}
