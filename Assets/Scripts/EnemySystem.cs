
using UnityEngine;
using UnityEngine.AI;

public class EnemySystem : MonoBehaviour
{
    
    public Transform target; 

    public LayerMask groundLayer;
    
    public float chaseDistance = 10f;
    public float stoppingDistance = 1.5f;

    private bool isGrounded;
    private bool isChased = false;
    private Vector3 originalPosition;

    private EnemyAnimator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        animator = GetComponent<EnemyAnimator>();

        originalPosition = transform.position;

    }

    void Start()
    {
        // if (target != null &&  PlayerSystem.Instance != null)
        // {
        //     target = PlayerSystem.Instance.transform;
        // }
    }

    void Update()
    {   
        Vector2 horizontalVelocity = new Vector2(agent.velocity.x, agent.velocity.z);
        if (horizontalVelocity != Vector2.zero)
        {
            animator.AnimateWalk(horizontalVelocity.magnitude);
        }
        animator.isGrounded = isGrounded;

        if (target == null || !agent.isOnNavMesh)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= chaseDistance && agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            isChased = true;
            
            Quaternion lookRotation = Quaternion.LookRotation(target.position);
            agent.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
            agent.SetDestination(target.position);
        }
        else
        {
            
            isChased = false;
        }

        if (!isChased && Vector3.Distance(transform.position, originalPosition) >= 0.5f)
        {
            agent.SetDestination(originalPosition);
        }
           
        
    
    }

    void OnCollisionEnter(Collision collision)
    {
        // 7 => Layer "AI Surface"
        
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
        } 
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        } 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
