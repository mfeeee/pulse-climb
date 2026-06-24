using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField, Range(0.2f, 1f)]   private float jumpDuration        = 0.35f;
    [SerializeField, Range(0.1f, 0.5f)] private float chargedJumpDuration  = 0.2f;
    [SerializeField, Range(0.2f, 1f)]   private float holdThreshold        = 0.4f;

    [Header("Fall Settings")]
    [SerializeField, Range(1f, 20f)] private float fallAcceleration = 8f;

    private Rigidbody       _rb;
    private CapsuleCollider _col;
    private bool  _isJumping;
    private bool  _isHolding;
    private bool  _isFalling;
    private float _holdTimer;

    public bool IsGrounded { get; private set; } = true;

    public event System.Action OnLanded;
    public event System.Action OnBeatSuccess;
    public event System.Action OnBeatMiss;

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

    private bool _pressedInWindow;

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isHolding    = true;
            _holdTimer    = 0f;

            // Verifica a janela NO MOMENTO do press — não do release
            _pressedInWindow = BeatManager.Instance.IsInsideBeatWindow();

            if (!_pressedInWindow)
            {
                PlatformSpawner.Instance.CurrentPlatform?.PulseMiss();
                AudioManager.Instance.PlayMiss();
                OnBeatMiss?.Invoke();
            }
        }

        if (ctx.canceled)
        {
            _isHolding = false;
            if (_isJumping || _isFalling) return;
            if (!IsGrounded)              return;
            if (!_pressedInWindow)        return; // press foi fora da janela — ignora

            TryJump(_holdTimer >= holdThreshold);
            _pressedInWindow = false;
        }
    }

    private void TryJump(bool charged)
    {
        PlatformBehavior target = charged
            ? PlatformSpawner.Instance.GetPlatformAhead(2)
            : PlatformSpawner.Instance.NextPlatform;

        if (target == null) return;

        PlatformSpawner.Instance.CurrentPlatform?.PulseSuccess();
        AudioManager.Instance.PlayJump();
        OnBeatSuccess?.Invoke();

        float duration      = charged ? chargedJumpDuration : jumpDuration;
        Vector3 destination = target.transform.position + Vector3.up * 0.65f;
        StartCoroutine(SnapJump(destination, duration, target));
    }

    // Chamado pelo GameManager após N erros consecutivos
    public void ForceMoveBack()
    {
        if (_isJumping || _isFalling) return;

        PlatformBehavior target = PlatformSpawner.Instance.PreviousPlatform;

        if (target == null)
        {
            // Não há plataforma anterior — cai no vazio
            StartCoroutine(FallOff());
            return;
        }

        Vector3 destination = target.transform.position + Vector3.up * 0.65f;
        StartCoroutine(SnapJump(destination, jumpDuration, target));
    }

    // Queda simulada: acelera para baixo até a DeathZone detectar
    private IEnumerator FallOff()
    {
        _isFalling   = true;
        IsGrounded   = false;
        _col.enabled = false; // evita colisão com plataformas durante a queda

        float speed = 0f;

        while (_isFalling) // DeathZone chama TriggerGameOver e para o jogo via timeScale = 0
        {
            speed += fallAcceleration * Time.deltaTime;
            transform.position += Vector3.down * speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SnapJump(Vector3 destination, float duration, PlatformBehavior target)
    {
        _isJumping   = true;
        IsGrounded   = false;
        _col.enabled = false;

        Vector3 start   = transform.position;
        float   elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t  = elapsed / duration;
            Vector3 pos = Vector3.Lerp(start, destination, t);
            pos.y = Mathf.Lerp(start.y, destination.y, t) + Mathf.Sin(t * Mathf.PI) * 1.5f;
            transform.position = pos;
            yield return null;
        }

        transform.position = destination;
        _col.enabled = true;
        _isJumping   = false;
        IsGrounded   = true;

        PlatformSpawner.Instance.OnPlayerLanded(target);
        OnLanded?.Invoke();
    }
}