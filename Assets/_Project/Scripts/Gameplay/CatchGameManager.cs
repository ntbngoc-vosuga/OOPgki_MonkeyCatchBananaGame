using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CatchGameManager : MonoBehaviour
{
    public static CatchGameManager Instance { get; private set; }

    [Header("UI References - Game Over")]
    [Tooltip("Bảng thua cuộc")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("UI References - Game Win")]
    [Tooltip("Bảng chiến thắng")]
    [SerializeField] private GameObject gameWinPanel;
    [Tooltip("Text hiển thị tổng chuối đạt được khi thắng")]
    [SerializeField] private TextMeshProUGUI winScoreText;

    [Header("UI References - HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Dead Monkey Visual")]
    [SerializeField] private GameObject deadMonkey;

    [Header("Game Settings & State")]
    public int currentBananas = 0;
    public float totalGameTime = 180f; // Tổng thời gian: 3 phút (180s)
    public float timeRemaining = 180f;
    public bool isGameOver = false;

    private GameObject playerInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1f;

        // Đảm bảo ẩn cả 2 bảng khi mới vào game
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
        if (deadMonkey != null) deadMonkey.SetActive(false);
    }

    private void Start()
    {
        playerInstance = GameObject.FindGameObjectWithTag("Player");
        timeRemaining = totalGameTime;
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void Update()
    {
        if (isGameOver) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                UpdateTimerUI();
                GameWin();
            }
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Banana: " + currentBananas;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60F);
            int seconds = Mathf.FloorToInt(timeRemaining % 60F);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        currentBananas += amount;
        UpdateScoreUI();
    }

    public void SubtractScore(int amount)
    {
        if (isGameOver) return;

        currentBananas -= amount;

        if (currentBananas <= 0)
        {
            currentBananas = 0;
            UpdateScoreUI();

            Vector3 pos = playerInstance != null ? playerInstance.transform.position : Vector3.zero;
            TriggerGameOver(pos, playerInstance, "OUT OF BANANAS! GAME OVER!");
            return;
        }

        UpdateScoreUI();
    }

    public void GameOver(Vector3 playerPosition, GameObject playerObj)
    {
        TriggerGameOver(playerPosition, playerObj, "OOPS! GAME OVER!");
    }

    private void TriggerGameOver(Vector3 playerPosition, GameObject playerObj, string reasonMessage)
    {
        if (isGameOver) return;
        isGameOver = true;

        if (playerObj != null) playerObj.SetActive(false);
        if (deadMonkey != null)
        {
            deadMonkey.transform.position = playerPosition;
            deadMonkey.SetActive(true);
        }

        if (gameOverText != null) gameOverText.text = reasonMessage;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // Xử lý khi sống sót hết 3 phút
    private void GameWin()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        // Cập nhật số chuối đạt được vào bảng thắng
        if (winScoreText != null)
        {
            winScoreText.text = "Total banana: " + currentBananas;
        }

        // Bật bảng thắng
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }
    }

    // Nút Chơi Lại dùng chung cho cả Win và Lose
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hàm dùng cho nút Quit trong màn chơi
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Khôi phục thời gian bình thường phòng khi game đang pause/thua
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}