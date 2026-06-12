using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private int totalPlatforms = 20;

    private void Update()
    {
        if (PlatformSpawner.Instance == null) return;

        PlatformBehavior current = PlatformSpawner.Instance.CurrentPlatform;
        if (current == null) return;

        float progress = (float)current.PlatformIndex / (totalPlatforms - 1);
        progressBar.value = Mathf.Clamp01(progress);
    }
}