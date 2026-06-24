using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

    [Header("Progresso")]
    [SerializeField] private Slider progressBar;

    [Header("Boost")]
    [SerializeField] private GameObject boostIndicator; // o objeto que aparece/some
    [SerializeField] private Image      boostFillImage; // estreaks acumuladas (opcional)

    [Header("Streak")]
    [SerializeField, Range(2, 10)] private int streakToBoost = 5;

    private int   _totalPlatforms;
    private int   _currentStreak;
    private bool  _initialized;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_initialized)
        {
            if (PlatformSpawner.Instance == null) return;
            _totalPlatforms = PlatformSpawner.Instance.TotalPlatforms;
            _initialized    = true;
        }

        // Progresso
        PlatformBehavior current = PlatformSpawner.Instance.CurrentPlatform;
        if (current != null && _totalPlatforms > 1)
        {
            float progress = (float)current.PlatformIndex / (_totalPlatforms - 1);
            progressBar.value = Mathf.Clamp01(progress);
        }

        // Fill do streak (mostra quantos acertos acumulou dos N necessários)
        if (boostFillImage != null && !GameManager.Instance.BoostReady)
            boostFillImage.fillAmount = (float)_currentStreak / streakToBoost;
    }

    // Chamado pelo GameManager a cada acerto
    public void UpdateStreak(int streak)
    {
        _currentStreak = streak;
    }

    // Chamado pelo GameManager quando streak completo
    public void ShowBoostReady()
    {
        _currentStreak = 0;
        if (boostIndicator != null) boostIndicator.SetActive(true);
        if (boostFillImage  != null) boostFillImage.fillAmount = 1f;
    }

    // Chamado pelo GameManager quando boost é consumido
    public void HideBoostReady()
    {
        if (boostIndicator != null) boostIndicator.SetActive(false);
        if (boostFillImage  != null) boostFillImage.fillAmount = 0f;
    }
}