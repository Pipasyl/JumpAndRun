using UnityEngine;

public class EnemyCircle : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private bool isWalking = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = 3.0f;
        animator.SetBool("WalkingCircle", isWalking);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            isWalking = !isWalking;
            animator.SetBool("WalkingCircle", isWalking);
            timer = 3.0f;
        }
    }
}