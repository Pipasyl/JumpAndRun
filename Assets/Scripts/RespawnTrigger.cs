using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject respawnPoofPrefab; // ← ADD

    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.gameObject.GetComponent<CharacterController>();
        if (cc != null)
        {
            Respawn(cc);
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