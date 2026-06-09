using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField, Range(-5f, -20f)] private float offsetBelowCamera = -8f;

    private Transform _cameraRig;

    private void Awake()
    {
        _cameraRig = Camera.main.transform.parent;
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.y = _cameraRig.position.y + offsetBelowCamera;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.TriggerGameOver();
    }
}