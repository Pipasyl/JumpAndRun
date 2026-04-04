using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Public Fields

    public GameObject ballPrefab;
    public GameObject blockPrefab;

    public List<Level> levels = new List<Level>();

    public Vector2Int tiles;
    public Rect bounds;

    public Vector3 spawnPos;

    #endregion

    #region Private Fields

    private GameObject currentBall = null;

    private int currentLevel = 0;

    private List<Block> currentBlocks = new List<Block>();


    #endregion

    public GameObject CurrentBall => this.currentBall;

    public void RespawnBall()
    {
        if (this.currentBall != null)
        {
            Destroy(this.currentBall);
        }

        this.currentBall = GameObject.Instantiate(this.ballPrefab);
        this.currentBall.transform.parent = this.transform.parent;
        this.currentBall.transform.position = this.spawnPos;

    }

    private void ClearLevel() {

        this.currentBlocks.Clear();
    }

    private void LoadLevel(Level level)
    {
        this.ClearLevel();
        this.RespawnBall();

        string levelString = level.levelString;

        float xInterval = this.bounds.width / (float)(this.tiles.x - 1);
        float yInterval = this.bounds.height / (float)(this.tiles.y - 1);

        int symbolCounter = 0;
        for(int i = 0; i < levelString.Length; i++)
        {
            int x = symbolCounter % tiles.x;
            int y = symbolCounter / tiles.x;

            char c = levelString[i];
            if(c >= '0' && c <= '9')
            {
                int blockStrength = c - '0';

                var blockInstance = GameObject.Instantiate(this.blockPrefab);
                blockInstance.transform.parent = this.transform;

                var blockComponent = blockInstance.GetComponentInChildren<Block>();
                blockComponent.startStrength = blockStrength;

                float posX = this.bounds.x + x * xInterval;
                float posY = this.bounds.yMax - y * yInterval;

                Vector3 worldPos = new Vector3(posX, posY, 0.0f);

                blockInstance.transform.position = worldPos;

                this.currentBlocks.Add(blockComponent);

                symbolCounter++;
            } else if(c == '-')
            {
                symbolCounter++;
            }
        }
    }

    private void Awake()
    {
        if(this.levels.Count > 0)
        {
            this.LoadLevel(this.levels[this.currentLevel]);
        }
    }

    private bool AllBlocksAreDestroyed()
    {
        for (int i = 0; i < this.currentBlocks.Count; i++)
        {
            if (this.currentBlocks[i] != null)
            {
                return false;
            }
        }
        return true;
    }
    private void Update()
    {
        
        if(AllBlocksAreDestroyed())
        {
            this.currentLevel++;

            if(this.currentLevel < this.levels.Count)
            {
                this.LoadLevel(this.levels[this.currentLevel]);
            }
        }

    }



}
