using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField, Range(0.2f, 1f)] private float jumpDuration = 0.35f;
    [SerializeField, Range(0.1f, 0.5f)] private float chargedJumpDuration = 0.2f;
    [SerializeField, Range(0.2f, 1f)] private float holdThreshold = 0.4f;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private bool _isJumping;
    private bool _isHolding;
    private float _holdTimer;

    public bool IsGrounded { get; private set; } = true;
    public event System.Action OnLanded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _rb.freezeRotation = true;
        _rb.useGravity = false;
    }

    private void Update()
    {
        if (_isHolding) _holdTimer += Time.deltaTime;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isHolding = true;
            _holdTimer = 0f;
        }

        if (ctx.canceled)
        {
            _isHolding = false;
            if (_isJumping) return;

            if (!IsGrounded) return;

            // Tenta pular — se fora do beat, feedback de erro
            if (!BeatManager.Instance.IsInsideBeatWindow())
            {
                PlatformSpawner.Instance.CurrentPlatform?.PulseMiss();
                return;
            }

            bool charged = _holdTimer >= holdThreshold;
            TryJump(charged);
        }
    }

    private void TryJump(bool charged)
    {
        PlatformBehavior target = charged
            ? PlatformSpawner.Instance.GetPlatformAhead(2)  // boost: pula 2
            : PlatformSpawner.Instance.NextPlatform;         // normal: pula 1

        if (target == null) return;

        // Feedback de acerto na plataforma atual
        PlatformSpawner.Instance.CurrentPlatform?.PulseSuccess();

        float duration = charged ? chargedJumpDuration : jumpDuration;
        Vector3 destination = target.transform.position + Vector3.up * 0.65f;
        StartCoroutine(SnapJump(destination, duration, target));
    }

    private IEnumerator SnapJump(Vector3 destination, float duration, PlatformBehavior target)
    {
        _isJumping = true;
        IsGrounded = false;
        _col.enabled = false;

        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Vector3 pos = Vector3.Lerp(start, destination, t);
            pos.y = Mathf.Lerp(start.y, destination.y, t) + Mathf.Sin(t * Mathf.PI) * 1.5f;
            transform.position = pos;
            yield return null;
        }

        transform.position = destination;
        _col.enabled = true;
        _isJumping = false;
        IsGrounded = true;

        PlatformSpawner.Instance.OnPlayerLanded(target);
        OnLanded?.Invoke();
    }
}