using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static bool requiredEnemyDefeated = false;

    [SerializeField] private AudioClip squashSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject deathParticlePrefab;
    [SerializeField] private float damageAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if we hit the player
        if (other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();

            if (player != null)
            {
                player.InflictDamage(damageAmount);
            }
        }
    }

    public void SquashEnemy()
    {
        requiredEnemyDefeated = true;

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

        transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 0.2f, transform.localScale.z * 1.5f);

        Destroy(gameObject, 0.5f);
    }
}