using UnityEngine;
using UnityEngine.UIElements;

public class AdviceSign : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Sign Content")]
    [TextArea(3, 5)] // This creates a nice big text box in the Inspector!
    [SerializeField] private string adviceMessage = "Type your advice here...";

    private Label signLabel;
    private VisualElement uiRoot;

    private void Start()
    {
        if (this.uiDocument != null)
        {
            uiRoot = this.uiDocument.rootVisualElement;
            signLabel = uiRoot.Q<Label>("sign");

            // Hide the UI when the game starts
            uiRoot.style.display = DisplayStyle.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if the Mage touches it
        if (!other.CompareTag("Player")) return;

        if (uiRoot != null && signLabel != null)
        {
            // Set the exact text you typed in the Inspector for this specific sign
            signLabel.text = this.adviceMessage;

            // Show the UI box
            uiRoot.style.display = DisplayStyle.Flex;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Only trigger if the Mage walks away
        if (!other.CompareTag("Player")) return;

        if (uiRoot != null)
        {
            // Hide the UI box
            uiRoot.style.display = DisplayStyle.None;
        }
    }
}