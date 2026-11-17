using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    private static readonly int HorizontalSpeedHash = Animator.StringToHash("speed_horizontal");

    private static readonly int VerticalSpeedHash = Animator.StringToHash("speed_vertical");

    private static readonly int GroundedHash = Animator.StringToHash("isGrounded");


    private Animator animator;
    private PlayerMotor motor;

    void Awake()
    {
        animator = GetComponent<Animator>();
        motor = GetComponent<PlayerMotor>();
    }

    
    void Update()
    {
        if (animator == null) return;

        // Обновление аниматора
        animator.SetFloat(HorizontalSpeedHash, motor.HorizontalSpeed);

        animator.SetFloat(VerticalSpeedHash, Mathf.Abs(motor.VerticalVelocity));

        animator.SetBool(GroundedHash, motor.IsGrounded);    
    }
}
