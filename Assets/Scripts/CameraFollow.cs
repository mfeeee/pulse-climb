using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -8f);
    [SerializeField, Range(1f, 20f)] private float smoothSpeed = 6f;
    [SerializeField] private bool followOnlyUp = true; // câmera só sobe, nunca desce

    private float _highestY;

    private void Awake()
    {
        _highestY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float targetY = followOnlyUp
            ? Mathf.Max(target.position.y + offset.y, _highestY)
            : target.position.y + offset.y;

        _highestY = targetY;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            targetY,
            target.position.z + offset.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}