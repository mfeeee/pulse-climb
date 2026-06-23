using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField, Range(-20f, -5f)] private float offsetBelowCamera = -8f;

    private Transform _cameraTransform;
    private Transform _playerTransform;
    private bool _active = false;

    private void Awake()
    {
        // Usa a câmera diretamente — sem depender de um pai "CameraRig"
        _cameraTransform = Camera.main.transform;
        Invoke(nameof(Activate), 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawCube(transform.position, new Vector3(20f, 0.1f, 20f));
    }

    private void Start()
    {
        // Cache do player — único FindWithTag, só no Start
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _playerTransform = playerObj.transform;
        else
            Debug.LogError("[DeathZone] Nenhum objeto com tag 'Player' encontrado!");
    }

    private void Activate() => _active = true;

    private void Update()
    {
        if (_cameraTransform == null) return;

        // Segue a câmera
        Vector3 pos = transform.position;
        pos.y = _cameraTransform.position.y + offsetBelowCamera;
        transform.position = pos;

        // Verificação por posição Y — imune ao CapsuleCollider desabilitado durante o SnapJump
        if (_active && _playerTransform != null &&
            _playerTransform.position.y < transform.position.y)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}