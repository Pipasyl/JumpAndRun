using UnityEngine;

public class SawController : MonoBehaviour
{
    [Header("Spinning")]
    [SerializeField] private float spinSpeed = 300f;
    [SerializeField] private Vector3 spinAxis = Vector3.forward;

    [Header("Audio")]
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip cuttingSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem cutEffect;
    [SerializeField] private float normalEmission = 20f;
    [SerializeField] private float cuttingEmission = 100f;

    private AudioSource audioSource;
    private bool isCutting;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = true;
    }

    private void Start()
    {
        if (idleSound != null)
        {
            audioSource.clip = idleSound;
            audioSource.Play();
        }
    }

    private void Update()
    {
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetState(false);
    }

    private void SetState(bool newState)
    {
        if (isCutting == newState) return;

        isCutting = newState;
        audioSource.clip = isCutting ? cuttingSound : idleSound;
        audioSource.Play();

        if (cutEffect != null)
        {
            var emission = cutEffect.emission;
            emission.rateOverTime = isCutting ? cuttingEmission : normalEmission;
        }
    }
}