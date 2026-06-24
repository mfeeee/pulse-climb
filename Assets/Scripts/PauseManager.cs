using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject hudPanel;

    private bool _isPaused;

    private void Awake() => Instance = this;

    // Conecte no botão de pause do HUD
    public void TogglePause()
    {
        if (_isPaused) Resume();
        else           Pause();
    }

    public void Resume()
    {
        _isPaused            = false;
        Time.timeScale       = 1f;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
        BeatManager.Instance.ResumeMusic();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void Pause()
    {
        _isPaused            = true;
        Time.timeScale       = 0f;
        pausePanel.SetActive(true);
        hudPanel.SetActive(false);
        BeatManager.Instance.PauseMusic();
    }
}