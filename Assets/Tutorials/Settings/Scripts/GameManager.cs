using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Make sure TextMeshPro is imported

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public float gameDuration = 90f;
    public int maxCastleHealth = 100;

    [Header("UI References")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countdownText;
    public GameObject pausePanel;
    public GameObject endGamePanel;
    public TextMeshProUGUI endTitleText;
    public TextMeshProUGUI statsText;

    [Header("Turret Reference (Bonus)")]
    public TurretController turret;

    // Runtime State
    private int currentHealth;
    private float timeRemaining;
    private bool isGameActive = false;
    private bool isPaused = false;

    // Player Stats
    private int enemiesDefeated = 0;
    private int totalDamageTaken = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        currentHealth = maxCastleHealth;
        timeRemaining = gameDuration;
        enemiesDefeated = 0;
        totalDamageTaken = 0;

        pausePanel.SetActive(false);
        endGamePanel.SetActive(false);
        UpdateUI();

        StartCoroutine(StartCountdownRoutine());
    }

    IEnumerator StartCountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);
        string[] steps = { "3", "2", "1", "GAME START!" };

        foreach (string step in steps)
        {
            countdownText.text = step;
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);
        isGameActive = true;
    }

    void Update()
    {
        if (!isGameActive || isPaused) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame(true); // Victory
        }
        UpdateUI();
    }

    public bool IsGameRunning() => isGameActive && !isPaused;

    public void RegisterEnemyDefeated()
    {
        enemiesDefeated++;
    }

    public void TakeCastleDamage(int damage)
    {
        if (!isGameActive) return;

        currentHealth -= damage;
        totalDamageTaken += damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateUI();

        if (currentHealth <= 0)
        {
            EndGame(false); // Defeat
        }
    }

    void UpdateUI()
    {
        healthText.text = "Castle HP: " + currentHealth + " / " + maxCastleHealth;
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining) + "s";
    }

    public void TogglePause()
    {
        if (!isGameActive) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void EndGame(bool isVictory)
    {
        isGameActive = false;
        Time.timeScale = 0f;

        // Destroy all remaining enemies
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }

        endGamePanel.SetActive(true);
        endTitleText.text = isVictory ? "VICTORY!" : "GAME OVER";
        endTitleText.color = isVictory ? Color.green : Color.red;

        float timeSurvived = gameDuration - timeRemaining;
        statsText.text = $"Time Survived: {timeSurvived:F1}s\n" +
                         $"Enemies Defeated: {enemiesDefeated}\n" +
                         $"Remaining Castle HP: {currentHealth}\n" +
                         $"Total Damage Taken: {totalDamageTaken}";
    }
}