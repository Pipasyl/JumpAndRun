using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;
    private Vector3 platformVelocity = Vector3.zero;  // ← add this line

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()   // ← was Update() before
    {
        float h = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float v = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.fixedDeltaTime;

        Vector3 moveDirection = new Vector3(h, 0, v);
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * 10f
            );
        }

        if (controller.isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = jumpForce;
        }

        velocity.y -= gravity * Time.fixedDeltaTime;
        move.y = velocity.y * Time.fixedDeltaTime;

        GetPlatformVelocity();

        var combinedMovement = move + platformVelocity * Time.fixedDeltaTime;
        controller.Move(combinedMovement);
    }

    private void GetPlatformVelocity()
    {
        RaycastHit hit;
        int platformLayer = LayerMask.GetMask("Platforms");

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, platformLayer))
        {
            MovingPlatform platform = hit.collider.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platformVelocity = platform.GetVelocity();
            }
        }
        else
        {
            platformVelocity = Vector3.zero;
        }
    }
}