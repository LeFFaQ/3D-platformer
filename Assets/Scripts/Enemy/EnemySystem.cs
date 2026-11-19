using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimator))]
public sealed class EnemySystem : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float chaseDistance = 10f;
    [SerializeField] private float targetUpdateRate = 0.25f;

    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float pathUpdateRate = 0.1f;
    [SerializeField] private float returnDistance = 0.5f;

    private NavMeshAgent agent;
    private EnemyAnimator enemyAnimator;

    private Vector3 spawnPosition;
    private Transform target;

    private float nextTargetUpdate;
    private float nextPathUpdate;

    public Transform Target => target;
    public bool HasTarget => target != null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<EnemyAnimator>();

        spawnPosition = transform.position;
        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (!agent.isOnNavMesh)
            return;

        UpdateTarget();
        UpdateMovement();
        UpdateAnimation();
    }

    private void UpdateTarget()
    {
        if (Time.time < nextTargetUpdate)
            return;

        nextTargetUpdate = Time.time + targetUpdateRate;

        target = FindClosestPlayer();

        if (target == null)
        {
            float distanceFromSpawn =
                (transform.position - spawnPosition).sqrMagnitude;

            if (distanceFromSpawn > returnDistance * returnDistance)
                MoveTo(spawnPosition);

            return;
        }

        float distanceToTarget =
            (target.position - transform.position).sqrMagnitude;

        if (distanceToTarget > chaseDistance * chaseDistance)
            target = null;
    }

    private void UpdateMovement()
    {
        if (target == null)
        {
            float distanceFromSpawn =
                (transform.position - spawnPosition).sqrMagnitude;

            if (distanceFromSpawn > returnDistance * returnDistance)
                MoveTo(spawnPosition);
            else
                agent.ResetPath();

            return;
        }

        MoveTo(target.position);
    }

    private void MoveTo(Vector3 destination)
    {
        if (Time.time < nextPathUpdate)
            return;

        nextPathUpdate = Time.time + pathUpdateRate;

        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    private void UpdateAnimation()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0f;

        enemyAnimator.SetWalkSpeed(velocity.magnitude);
        enemyAnimator.SetGrounded(true);
    }

    private Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Transform closestPlayer = null;
        float closestDistance = chaseDistance * chaseDistance;

        foreach (GameObject player in players)
        {
            if (!player.activeInHierarchy)
                continue;

            CharacterHealth health =
                player.GetComponentInParent<CharacterHealth>();

            if (health != null && health.IsDead)
                continue;

            float distance =
                (player.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        return closestPlayer;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
        agent.ResetPath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}