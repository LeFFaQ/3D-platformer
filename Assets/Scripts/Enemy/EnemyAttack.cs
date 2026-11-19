using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
public sealed class EnemyAttack : MonoBehaviour
{
    [SerializeField, Min(0)] private int damage = 1;
    [SerializeField, Min(0.1f)] private float attackRange = 2f;
    [SerializeField, Min(0.01f)] private float attackRate = 1f;
    [SerializeField] private LayerMask playerLayer;

    private static readonly Collider[] HitResults = new Collider[16];

    private EnemyAnimator enemyAnimator;
    private EnemySystem enemySystem;

    private float nextAttackTime;

    private void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemySystem = GetComponent<EnemySystem>();
    }

    private void Update()
    {
        if (Time.time < nextAttackTime)
            return;

        Transform target = enemySystem.Target;

        if (target == null)
            return;

        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y;

        float distance =
            (targetPosition - transform.position).sqrMagnitude;

        if (distance > attackRange * attackRange)
            return;

        TryAttack();
    }

    private void TryAttack()
    {
        nextAttackTime = Time.time + 1f / attackRate;

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            attackRange,
            HitResults,
            playerLayer);

        for (int i = 0; i < hitCount; i++)
        {
            CharacterHealth health =
                HitResults[i].GetComponentInParent<CharacterHealth>();

            if (health == null || health.IsDead)
                continue;

            health.Hit(damage);
            enemyAnimator.PlayAttack();
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}