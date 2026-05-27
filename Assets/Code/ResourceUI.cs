using UnityEngine;
using TMPro; // You must include this to talk to TextMeshPro

public class ResourceUI : MonoBehaviour
{
    [Header("Connections")]
    public ResourceWallet targetWallet; // Drag the Player here in the Inspector
    public TextMeshProUGUI textElement;

    [Header("Settings")]
    public ResourceType resourceToTrack = ResourceType.Wood;
    public string prefix = "Wood: "; // e.g., "Wood: 5"

    private void Awake()
    {
        // If you forgot to assign the text element, grab it automatically
        if (textElement == null)
            textElement = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (targetWallet != null)
        {
            // 1. Subscribe to the wallet's shout!
            targetWallet.OnResourceChanged += HandleResourceChanged;

            // 2. Force an immediate update so the UI doesn't say "0" if we already have 5 wood
            UpdateUI(targetWallet.GetAmount(resourceToTrack));
        }
    }

    private void OnDisable()
    {
        // CRITICAL: Always unsubscribe when UI is turned off or destroyed to prevent memory leaks
        if (targetWallet != null)
        {
            targetWallet.OnResourceChanged -= HandleResourceChanged;
        }
    }

    private void HandleResourceChanged(ResourceType changedType, int newAmount)
    {
        // We only care if the resource that just changed matches the one this UI is tracking
        if (changedType == resourceToTrack)
        {
            UpdateUI(newAmount);
        }
    }

    private void UpdateUI(int amount)
    {
        // Update the actual text on the screen
        textElement.text = prefix + amount.ToString();
    }
}