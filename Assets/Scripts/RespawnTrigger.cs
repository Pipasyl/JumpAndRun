using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        CharacterController controller = other.gameObject.GetComponent<CharacterController>();

        if (controller != null)
        {
            Respawn(controller);
        }
    }

    private void Respawn(CharacterController controller)
    {
        // Step 1: disable controller to avoid collision issues
        controller.enabled = false;

        // Step 2: move player to respawn point
        controller.gameObject.transform.position = respawnPoint.position;

        // Step 3: re-enable controller
        controller.enabled = true;
    }
}