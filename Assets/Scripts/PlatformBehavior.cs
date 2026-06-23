using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Renderer platformRenderer;
    [SerializeField] private Color normalColor  = Color.white;
    [SerializeField] private Color beatColor    = Color.cyan;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color missColor    = Color.red;
    [SerializeField] private Color finalColor   = Color.yellow;

    public int PlatformIndex { get; private set; }
    private bool _isFinal;
    private MaterialPropertyBlock _propBlock;
    private void OnEnable()  => BeatManager.Instance?.RegisterPlatform(this);
    private void OnDisable() => BeatManager.Instance?.UnregisterPlatform(this);

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

    public void PulseSuccess()
    {
        CancelInvoke(nameof(ResetColor));
        SetColor(successColor);
        Invoke(nameof(ResetColor), 0.3f);
    }

    public void PulseMiss()
    {
        CancelInvoke(nameof(ResetColor));
        SetColor(missColor);
        Invoke(nameof(ResetColor), 0.3f);
    }

    private void ResetColor() => SetColor(_isFinal ? finalColor : normalColor);

    private void SetColor(Color c)
    {
        if (platformRenderer == null) return;
        platformRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", c);
        platformRenderer.SetPropertyBlock(_propBlock);
    }

}