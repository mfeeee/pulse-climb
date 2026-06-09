using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField, Range(3f, 12f)] private float jumpForce = 6f;
    [SerializeField, Range(5f, 20f)] private float chargedJumpForce = 12f;
    [SerializeField, Range(0.2f, 1f)] private float holdThreshold = 0.4f; // tempo mínimo para considerar "segurado"

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(0.05f, 0.3f)] private float groundCheckDistance = 0.1f;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private bool _isGrounded;
    private bool _isHolding;
    private float _holdTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();

        _rb.freezeRotation = true;
    }

    private void Update()
    {
        CheckGround();
        HandleHoldTimer();
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position;
        float radius = _col.radius * 0.9f;
        // SphereCast evita falsos negativos nas bordas da cápsula
        _isGrounded = Physics.SphereCast(origin, radius, Vector3.down,
            out _, _col.bounds.extents.y + groundCheckDistance, groundLayer);
    }

    private void HandleHoldTimer()
    {
        if (_isHolding)
            _holdTimer += Time.deltaTime;
    }

    // Chamado pelo Input System (Action: Jump, started)
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

            if (!_isGrounded) return;

            float force = _holdTimer >= holdThreshold ? chargedJumpForce : jumpForce;
            ExecuteJump(force);
        }
    }

    private void ExecuteJump(float force)
    {
        // Zera velocidade vertical antes do impulso para consistência
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}