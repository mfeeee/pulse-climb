using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Painéis")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private int levelSelectSceneIndex = 1;

    private void Start()
    {
        Time.timeScale = 1f;
        mainPanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        // índice 1 = cena do jogo no Build Settings
        SceneManager.LoadScene(1);
    }

    public void ShowInstructions()
    {
        mainPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(true);
        mainPanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene(levelSelectSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}