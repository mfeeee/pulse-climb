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

    [Header("Regras")]
    [SerializeField, Range(1, 5)] private int errorsToGoBack = 2;

    private bool _gameOver;
    private int  _consecutiveErrors;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        player.OnLanded      += CheckVictory;
        player.OnBeatSuccess += OnSuccess;
        player.OnBeatMiss    += OnMiss;
        ShowHUD();
    }

    private void OnSuccess() => _consecutiveErrors = 0;

    private void OnMiss()
    {
        _consecutiveErrors++;

        if (_consecutiveErrors >= errorsToGoBack)
        {
            _consecutiveErrors = 0;
            player.ForceMoveBack(); // ele decide se recua ou cai
        }
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
        AudioManager.Instance.PlaySuccess();
        hudPanel.SetActive(false);
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void ShowHUD()
    {
        hudPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }
}