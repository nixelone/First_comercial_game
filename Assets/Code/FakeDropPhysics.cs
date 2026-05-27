using System.Collections;
using UnityEngine;

public class FakeDropPhysics : MonoBehaviour
{
    [Header("Drop Settings")]
    public float dropDuration = 0.5f;
    public float bounceHeight = 1.0f;
    public float scatterRadius = 1.5f;

    [Header("References")]
    public Transform visualSprite;

    private Collider2D pickupCollider;

    private void Awake()
    {
        // Grab the collider attached to this parent object
        pickupCollider = GetComponent<Collider2D>();

        // Disable the collider immediately so it cannot trigger pickups mid-air
        if (pickupCollider != null)
        {
            pickupCollider.enabled = false;
        }
    }

    private void Start()
    {
        StartCoroutine(DropRoutine());
    }

    private IEnumerator DropRoutine()
    {
        Vector3 startPosition = transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * scatterRadius;
        Vector3 targetPosition = startPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            float percent = elapsedTime / dropDuration;

            // Move the base along the ground
            transform.position = Vector3.Lerp(startPosition, targetPosition, percent);

            // Arcing the visual sprite on the local Y axis
            float currentHeight = Mathf.Sin(percent * Mathf.PI) * bounceHeight;
            visualSprite.localPosition = new Vector3(0, currentHeight, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // --- LANDING SEQUENCE ---
        transform.position = targetPosition;
        visualSprite.localPosition = Vector3.zero;

        // The log has hit the ground! Safely enable the collider so the player can collect it.
        if (pickupCollider != null)
        {
            pickupCollider.enabled = true;
        }
    }
}