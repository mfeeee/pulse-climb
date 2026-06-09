using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Renderer platformRenderer;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color beatColor = Color.cyan;
    [SerializeField] private Color finalColor = Color.yellow;

    public int PlatformIndex { get; private set; }
    private bool _isFinal;
    private MaterialPropertyBlock _propBlock;

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
    }

    public void Init(int index, int total)
    {
        PlatformIndex = index;
        _isFinal = (index >= total - 1);
        SetColor(_isFinal ? finalColor : normalColor);
    }

    public void PulseOnBeat()
    {
        SetColor(beatColor);
        Invoke(nameof(ResetColor), 0.15f);
    }

    private void ResetColor() => SetColor(_isFinal ? finalColor : normalColor);

    private void SetColor(Color c)
    {
        if (platformRenderer == null) return;
        platformRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_BaseColor", c);
        platformRenderer.SetPropertyBlock(_propBlock);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            PlatformSpawner.Instance.OnPlayerLanded(this);
    }
}