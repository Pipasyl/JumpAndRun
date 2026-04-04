using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothLever : MonoBehaviour
{
    private bool on = false;
    private bool interpolating = false;
    private float currentInterpolationTime = 0.0f;

    private InputAction interactAction;
    private bool playerInRange = false;

    [Header("Lever Settings")]
    [SerializeField] private float switchTime = 1f;
    [SerializeField] private Transform onPosition;
    [SerializeField] private Transform offPosition;
    [SerializeField] private GameObject leverHandle;

    private void Start()
    {
        if (InputSystem.actions != null)
        {
            interactAction = InputSystem.actions.FindAction("Interact");
        }
        else
        {
            Debug.LogWarning("InputSystem default actions not found.");
        }
    }

    private void Update()
    {
        if (interactAction != null && interactAction.WasPressedThisFrame())
        {
            if (playerInRange && !interpolating)
            {
                ToggleLever();
            }
        }
    }

    private void ToggleLever()
    {
        on = !on;
        StartCoroutine(InterpolateLeverCoroutine());
    }

    private IEnumerator InterpolateLeverCoroutine()
    {
        interpolating = true;

        if (onPosition == null || offPosition == null || leverHandle == null)
        {
            Debug.LogError("Lever variables are missing in the Inspector!");
            yield break;
        }

        Vector3 startPosition = on ? offPosition.position : onPosition.position;
        Quaternion startRotation = on ? offPosition.rotation : onPosition.rotation;

        Vector3 targetPosition = on ? onPosition.position : offPosition.position;
        Quaternion targetRotation = on ? onPosition.rotation : offPosition.rotation;

        currentInterpolationTime = 0.0f;

        while (currentInterpolationTime < switchTime)
        {
            float percentage = currentInterpolationTime / switchTime;

            var currentPosition = Vector3.Lerp(startPosition, targetPosition, percentage);
            var currentRotation = Quaternion.Slerp(startRotation, targetRotation, percentage);

            leverHandle.transform.SetPositionAndRotation(currentPosition, currentRotation);

            yield return null;
            currentInterpolationTime += Time.deltaTime;
        }

        // Snap perfectly to the final position at the end of the animation
        leverHandle.transform.SetPositionAndRotation(targetPosition, targetRotation);
        interpolating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            playerInRange = false;
        }
    }
}