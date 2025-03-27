using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Camera playerCamera;
    public bool isAllowed = true;
    private float xRotation = 0f;

    [Header("Walking and Running")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private Vector3 velocity;
    private bool isSprinting = false;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    private bool isGrounded;
    private bool isJumping = false;

    [Header("Crouching and Crouch Walk")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    private float originalHeight;
    private float originalSpeed;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Miscellaneous")]
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;

        FetchValues();

        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        originalHeight = controller.height;
        originalSpeed = walkSpeed;

    }

    private void Update()
    {
        if (isAllowed)
        {
            if (MovementValues.Instance.canLookWithMouse)
                HandleMouseLook();

            if (MovementValues.Instance.canJump)
                Jump();

            if (MovementValues.Instance.canWalk)
                Move();

            if (MovementValues.Instance.canCrouch)
                Crouch();

            CheckIsGrounded();
            ApplyGravity();
        }
    }

    //private void LateUpdate()
    //{
    //    if (MovementValues.Instance.canLookWithMouse)
    //        HandleMouseLook();
    //}

    private void CheckIsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        isJumping = !isGrounded && velocity.y > 0.01f;
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isGrounded)
        {
            if (Input.GetKey(KeybindManager.Instance.GetKey("Sprint")) && MovementValues.Instance.canSprint)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }

        if (isSprinting)
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(move * walkSpeed * Time.deltaTime);
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeybindManager.Instance.GetKey("Jump")) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeybindManager.Instance.GetKey("Crouch")))
        {
            controller.height = crouchHeight;
            walkSpeed *= crouchSpeedMultiplier;
            MovementValues.Instance.ToggleCanSprint(false);
            MovementValues.Instance.ToggleCanJump(false);
        }
        else if (Input.GetKeyUp(KeybindManager.Instance.GetKey("Crouch")))
        {
            controller.height = originalHeight;
            walkSpeed = originalSpeed;
            MovementValues.Instance.ToggleCanSprint(true);
            MovementValues.Instance.ToggleCanJump(true);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void FetchValues()
    {
        walkSpeed = MovementValues.Instance.walkSpeed;
        sprintSpeed = MovementValues.Instance.sprintSpeed;

        jumpHeight = MovementValues.Instance.jumpHeight;
        gravity = MovementValues.Instance.gravity;

        crouchHeight = MovementValues.Instance.crouchHeight;
        crouchSpeedMultiplier = MovementValues.Instance.crouchSpeedMultiplier;
    }

    public void ReturnToInitialPosition()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        transform.position = initialPosition;

        if (controller != null) controller.enabled = true;
    }
}
