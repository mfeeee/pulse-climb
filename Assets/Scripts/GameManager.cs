using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    [Header("Referências")]
    [SerializeField] private PlayerController player;

    private bool _gameOver;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        player.OnLanded += CheckVictory;
        ShowHUD();
    }

    private void CheckVictory()
    {
        if (PlatformSpawner.Instance.IsFinalPlatform(
                PlatformSpawner.Instance.CurrentPlatform))
            TriggerVictory();
    }

    public void TriggerGameOver()
    {
        if (_gameOver) return;
        _gameOver = true;

        hudPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TriggerVictory()
    {
        hudPanel.SetActive(false);
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowHUD()
    {
        hudPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }
}