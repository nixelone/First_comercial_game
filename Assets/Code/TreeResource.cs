using UnityEngine;

public class TreeResource : MonoBehaviour, IDamageable
{
    [Header("Tree Visuals")]
    public SpriteRenderer mainSpriteRenderer;
    public Sprite[] treeSprites;

    [Header("Health & UI")]
    public int maxHealth = 3;
    private int currentHealth;

    // The parent object holding the blocks, so we can hide the whole thing at once
    public GameObject healthBarContainer;
    // The individual green sprite blocks
    public GameObject[] healthBlocks;

    [Header("Drops")]
    public GameObject logPrefab;
    public int logDropCount = 2;

    private void Start()
    {
        currentHealth = maxHealth;

        // 1. Pick a random sprite on spawn
        if (treeSprites.Length > 0 && mainSpriteRenderer != null)
        {
            int randomIndex = Random.Range(0, treeSprites.Length);
            mainSpriteRenderer.sprite = treeSprites[randomIndex];
        }

        // 2. Hide the health bar when at max health
        if (healthBarContainer != null)
        {
            healthBarContainer.SetActive(false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Show the health bar the moment we take any damage
        if (currentHealth < maxHealth && healthBarContainer != null)
        {
            healthBarContainer.SetActive(true);
        }

        UpdateHealthBarVisuals();

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBarVisuals()
    {
        // Loop through our green blocks and turn them on/off based on current health
        for (int i = 0; i < healthBlocks.Length; i++)
        {
            if (healthBlocks[i] != null)
            {
                // If the block's index is lower than current health, it stays ON.
                // E.g., If health is 2, block[0] and block[1] are ON. block[2] turns OFF.
                healthBlocks[i].SetActive(i < currentHealth);
            }
        }
    }

    private void Die()
    {
        // Spawn the logs exactly at the tree's base
        for (int i = 0; i < logDropCount; i++)
        {
            Instantiate(logPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the tree (or you could swap the mainSpriteRenderer.sprite to a Stump here!)
        Destroy(gameObject);
    }
}