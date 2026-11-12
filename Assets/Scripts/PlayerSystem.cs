
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof (PlayerCamera))]
public class PlayerSystem : MonoBehaviour
{
    public static PlayerSystem Instance { get; private set; }

    public float speed = 3f;
    public float jumpHeight = 1f;
    public float rotationSpeed = 10f;

    public InputActionAsset inputActions;

    // Кэшированные компоненты
    private Animator animator;
    private PlayerCamera playerCamera;
    private CharacterController controller;
    private Transform mainCameraTransform;

    // Действия ввода
    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    public InputAction interactAction { get; private set; }

    // Физика и состояние
    private bool isGrounded;
    private float verticalVelocity;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controller = GetComponent<CharacterController>();
        playerCamera = GetComponent<PlayerCamera>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        // Берем действия напрямую из назначенного ассета
        playerActionMap = inputActions.FindActionMap("Player", true);
        moveAction = playerActionMap.FindAction("Move", true);
        jumpAction = playerActionMap.FindAction("Jump", true);
        lookAction = playerActionMap.FindAction("Look", true);
        interactAction = playerActionMap.FindAction("Interact", true);
    }

    void OnEnable()
    {
        playerActionMap.Enable();
        jumpAction.performed += Jump;
    }

    void OnDisable()
    {
        jumpAction.performed -= Jump;
        playerActionMap.Disable();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        Vector2 moveAmt = moveAction.ReadValue<Vector2>();
        Vector3 move = Vector3.zero;

        // Считаем направление и поворот только если игрок нажимает кнопки движения
        if (moveAmt.sqrMagnitude > 0.001f && mainCameraTransform != null)
        {
            Vector3 forward = mainCameraTransform.forward;
            Vector3 right = mainCameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            move = right * moveAmt.x + forward * moveAmt.y;

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * dt);
        }

        verticalVelocity += Physics.gravity.y * dt;

        // Обновление аниматора
        animator.SetFloat("speed_horizontal", moveAmt.magnitude);
        animator.SetFloat("speed_vertical", Mathf.Abs(verticalVelocity));
        animator.SetBool("isGrounded", isGrounded);

        // Итоговое перемещение
        Vector3 finalMove = move * speed;
        finalMove.y = verticalVelocity;
        controller.Move(finalMove * dt);

        playerCamera.lookAmt = lookAction.ReadValue<Vector2>();
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            animator.SetTrigger("Jump");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }
}