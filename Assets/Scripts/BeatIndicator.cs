using UnityEngine;
using UnityEngine.UI;

public class BeatIndicator : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image glowImage;

    [Header("Cores")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f,  0.4f);
    [SerializeField] private Color windowColor = new Color(0f, 1f, 0.8f, 0.9f);
    [SerializeField] private Color beatColor   = new Color(1f, 1f, 0f,   1f);

    private float _beatInterval;
    private float _timer;
    private bool  _pulsing;
    private bool  _initialized;

    private void Update()
    {
        if (!_initialized)
        {
            // Aguarda BeatManager existir E estar pronto
            if (BeatManager.Instance == null)           return;
            if (BeatManager.Instance.CurrentBpm <= 0f)  return;

            _beatInterval = 60f / BeatManager.Instance.CurrentBpm;
            BeatManager.Instance.OnBeat += OnBeat;
            _initialized = true;
            return; // próximo frame já roda normal
        }

        _timer += Time.deltaTime;
        if (_timer > _beatInterval) _timer -= _beatInterval;

        fillImage.fillAmount = _timer / _beatInterval;
        fillImage.color = BeatManager.Instance.IsInsideBeatWindow()
            ? windowColor
            : normalColor;

        if (_pulsing)
        {
            Color c = glowImage.color;
            c.a = Mathf.MoveTowards(c.a, 0f, Time.deltaTime * 6f);
            glowImage.color = c;
            if (c.a <= 0f) _pulsing = false;
        }
    }

    private void OnBeat()
    {
        _timer = 0f;
        Color c = beatColor;
        c.a = 1f;
        glowImage.color = c;
        _pulsing = true;
    }

    private void OnDestroy()
    {
        if (BeatManager.Instance != null)
            BeatManager.Instance.OnBeat -= OnBeat;
    }
}