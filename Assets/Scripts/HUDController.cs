using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private PlayerController player;

    private void Update()
    {
        if (PlatformSpawner.Instance == null) return;

        PlatformBehavior current = PlatformSpawner.Instance.CurrentPlatform;
        if (current == null) return;

        int total = 20; // deve bater com o totalPlatforms do PlatformSpawner
        float progress = (float)current.PlatformIndex / (total - 1);
        progressBar.value = progress;
    }
}