
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof (PlayerCamera))]
[RequireComponent(typeof (CharacterHealth))]
public class PlayerMotor : MonoBehaviour
{

    [Header("Player parameters")]
    [SerializeField, Min(0f)] private float speed = 3f;
    [SerializeField, Min(0f)] private float jumpHeight = 1f;
    [SerializeField, Min(0f)] private float rotationSpeed = 10f;
    [SerializeField] private Transform movementReference;

    private CharacterController controller;
    
    private PlayerInputHandler input;
    
    private PlayerCamera playerCamera;

    private float verticalVelocity;
    
    public bool IsGrounded { get; private set; }
    public float VerticalVelocity => verticalVelocity;
    public float HorizontalSpeed { get; private set; }

    void Awake()
    {

        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputHandler>();
        playerCamera = GetComponent<PlayerCamera>();

        if (movementReference == null && playerCamera != null)
            movementReference = playerCamera.GetMovementReference();

    }

    void Update()
    {
        float dt = Time.deltaTime;
        IsGrounded = controller.isGrounded;

        if (IsGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        Vector2 inputDirection = input.Move;
        Vector3 movement = GetMovementDirection(inputDirection);

        HorizontalSpeed = movement.magnitude;

        // Считаем направление и поворот только если игрок нажимает кнопки движения
        if (movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * dt);
        }

        verticalVelocity += Physics.gravity.y * dt;

        

        // Итоговое перемещение
        Vector3 velocity = movement * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * dt);

        //playerCamera. = lookAction.ReadValue<Vector2>();
    }

    private Vector3 GetMovementDirection(Vector2 inputDirection)
    {
        if (movementReference == null) 
            return new Vector3(inputDirection.x, 0f, inputDirection.y); 

        Vector3 forward = movementReference.forward;
        Vector3 right = movementReference.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return right * inputDirection.x + forward * inputDirection.y;
    }

    private void Jump()
    {
        if (!IsGrounded) return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
    }

    private void OnEnable()
    {
        input.JumpPressed += Jump;
    }

    private void OnDisable()
    {
        input.JumpPressed -= Jump;
    }
}