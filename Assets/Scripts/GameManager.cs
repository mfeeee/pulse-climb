using System.Collections;
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
    [SerializeField, Range(0f, 1f)] private float stopBeforeVictorySound = 0.3f;

    [Header("Level")]
    [SerializeField] private LevelData levelData;

    [Header("Boost")]
    [SerializeField, Range(2, 10)] private int streakToBoost = 5;

    private bool _gameOver;
    private int  _consecutiveErrors;
    public bool BoostReady { get; private set; }
    private int _streak;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        if (LevelSelector.Selected != null)
            levelData = LevelSelector.Selected;
    }



    private void Start()
    {
        errorsToGoBack = levelData.errorsToGoBack;
        player.OnLanded += CheckVictory;
        player.OnBeatSuccess += OnSuccess;
        player.OnBeatMiss += OnMiss;
        ShowHUD();
    }

    private void OnSuccess()
    {
        _consecutiveErrors = 0;
        _streak++;
        HUDController.Instance?.UpdateStreak(_streak); // atualiza o fill parcial

        if (_streak >= streakToBoost && !BoostReady)
        {
            BoostReady = true;
            _streak    = 0;
            HUDController.Instance?.ShowBoostReady();
        }
    }

    private void OnMiss()
    {
        _streak = 0; // erra — perde o streak
        _consecutiveErrors++;

        if (_consecutiveErrors >= errorsToGoBack)
        {
            _consecutiveErrors = 0;
            player.ForceMoveBack();
        }
    }

    public void ConsumeBoost()
    {
        BoostReady = false;
        HUDController.Instance?.HideBoostReady();
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
        // Para a música com fade, depois toca o som de vitória
        BeatManager.Instance.StopMusic(stopBeforeVictorySound);
        StartCoroutine(VictorySequence());
    }


    private IEnumerator VictorySequence()
    {
        yield return new WaitForSecondsRealtime(stopBeforeVictorySound);
        AudioManager.Instance.PlaySuccess();
        hudPanel.SetActive(false);
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Volta ao menu principal
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // cena 0 = MainMenu
    }

    private void ShowHUD()
    {
        hudPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }
}