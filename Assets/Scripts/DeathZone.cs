using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    // O DeathZone segue a câmera e mata o player se ele ficar abaixo dela
    [SerializeField, Range(-5f, -20f)] private float offsetBelowCamera = -8f;

    private Transform _cameraRig;

    private void Awake()
    {
        _cameraRig = Camera.main.transform.parent; // pega o CameraRig
    }

    private void Update()
    {
        // Move o DeathZone para sempre ficar abaixo da câmera
        Vector3 pos = transform.position;
        pos.y = _cameraRig.position.y + offsetBelowCamera;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}