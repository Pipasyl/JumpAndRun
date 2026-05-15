using UnityEngine;

public class EnemyCircle : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private bool isWalking = true;

    [SerializeField] private float damageAmount = 3f;
    [SerializeField] private float hitCooldown = 1.5f;
    private float nextHitTime;

    private EnemyHealth healthScript;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();
        timer = 3.0f;
        animator.SetBool("WalkingCircle", isWalking);

        // Start playing the walk sound if walking
        if (audioSource != null && walkSound != null && isWalking)
        {
            audioSource.clip = walkSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            isWalking = !isWalking;
            animator.SetBool("WalkingCircle", isWalking);
            timer = 3.0f;

            // Toggle the walk sound based on the state
            if (audioSource != null && walkSound != null)
            {
                if (isWalking)
                {
                    audioSource.clip = walkSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                else
                {
                    // Stop the looping walk sound when standing still
                    audioSource.Stop();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Check if player is high enough
        bool isPlayerAbove = other.transform.position.y > transform.position.y + 1.5f;

        if (isPlayerAbove)
        {
            // SQUASH!
            if (healthScript != null)
            {
                // Stop the walk sound immediately so it doesn't overlap the squash
                if (audioSource != null && isWalking)
                {
                    audioSource.Stop();
                }

                healthScript.SquashEnemy();
            }
        }
        else if (Time.time >= nextHitTime)
        {
            // Instantly turn the enemy to face the player so the kick visually connects!
            Vector3 lookPosition = other.transform.position;
            lookPosition.y = transform.position.y; // Keep him standing straight up
            transform.LookAt(lookPosition);

            // KICK!
            animator.SetTrigger("Kick");

            // Play Kick Sound
            if (audioSource != null && kickSound != null)
            {
                audioSource.PlayOneShot(kickSound);
            }

            Character player = other.GetComponent<Character>();
            if (player != null) player.InflictDamage(damageAmount);

            nextHitTime = Time.time + hitCooldown;
        }
    }
}