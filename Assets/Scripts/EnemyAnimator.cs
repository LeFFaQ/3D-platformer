using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public bool isGrounded = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    public void AnimateWalk(float velocity)
    {
        animator.SetFloat("speed_horizontal", velocity);
    }

    public void AnimateHit()
    {
        animator.SetTrigger("Attack");
    }

    public void AnimateJump()
    {
        animator.SetTrigger("Jump");
    }
}
