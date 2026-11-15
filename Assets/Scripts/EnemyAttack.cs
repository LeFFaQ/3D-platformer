using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int damage = 1;
    public float attackRange = 2f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    private EnemyAnimator animator;

    void Start()
    {
        animator = GetComponent<EnemyAnimator>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange);
            foreach (Collider player in hitPlayers)
            {
                if (player.CompareTag("Player"))
                {
                    CharacterHealth healthComponent = player.GetComponent<CharacterHealth>();
                    if (healthComponent != null)
                    {
                        healthComponent.Hit(damage);
                        animator.AnimateHit();
                        nextAttackTime = Time.time + (1f / attackRate);
                    }
                }
            }
        }
    }
}

