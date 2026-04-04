using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Powerup Settings")]
    [SerializeField] private float powerupChancePerStrength = 0.01f;
    [SerializeField] private GameObject powerupSprite;
    [SerializeField] private GameObject powerupPrefab;

    [Header("Block Visuals")]
    [SerializeField] private Gradient blockColor;
    [SerializeField, Range(0, 9)] public int startStrength;
    [SerializeField] private MeshRenderer blockMesh;

    private const int MAX_BLOCK_STRENGTH = 9;

    // Caching the shader property ID is much faster than using the string "_Tint"
    private static readonly int TintPropertyId = Shader.PropertyToID("_Tint");

    private bool hasPowerup = false;
    private int currentStrength = 0;
    private MaterialPropertyBlock mpb;

    private void Start()
    {
        mpb = new MaterialPropertyBlock();
        currentStrength = startStrength;

        float powerupChance = powerupChancePerStrength * startStrength;

        // Determine if this block gets a powerup
        if (Random.value < powerupChance)
        {
            hasPowerup = true;
            if (powerupSprite != null) powerupSprite.SetActive(true);
        }
        else
        {
            // Make sure the sprite is hidden if it didn't get a powerup
            if (powerupSprite != null) powerupSprite.SetActive(false);
        }

        // Set the initial color once at the start!
        UpdateColor();
    }

    /// <summary>
    /// Updates the block's material color based on its current strength.
    /// Call this whenever the block takes damage!
    /// </summary>
    private void UpdateColor()
    {
        if (blockMesh == null) return;

        float strengthPercent = currentStrength / (float)MAX_BLOCK_STRENGTH;
        Color color = blockColor.Evaluate(strengthPercent);

        // Best practice: get the current block, modify it, then set it back
        blockMesh.GetPropertyBlock(mpb);
        mpb.SetColor(TintPropertyId, color);
        blockMesh.SetPropertyBlock(mpb);
    }

    private void OnDestroy()
    {
        // Only spawn the powerup if the scene is active (prevents errors when closing the game)
        if (hasPowerup && gameObject.scene.isLoaded && powerupPrefab != null)
        {
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // This is where you will handle the block getting hit!
        // Example of what you will likely add here later:

        /*
        currentStrength--;
        UpdateColor(); // Update the visuals ONLY when it gets hit!
        
        if (currentStrength <= 0)
        {
            Destroy(gameObject);
        }
        */
    }
}
