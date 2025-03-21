using Unity.Cinemachine;
using UnityEngine;

public class MovementController : MonoBehaviour
{
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

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
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
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void HandleSprint()
    {
        // Проверка, если мы на земле и удерживаем кнопку спринта

        // Проверка на движение (если нет ввода, то персонаж стоит на месте)
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        bool isSprinting = Input.GetKey(sprintKey) && grounded && isMoving;

        // Устанавливаем целевую скорость и ускорение в зависимости от состояния
        float targetSpeed = 0f; // по умолчанию для состояния стояния
        float targetAcceleration = 0f; // по умолчанию для состояния стояния

        if (isSprinting)
        {
            targetSpeed = sprintSpeed;
            targetAcceleration = sprintAcceleration;
        }
        else if (isMoving)
        {
            targetSpeed = walkSpeed;
            targetAcceleration = walkAcceleration;
        }
        else
        {
            // Если персонаж не двигается, ставим минимальное значение для скорости и ускорения
            targetSpeed = 0; // можно установить минимальную скорость, если персонаж не двигается
            targetAcceleration = 30; // или оставить стандартное ускорение для стояния
        }

        // Плавное изменение значения moveSpeed и acceleration
        moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, Time.deltaTime * 10f);
        acceleration = Mathf.MoveTowards(acceleration, targetAcceleration, Time.deltaTime * 10f);
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