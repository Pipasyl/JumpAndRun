using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private AudioClip squashSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject deathParticlePrefab;

    public void SquashEnemy()
    {
        // 1. Spawn sparkles at enemy position
        if (deathParticlePrefab != null)
        {
            GameObject particles = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particles, 1f);
        }

        // 2. Play sound
        if (audioSource && squashSound)
        {
            audioSource.PlayOneShot(squashSound);
        }

        // 3. Squash and destroy
        transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 0.2f, transform.localScale.z * 1.5f);
        Destroy(gameObject, 0.2f);
    }
}