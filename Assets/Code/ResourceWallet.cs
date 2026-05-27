using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceWallet : MonoBehaviour
{
    // Broadcasts whenever a resource amount changes. 
    // Format: Action<The Resource That Changed, The New Total Amount>
    public event Action<ResourceType, int> OnResourceChanged;

    // The internal ledger. Dictionaries are invisible in the Inspector, 
    // but they are the safest, fastest way to pair data in C#.
    private Dictionary<ResourceType, int> storage = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        // Automatically initialize every resource type in your Enum to 0 on startup
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            storage[type] = 0;
        }
    }

    /// <summary>
    /// Adds resources to the wallet.
    /// </summary>
    public void AddResource(ResourceType type, int amount)
    {
        if (amount <= 0) return;

        storage[type] += amount;

        // Shout out to the game (UI, etc.) that this specific resource changed
        OnResourceChanged?.Invoke(type, storage[type]);
    }

    /// <summary>
    /// Checks if the wallet has enough of a specific resource to spend.
    /// </summary>
    public bool HasEnough(ResourceType type, int amountNeeded)
    {
        return storage[type] >= amountNeeded;
    }

    /// <summary>
    /// Attempts to spend resources. Returns true if successful.
    /// </summary>
    public bool TrySpendResource(ResourceType type, int amount)
    {
        if (amount <= 0) return false;

        if (HasEnough(type, amount))
        {
            storage[type] -= amount;

            // Shout out the updated lower amount
            OnResourceChanged?.Invoke(type, storage[type]);
            return true;
        }

        // Not enough resources to spend
        return false;
    }

    /// <summary>
    /// Returns the current amount of a specific resource (Useful for initial UI setup).
    /// </summary>
    public int GetAmount(ResourceType type)
    {
        return storage[type];
    }
}