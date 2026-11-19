using UnityEngine;

public sealed class EnemyAnimator : MonoBehaviour
{
    private static readonly int GroundedHash =
        Animator.StringToHash("isGrounded");

    private static readonly int HorizontalSpeedHash =
        Animator.StringToHash("speed_horizontal");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private static readonly int JumpHash =
        Animator.StringToHash("Jump");

    private Animator animator;

    private float currentSpeed;
    private bool isGrounded;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError($"{name}: Animator was not found.", this);
    }

    public void SetGrounded(bool value)
    {
        if (animator == null || isGrounded == value)
            return;

        isGrounded = value;
        animator.SetBool(GroundedHash, value);
    }

    public void SetWalkSpeed(float value)
    {
        if (animator == null ||
            Mathf.Abs(currentSpeed - value) < 0.01f)
        {
            return;
        }

        currentSpeed = value;
        animator.SetFloat(HorizontalSpeedHash, value);
    }

    public void PlayAttack()
    {
        if (animator != null)
            animator.SetTrigger(AttackHash);
    }

    public void PlayJump()
    {
        if (animator != null)
            animator.SetTrigger(JumpHash);
    }
}