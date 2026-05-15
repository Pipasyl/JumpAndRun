using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    public static UIManager Instance => instance;

    private PlayerStatistics statistics;


    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private float fadingTime = 2.0f;
    private bool isFadingInGameOver = false;

    [SerializeField] private Character character;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI coinCounterText;

    private IEnumerator FadeInGameOver()
    {
        this.isFadingInGameOver = true;
        float timer = 0.0f;

        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;
            this.hudCanvasGroup.alpha = 1.0f - percent;
            this.gameOverCanvasGroup.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }

        this.hudCanvasGroup.alpha = 0.0f;
        this.gameOverCanvasGroup.alpha = 1.0f;
        this.gameOverCanvasGroup.interactable = true; // Allows clicking [cite: 449]
        this.gameOverCanvasGroup.blocksRaycasts = true; // Allows the mouse to "hit" the buttons [cite: 451]
    }


    private void Awake()
    {
        instance = this;
        this.statistics = new PlayerStatistics() { coinCounter = 0 };
    }

    private void Update()
    {
        float healthInPercent = this.character.GetCurrentHealth() / this.character.GetMaxHealth();
        this.healthBar.fillAmount = healthInPercent;

        if (healthInPercent <= 0.0f && !this.isFadingInGameOver)
        {
            this.StartCoroutine(this.FadeInGameOver());
        }
    }

    public void CollectCoin()
    {
        this.statistics.coinCounter++;
        string coinText = $"Coins: {this.statistics.coinCounter}";
        this.coinCounterText.text = coinText;
    }

    // TODO: extract into own script
    private class PlayerStatistics
    {
        public int coinCounter = 0;
    }
    public void FadeInHUD()
    {
        StopAllCoroutines();
        hudCanvasGroup.alpha = 1.0f;
        gameOverCanvasGroup.alpha = 0.0f;
        isFadingInGameOver = false;

        // Reset coins too
        statistics.coinCounter = 0;
        coinCounterText.text = "Coins: 0";
    }
    public void RespawnPlayer()
    {
        // Fade out Game Over, fade in HUD
        StopAllCoroutines();
        this.hudCanvasGroup.alpha = 1.0f;
        this.gameOverCanvasGroup.alpha = 0.0f;
        this.isFadingInGameOver = false;

        // Reset coins
        this.statistics.coinCounter = 0;
        this.coinCounterText.text = "Coins: 0";

        // ← NEW: Make all coins reappear
        Coin[] allCoins = FindObjectsOfType<Coin>(true);  // true = include inactive objects
        foreach (Coin coin in allCoins)
        {
            coin.Reappear();
        }

        // Reset character health and position
        this.character.Respawn();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}