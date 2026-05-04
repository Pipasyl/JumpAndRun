using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private AudioClip squashSound;
    [SerializeField] private AudioSource audioSource;

    public void SquashEnemy()
    {
        if (audioSource && squashSound)
        {
            audioSource.PlayOneShot(squashSound);
        }

        transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 0.2f, transform.localScale.z * 1.5f);

        Destroy(gameObject, 0.2f);
    }
}