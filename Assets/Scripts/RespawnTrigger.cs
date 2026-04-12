using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    // Use OnTriggerEnter OR OnControllerColliderHit to be 100% safe
    void OnTriggerEnter(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null)
        {
            Respawn(controller);
        }
    }

    private void Respawn(CharacterController controller)
    {
        // 1. Kill the movement entirely
        controller.enabled = false;

        // 2. Move to the point
        controller.transform.position = respawnPoint.position;

        // 3. FORCE Unity to update the physics engine immediately
        Physics.SyncTransforms();

        // 4. Turn it back on
        controller.enabled = true;
    }
}