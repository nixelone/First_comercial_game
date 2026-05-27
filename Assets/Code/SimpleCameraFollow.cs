using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target; // Drag your player here
    public float smoothTime = 0.15f; // Lower is snappier, higher is floatier
    public Vector3 offset = new Vector3(0, 0, -10f); // Keep Z at -10 so it stays behind the 2D world

    private Vector3 velocity = Vector3.zero;

    // We use LateUpdate so the camera moves AFTER the player's physics update
    void LateUpdate()
    {
        if (target == null) return;

        // The target position we want to reach
        Vector3 targetPosition = target.position + offset;

        // SmoothDamp smoothly glides the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}