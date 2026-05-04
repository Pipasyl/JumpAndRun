using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    private float currentHealth;

    public float GetCurrentHealth() => this.currentHealth;
    public float GetMaxHealth() => this.maxHealth;

    private bool isJumping = false;
    private bool wasGroundedLastFrame = true;

    private float jumpCooldownTimer;

    private CharacterController controller;

    
    private Animator animator;

    private InputAction moveAction;
    private InputAction jumpAction;

    [SerializeField] private float jumpCooldown;
    [SerializeField] private float characterSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float dampening;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpSpeed;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip footstepClip;

    private Vector3 characterMovement;
    private Vector3 characterGravity;
    private Vector3 jumpVelocity;
    private Vector3 platformVelocity;

    public void InflictDamage(float damage)
    {
        this.currentHealth -= damage;
        this.currentHealth = Mathf.Clamp(this.currentHealth, 0.0f, this.maxHealth);
    }

    private void HandleJumping()
    {
        if (this.controller.isGrounded && this.isJumping && this.jumpCooldownTimer <= 0.0f)
        {
            this.jumpVelocity = Vector3.zero;
            this.isJumping = false;
        }

        if (this.controller.isGrounded && !this.isJumping && this.jumpAction.WasPressedThisFrame())
        {
            this.characterGravity = Vector3.zero;
            this.jumpVelocity = Vector3.zero;
            this.jumpVelocity.y = this.jumpSpeed;
            this.jumpCooldownTimer = this.jumpCooldown;
            this.isJumping = true;

            if (sfxSource && jumpClip) sfxSource.PlayOneShot(jumpClip);
        }

        if (this.jumpVelocity.y > 0.0f)
        {
            this.jumpVelocity.y -= Time.fixedDeltaTime;
        }
        else
        {
            this.jumpVelocity = Vector3.zero;
        }

        this.jumpCooldownTimer -= Time.fixedDeltaTime;
    }

    
    void SetAnimationState(Vector2 inputMovement)
    {
        this.animator.SetBool("IsJumping", this.isJumping);
        this.animator.SetBool("IsRunning", inputMovement != Vector2.zero);
        this.animator.SetFloat("MovementForward", inputMovement.magnitude);
    }

    private void Start()
    {
        this.controller = this.GetComponent<CharacterController>();

        
        this.animator = this.GetComponent<Animator>();

        this.moveAction = InputSystem.actions.FindAction("Move");
        this.jumpAction = InputSystem.actions.FindAction("Jump");
        this.jumpCooldownTimer = 0.0f;
        this.currentHealth = this.maxHealth;

        if (footstepSource != null && footstepClip != null)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
        }
    }

    private void FixedUpdate()
    {
        this.HandleJumping();

        if (this.controller.isGrounded && !wasGroundedLastFrame)
        {
            if (sfxSource && landClip) sfxSource.PlayOneShot(landClip);
        }
        wasGroundedLastFrame = this.controller.isGrounded;

        var inputMovement = this.moveAction.ReadValue<Vector2>();

        
        this.SetAnimationState(inputMovement);

        if (this.controller.isGrounded && inputMovement.sqrMagnitude > 0.1f)
        {
            if (footstepSource && !footstepSource.isPlaying) footstepSource.Play();
        }
        else
        {
            if (footstepSource && footstepSource.isPlaying) footstepSource.Stop();
        }

        var inputForwardDirection = this.cameraTransform.forward;
        var inputRightDirection = this.cameraTransform.right;

        inputRightDirection.y = 0.0f;
        inputForwardDirection.y = 0.0f;

        inputForwardDirection.Normalize();
        inputRightDirection.Normalize();

        if (this.controller.isGrounded)
        {
            this.characterGravity.y = 0.0f;
        }

        this.characterGravity.y += this.gravity * Time.fixedDeltaTime;
        this.characterMovement += this.characterGravity * Time.fixedDeltaTime;
        this.characterMovement += this.jumpVelocity * Time.fixedDeltaTime;
        this.characterMovement += inputRightDirection * inputMovement.x * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement += inputForwardDirection * inputMovement.y * this.characterSpeed * Time.fixedDeltaTime;

        this.characterMovement *= (1.0f - this.dampening);
        this.GetPlatformVelocity();
        var combinedMovement = this.characterMovement + (!this.isJumping ? this.platformVelocity * Time.fixedDeltaTime : Vector3.zero);

        Vector3 characterForward = this.characterMovement;
        characterForward.y = 0.0f;

        if (characterForward.sqrMagnitude > 0.0f && characterForward != Vector3.zero)
        {
            this.transform.forward = characterForward.normalized;
        }

        this.controller.Move(combinedMovement);
    }

    private void GetPlatformVelocity()
    {
        int platformLayer = LayerMask.GetMask("Platforms");
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1.5f, platformLayer))
        {
            MovingPlatform platform = hit.collider.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                this.platformVelocity = platform.GetVelocity();
                return;
            }
        }
        this.platformVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            // Only kill if the Mage is falling (negative Y velocity) 
            // and is currently in the jumping state
            if (this.characterMovement.y < -0.1f && this.isJumping)
            {
                enemy.SquashEnemy();

                this.jumpVelocity = Vector3.zero;
                this.jumpVelocity.y = this.jumpSpeed * 0.7f;
                this.isJumping = true;
            }
        }
    }
}