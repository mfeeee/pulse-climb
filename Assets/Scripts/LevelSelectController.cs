using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private LevelData easyData;
    [SerializeField] private LevelData mediumData;
    [SerializeField] private LevelData hardData;

    [Header("Índices das Cenas")]
    [SerializeField] private int easySceneIndex   = 2;
    [SerializeField] private int mediumSceneIndex = 3;
    [SerializeField] private int hardSceneIndex   = 4;

    // Conecte esses métodos nos botões via OnClick() no Inspector
    public void SelectEasy()   => LoadLevel(easyData,   easySceneIndex);
    public void SelectMedium() => LoadLevel(mediumData, mediumSceneIndex);
    public void SelectHard()   => LoadLevel(hardData,   hardSceneIndex);

    private void LoadLevel(LevelData data, int sceneIndex)
    {
        LevelSelector.Select(data);
        SceneManager.LoadScene(sceneIndex);
    }
}