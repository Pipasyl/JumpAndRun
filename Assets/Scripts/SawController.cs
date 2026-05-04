using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SawController : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerSecond = 25;

    [Header("Spinning")]
    [SerializeField] private float spinSpeed = 300f;
    [SerializeField] private Vector3 spinAxis = Vector3.forward;

    [Header("Audio")]
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip cuttingSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem sparklingParticles;

    private AudioSource audioSource;
    private bool isCutting;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        if (sparklingParticles != null)
            sparklingParticles.Stop();
    }

    private void Start()
    {
        if (idleSound != null)
        {
            audioSource.clip = idleSound;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(false);
        }
    }

    private void OnTriggerStay(Collider other)
{
    Debug.Log("OnTriggerStay hit by: " + other.name + " tag: " + other.tag);
    if (other.CompareTag("Player"))
    {
        var character = other.GetComponentInParent<Character>();
        if (character != null)
        {
            character.InflictDamage(this.damagePerSecond * Time.fixedDeltaTime);
        }
    }
}

    private void SetState(bool newState)
    {
        if (isCutting == newState)
            return;

        if (newState)
        {
            isCutting = true;
            if (cuttingSound != null)
            {
                audioSource.clip = cuttingSound;
                audioSource.Play();
            }
            if (sparklingParticles != null)
                sparklingParticles.Play();
        }
        else
        {
            isCutting = false;
            if (idleSound != null)
            {
                audioSource.clip = idleSound;
                audioSource.Play();
            }
            if (sparklingParticles != null)
                sparklingParticles.Stop();
        }
    }

    private void Update()
    {
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime);
    }
}