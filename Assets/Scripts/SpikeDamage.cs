using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float damageAmount = 20f;
    public GameObject bloodParticlePrefab;

    [Header("Audio Settings")]
    public AudioClip hurtSound;
    public AudioSource audioSource;

    private void Start()
    {
        // Automatically grab the AudioSource if you forget to assign it
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Try finding Character on the object that touched the trigger
        Character player = other.GetComponent<Character>();

        // 2. If not found, look in the parent (common for CharacterControllers)
        if (player == null)
        {
            player = other.GetComponentInParent<Character>();
        }

        // 3. If found, hurt them
        if (player != null)
        {
            player.InflictDamage(damageAmount);

            // Play the hurt sound!
            if (hurtSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hurtSound);
            }

            if (bloodParticlePrefab != null)
            {
                Instantiate(bloodParticlePrefab, other.transform.position, Quaternion.identity);
            }
            Debug.Log("Mage hit by spikes!");
        }
    }
}