using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Localization;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private LocalizedString dialogueText;

    
    [SerializeField] private Character character;

    private bool canInteract;
    private InputAction inputAction;

    private void Start()
    {
        this.inputAction = InputSystem.actions.FindAction("Attack");
        this.inputAction.performed += ToggleDialogueBox;

        this.dialogueBox.SetActive(false);
        this.canInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        this.canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        this.canInteract = false;
        this.dialogueBox.SetActive(false);
    }

    private void ToggleDialogueBox(InputAction.CallbackContext context)
    {
        // Only trigger if the player is standing near the sign
        if (this.canInteract)
        {
            this.dialogueBox.SetActive(true);
            var uiDocument = this.dialogueBox.GetComponent<UIDocument>();
            var label = uiDocument.rootVisualElement.Q<Label>();
            label.text = this.dialogueText.GetLocalizedString();
        }
        else
        {
            // If they move away or aren't interacting, keep it closed
            this.dialogueBox.SetActive(false);
        }
    }
}
