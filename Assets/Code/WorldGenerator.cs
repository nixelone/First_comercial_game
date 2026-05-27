using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSizeX = 50;
    public int gridSizeY = 50;
    public float cellSize = 1f;

    [Header("Seed Settings")]
    public bool useRandomSeed = true;
    public int seed = 12345;

    [Header("Perlin Settings")]
    public float noiseScale = 10f; // Higher number = larger, more spread out forests
    [Range(0f, 1f)] public float treeThreshold = 0.55f;

    [Header("Prefabs")]
    public GameObject treePrefab;
    public GameObject unbreakableStonePrefab;

    // The data map. 0 = Empty, 1 = Tree, 2 = Stone Border
    private int[,] mapData;

    private void Start()
    {
        GenerateMapData();
        BuildWorldFromData();
    }

    private void GenerateMapData()
    {
        mapData = new int[gridSizeX, gridSizeY];

        // 1. Handle the Seed
        if (useRandomSeed)
        {
            seed = Random.Range(0, 999999);
            Debug.Log("Generated random map with seed: " + seed);
        }

        // Initialize a standard C# random number generator using our exact seed
        System.Random prng = new System.Random(seed);

        // Pick a random starting point for the noise based on the seed
        float offsetX = prng.Next(-100000, 100000);
        float offsetY = prng.Next(-100000, 100000);

        // 2. Loop through the grid to fill the integer map
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // A. Check if we are on the extreme outer edge (The Border)
                if (x == 0 || x == gridSizeX - 1 || y == 0 || y == gridSizeY - 1)
                {
                    mapData[x, y] = 2; // 2 represents Stone
                }
                else
                {
                    // B. Not a border, so calculate Perlin Noise for trees
                    float sampleX = (x / noiseScale) + offsetX;
                    float sampleY = (y / noiseScale) + offsetY;

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

                    if (noiseValue > treeThreshold)
                    {
                        mapData[x, y] = 1; // 1 represents Tree
                    }
                    else
                    {
                        mapData[x, y] = 0; // 0 represents Empty Grass
                    }
                }
            }
        }
    }

    private void BuildWorldFromData()
    {
        // Re-initialize the random generator with our saved seed 
        // This guarantees the pixel offsets are identical every time you load this specific map!
        System.Random prng = new System.Random(seed);

        // 3. Read the array and spawn the actual GameObjects
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Default center position of the grid cell
                float xPos = x * cellSize;
                float yPos = y * cellSize;

                if (mapData[x, y] == 1)
                {
                    // --- THE PIXEL OFFSET MATH ---
                    // prng.Next(min, max) is exclusive on the max bound, so we use (-2, 3) to get -2, -1, 0, 1, or 2.
                    int pixelOffsetX = prng.Next(-3, 4);
                    int pixelOffsetY = prng.Next(-3, 4);

                    // Convert those strict pixels into Unity world units (based on 32 PPU)
                    float unitOffsetX = pixelOffsetX / 32f;
                    float unitOffsetY = pixelOffsetY / 32f;

                    // Apply the offset to the tree's spawn position
                    Vector3 treeSpawnPos = new Vector3(xPos + unitOffsetX, yPos + unitOffsetY, 0);
                    Instantiate(treePrefab, treeSpawnPos, Quaternion.identity, transform);
                }
                else if (mapData[x, y] == 2)
                {
                    // Stones stay perfectly snapped to the grid to form a clean, unbroken wall
                    Vector3 stoneSpawnPos = new Vector3(xPos, yPos, 0);
                    Instantiate(unbreakableStonePrefab, stoneSpawnPos, Quaternion.identity, transform);
                }
            }
        }
    }
}