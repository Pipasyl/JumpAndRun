using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static bool requiredEnemyDefeated = false;

    [SerializeField] private AudioClip squashSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject deathParticlePrefab;

    public void SquashEnemy()
    {
        requiredEnemyDefeated = true;

        // NEW: Turn off the Animator so it stops forcing the bones back to normal size!
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }

        if (deathParticlePrefab != null)
        {
            GameObject particles = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particles, 1f);
        }

        if (audioSource && squashSound)
        {
            audioSource.PlayOneShot(squashSound);
        }

        // Squash him into a pancake
        transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 0.2f, transform.localScale.z * 1.5f);

        // Changed to 0.5f so he stays on the ground as a pancake for half a second before disappearing!
        Destroy(gameObject, 0.5f);
    }
}