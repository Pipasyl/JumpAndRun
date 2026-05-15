using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [Header("Detection & Movement")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f; // How close he needs to be to kick
    [SerializeField] private float moveSpeed = 3f;

    [Header("Combat Settings")]
    [SerializeField] private float damageAmount = 15f;
    [SerializeField] private float attackCooldown = 2.0f; // Seconds between kicks

    private Transform playerTransform;
    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();

        // This finds the object you tagged as "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 1. If very close, KICK
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        // 2. If close but not close enough to kick, CHASE
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        // 3. Otherwise, IDLE
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        animator.SetBool("IsRunning", false);
    }

    private void ChasePlayer()
    {
        animator.SetBool("IsRunning", true);

        // Rotate to look at player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;

        // Move forward
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("Kick");
            collision.gameObject.GetComponent<Character>().InflictDamage(10f);
        }
    }
    private void AttackPlayer()
    {
        // Stop running to kick
        animator.SetBool("IsRunning", false);

        // Face the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;

        // Only kick if enough time has passed (Cooldown)
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // PULL THE TRIGGER in the Animator
            animator.SetTrigger("Kick");

            // DAMAGE the player using your Character script
            Character playerScript = playerTransform.GetComponent<Character>();
            if (playerScript != null)
            {
                playerScript.InflictDamage(damageAmount);
            }

            lastAttackTime = Time.time;
        }
    }
}