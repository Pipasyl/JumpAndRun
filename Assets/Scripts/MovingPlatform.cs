using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float platformSpeed;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;

    private Vector3 lastPosition;
    private Vector3 velocity;

    void Start()
    {
        lastPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        float pingPong = Mathf.PingPong(Time.fixedTime * this.platformSpeed, 1.0f);
        var newPosition = Vector3.Lerp(this.start, this.end, pingPong);
        this.transform.localPosition = newPosition;

        // Calculate velocity
        velocity = (transform.localPosition - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.localPosition;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
}