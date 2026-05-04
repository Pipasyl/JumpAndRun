using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

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
        cc.enabled = false;
        cc.transform.position = this.respawnPoint.position;
        cc.enabled = true;
    }
}