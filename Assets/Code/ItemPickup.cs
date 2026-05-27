using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Resource Settings")]
    public ResourceType resourceType = ResourceType.Wood;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Verify it is actually the player touching the item
        if (other.CompareTag("Player"))
        {
            // 2. Try to grab the Wallet component from the player
            ResourceWallet playerWallet = other.GetComponent<ResourceWallet>();

            if (playerWallet != null)
            {
                // 3. Add the wood to the ledger
                playerWallet.AddResource(resourceType, amount);

                // (Optional: Play a satisfying 'pop' audio clip here)

                // 4. Destroy the log so it can't be picked up twice
                Destroy(gameObject);
            }
        }
    }
}