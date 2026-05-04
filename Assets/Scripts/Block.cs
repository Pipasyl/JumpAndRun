using UnityEngine;

public class Block : MonoBehaviour
{
    #region Public Fields

    public float powerupChancePerStrength = 0.01f;

    public GameObject powerupSprite;
    public GameObject powerupPrefab;

    public Gradient blockColor;

    [Range(0, 9)]
    public int startStrength;

    public MeshRenderer blockMesh;

    #endregion

    #region Private Fields

    private static readonly int MAX_BLOCK_STRENGTH = 9;
    private static readonly string BLOCK_TINT = "_Tint";

    private bool hasPowerup = false;

    private float powerupChance = 0.0f;

    private int currentStrength = 0;

    private MaterialPropertyBlock mpb;

    #endregion

    void Start()
    {
        this.mpb = new MaterialPropertyBlock();
        this.currentStrength = this.startStrength;
        this.powerupChance = this.powerupChancePerStrength * this.startStrength;

        if (Random.value < this.powerupChance)
        {
            this.powerupSprite.SetActive(true);
            this.hasPowerup = true;
        }
    }

    void Update()
    {
        float strengthPercent = this.currentStrength / (float)MAX_BLOCK_STRENGTH;
        var color = this.blockColor.Evaluate(strengthPercent);

        this.mpb.SetColor(BLOCK_TINT, color);
        this.blockMesh.SetPropertyBlock(this.mpb);
    }

    private void OnDestroy()
    {
        if (this.hasPowerup)
        {
            if (!this.gameObject.scene.isLoaded) return;
            var powerup = GameObject.Instantiate(this.powerupPrefab);
            powerup.transform.position = this.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}