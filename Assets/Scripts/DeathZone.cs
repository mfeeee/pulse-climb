using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField, Range(-5f, -20f)] private float offsetBelowCamera = -8f;

    private Transform _cameraRig;
    private bool _active = false;

    private void Awake()
    {
        _cameraRig = Camera.main.transform.parent;
        // Aguarda 1 segundo antes de ativar a death zone
        Invoke(nameof(Activate), 1f);
    }

    private void Activate() => _active = true;

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.y = _cameraRig.position.y + offsetBelowCamera;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_active) return;
        if (other.CompareTag("Player"))
            GameManager.Instance.TriggerGameOver();
    }
}