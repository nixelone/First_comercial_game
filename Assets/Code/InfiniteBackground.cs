using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Background Setup")]
    public GameObject backgroundPrefab; // Drag your background sprite prefab here

    private Transform[] tiles = new Transform[9];
    private float bgWidth;
    private float bgHeight;
    private Vector3 lastCameraPos;

    void Start()
    {
        // 1. Get the size of the sprite so we know how far apart to snap them
        SpriteRenderer sr = backgroundPrefab.GetComponent<SpriteRenderer>();
        bgWidth = sr.bounds.size.x;
        bgHeight = sr.bounds.size.y;

        // 2. Spawn the 9 tiles and store them in an array
        int index = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Instantiate them as children of the camera momentarily just to organize the hierarchy
                GameObject tile = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity);
                tile.name = $"BG_Tile_{index}";
                tiles[index] = tile.transform;
                index++;
            }
        }

        // Force a layout update immediately
        UpdateBackgroundGrid();
    }

    void LateUpdate()
    {
        // Only do the math if the camera has actually moved
        if (transform.position != lastCameraPos)
        {
            UpdateBackgroundGrid();
            lastCameraPos = transform.position;
        }
    }

    private void UpdateBackgroundGrid()
    {
        // Calculate which "cell" the camera is currently floating over
        int centerCol = Mathf.RoundToInt(transform.position.x / bgWidth);
        int centerRow = Mathf.RoundToInt(transform.position.y / bgHeight);

        // Position the 9 tiles in a 3x3 grid around that center cell
        int index = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                float targetX = (centerCol + x) * bgWidth;
                float targetY = (centerRow + y) * bgHeight;

                // Snap the tile to its correct position. Keep its original Z depth.
                tiles[index].position = new Vector3(targetX, targetY, tiles[index].position.z);
                index++;
            }
        }
    }
}