using UnityEngine;

public class EnemyPunch : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float damageAmount = 3f;
    [SerializeField] private float hitCooldown = 2.0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private AudioSource audioSource;

    private float nextHitTime;
    private EnemyHealth healthScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();

        // Automatically grab the AudioSource if it's on the same object
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 1. SQUASH CHECK
        if (other.transform.position.y > transform.position.y + 1.4f)
        {
            if (healthScript != null) healthScript.SquashEnemy();
            return;
        }

        // 2. PUNCH CHECK
        if (Time.time >= nextHitTime)
        {
            // Only rotate ONCE at the start of the punch
            Vector3 targetPos = other.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);

            // Trigger Animation
            if (animator != null) animator.SetTrigger("Punch");

            // Deal Damage
            Character player = other.GetComponentInParent<Character>();
            if (player == null) player = other.GetComponent<Character>();

            if (player != null)
            {
                player.InflictDamage(damageAmount);

                // Play the kick/punch sound!
                if (kickSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(kickSound);
                }

                Debug.Log("Mage Punched! Health reduced.");
            }

            nextHitTime = Time.time + hitCooldown;
        }
    }
}