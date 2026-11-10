
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof (PlayerCamera))]
public class PlayerSystem : MonoBehaviour
{
    public GameObject self;
    public static PlayerSystem Instance { get; private set; } 
    
    public float speed = 3;
    public float jumpHeight = 1;

    private Animator animator;

    private PlayerCamera playerCamera;
    private CharacterController controller; 
    private bool isGrounded;
    private Vector3 velocity;

    public InputActionAsset inputActions;

    private InputAction moveAction;
    private Vector2 moveAmt; // amt = amount - значение

    private InputAction jumpAction; 
    private InputAction lookAction; 

    public InputAction interactAction;


    void Awake()
    {
        self = gameObject;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        controller = GetComponent<CharacterController>();
        playerCamera = GetComponent<PlayerCamera>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Запускается при появлении объекта на сцене
    void OnEnable()
    {
        // Привязали карту взаимодействий при появлении персонажа
        inputActions.FindActionMap("Player").Enable();
        jumpAction.performed += Jump;
        
    }

    void OnDisable()
    {
        // Отвязали карту взаимодействий при удалении персонажа
        inputActions.FindActionMap("Player").Disable();
        jumpAction.performed -= Jump;
    }


    void Update()
    {
        Debug.Log(controller.isGrounded);

        isGrounded = controller.isGrounded;

        // Прижать к земле
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward.normalized;
        Vector3 right = cam.right.normalized;
        
        forward.y = 0;
        right.y = 0; 

        moveAmt = moveAction.ReadValue<Vector2>();  
        animator.SetFloat("speed_horizontal", moveAmt.magnitude);

        // Окончательный вектор
        Vector3 move = right * moveAmt.x + forward * moveAmt.y; 
        
        if (move != Vector3.zero)
        {
            move.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += Physics.gravity.y * Time.deltaTime;

        animator.SetFloat("speed_vertical", controller.velocity.y);

        controller.Move(velocity * Time.deltaTime);

        playerCamera.lookAmt = lookAction.ReadValue<Vector2>();
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            // Закон сохранения энергии 
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }
}


