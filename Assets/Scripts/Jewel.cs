using UnityEngine;

public class Jewel : MonoBehaviour
{
    public GameObject victoryCanvas;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private GameObject explosionParticlePrefab;  // ← ADD THIS
    private CanvasGroup victoryCanvasGroup;

    private void Start()
    {
        victoryCanvasGroup = victoryCanvas.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!EnemyHealth.requiredEnemyDefeated)
            {
                Debug.Log("You must defeat the enemy guarding the jewel first!");
                return;
            }

            // Spawn explosion BEFORE hiding jewel
            if (explosionParticlePrefab != null)
            {
                GameObject particles = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 2f);
            }

            // Show victory
            victoryCanvas.SetActive(true);
            victoryCanvasGroup.alpha = 1f;
            victoryCanvasGroup.interactable = true;
            victoryCanvasGroup.blocksRaycasts = true;

            gameObject.SetActive(false);
        }
    }
}