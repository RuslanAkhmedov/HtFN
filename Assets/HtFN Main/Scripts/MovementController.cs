using Unity.Cinemachine;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public PlayerController playerController;
    public CinemachineCamera virtualCamera;
    private Rigidbody rb;

    [Header("Keyboard Settings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Movement Settings")]
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;
    public float acceleration;

    [Header("Sprint Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float walkAcceleration = 30f;
    public float sprintAcceleration = 60f;

    [Header("Jump Settings")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private bool grounded;

    public bool useStamine { get; private set; }



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MoveInput();

        HandleSprint();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        rb.linearDamping = grounded ? groundDrag : airDrag;

        MovePlayer();

        SpeedControl();
    }

    private void MoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovePlayer()
    {
        if (virtualCamera == null) return;

        Transform camTransform = virtualCamera.transform;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * verticalInput + right * horizontalInput).normalized;
        if (moveDirection.magnitude > 0)
        {
            if (grounded)
                rb.AddForce(moveDirection * acceleration, ForceMode.Acceleration);
            else if (!grounded)
                rb.AddForce(moveDirection * acceleration * airMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f); // Останавливаем движение, если нет ввода
        }

}

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (playerController.stamine < 10) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Добавляем прыжковую силу
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);


        playerController.stamine -= 10;
        playerController.canUseStamine = playerController.stamine > 0;
        useStamine = true;
    }

    private void HandleSprint()
    {
        bool isMoving = horizontalInput != 0 || verticalInput != 0;
        bool isSprinting = Input.GetKey(sprintKey) && grounded && isMoving && playerController.canUseStamine;

        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
        float targetAcceleration = isSprinting ? sprintAcceleration : walkAcceleration;

        if (isSprinting)
        {
            playerController.stamine = Mathf.Max(playerController.stamine - Time.deltaTime * 5, 0);

            if (playerController.stamine <= 0)
            {
                playerController.canUseStamine = false;
                useStamine = false;
            }
            else
            {
                useStamine = true;
            }
        }
        else if (!grounded) // Если игрок в воздухе после прыжка, продолжаем считать стамину потраченной
        {
            useStamine = true;
        }
        else
        {
            useStamine = false;
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, Time.fixedDeltaTime * 10f);
        acceleration = Mathf.MoveTowards(acceleration, targetAcceleration, Time.fixedDeltaTime * 10f);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }



    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        rb.linearDamping = grounded ? groundDrag : airDrag;
    }


}