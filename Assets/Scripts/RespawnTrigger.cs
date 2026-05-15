using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject respawnPoofPrefab; // ← ADD

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();
        if (character != null)
        {
            // Set health to 0 to trigger the UI fade from the slides
            character.InflictDamage(character.GetMaxHealth());
        }
    }

    private void Respawn(CharacterController cc)
    {
        // ← POOF at death position FIRST
        if (respawnPoofPrefab != null)
        {
            GameObject poof = Instantiate(respawnPoofPrefab, cc.transform.position, Quaternion.identity);
            Destroy(poof, 1f);
        }

        cc.enabled = false;
        cc.transform.position = respawnPoint.position;
        cc.enabled = true;

        // ← POOF at respawn position too
        if (respawnPoofPrefab != null)
        {
            GameObject poof = Instantiate(respawnPoofPrefab, respawnPoint.position, Quaternion.identity);
            Destroy(poof, 1f);
        }
    }
}