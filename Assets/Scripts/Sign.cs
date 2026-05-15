using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;

public class Sign : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private LocalizedString dialogueText;
    [SerializeField] private Character character;
    [SerializeField] private GameObject doorToOpen;
    [SerializeField] private GameObject respawnPoufPrefab; // ← NEW: Slot for your Pouf!
    [SerializeField] private string successText = "You got everything! Door is open.";
    [SerializeField] private string notEnoughText = "You need 5 coins to pass!";
    [SerializeField] private int coinsRequired = 5;

    private Label signLabel;
    private VisualElement uiRoot;

    private void Start()
    {
        if (this.uiDocument != null)
        {
            uiRoot = this.uiDocument.rootVisualElement;
            signLabel = uiRoot.Q<Label>("sign");
            uiRoot.style.display = DisplayStyle.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Character livePlayerChar = other.GetComponent<Character>();
        Character charToCheck = livePlayerChar != null ? livePlayerChar : this.character;

        if (uiRoot != null && signLabel != null)
        {
            uiRoot.style.display = DisplayStyle.Flex;

            if (charToCheck != null && charToCheck.coins >= this.coinsRequired)
            {
                signLabel.text = this.successText;
            }
            else
            {
                try
                {
                    if (this.dialogueText != null && !this.dialogueText.IsEmpty)
                        signLabel.text = this.dialogueText.GetLocalizedString();
                    else
                        signLabel.text = this.notEnoughText;
                }
                catch
                {
                    signLabel.text = this.notEnoughText;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Character livePlayerChar = other.GetComponent<Character>();
        Character charToCheck = livePlayerChar != null ? livePlayerChar : this.character;

        if (uiRoot != null)
        {
            uiRoot.style.display = DisplayStyle.None;
        }

        if (charToCheck != null && charToCheck.coins >= this.coinsRequired)
        {
            if (this.doorToOpen != null)
            {
                Destroy(this.doorToOpen);
            }

            // ← NEW: Spawn the Pouf effect at the sign's exact position
            if (this.respawnPoufPrefab != null)
            {
                Instantiate(this.respawnPoufPrefab, transform.position, Quaternion.identity);
            }

            this.gameObject.SetActive(false);
        }
    }
}