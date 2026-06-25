using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectibleItem : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private AudioClip collectSFX;

    public static event System.Action<int> OnCollected;

    private bool _collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        if (!other.CompareTag("Player")) return;

        _collected = true;
        OnCollected?.Invoke(scoreValue);

        if (collectSFX != null)
            AudioSource.PlayClipAtPoint(collectSFX, transform.position);

        gameObject.SetActive(false);
    }
}