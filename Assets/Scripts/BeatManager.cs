using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [Header("Ritmo")]
    [SerializeField, Range(60f, 180f)] private float bpm = 120f;
    [SerializeField, Range(0.05f, 0.5f)] private float beatWindowSize = 0.2f;

    [Header("Referências")]
    [SerializeField] private AudioSource musicSource;

    public event System.Action OnBeat;

    private float _beatInterval;
    private float _beatTimer;
    private bool  _insideWindow;

    // Platforms se auto-registram — sem FindObjectsByType a cada beat
    private readonly List<PlatformBehavior> _platforms = new List<PlatformBehavior>();

    private void Awake()
    {
        Instance = this;
        _beatInterval = 60f / bpm;
    }

    private void Start()
    {
        if (musicSource != null) musicSource.Play();
    }

    public void RegisterPlatform(PlatformBehavior pb)   => _platforms.Add(pb);
    public void UnregisterPlatform(PlatformBehavior pb) => _platforms.Remove(pb);

    private void Update()
    {
        _beatTimer += Time.deltaTime;

        if (_beatTimer >= _beatInterval)
        {
            _beatTimer -= _beatInterval;
            FireBeat();
        }

        _insideWindow = _beatTimer <= beatWindowSize;
    }

    private void FireBeat()
    {
        OnBeat?.Invoke();
        for (int i = 0; i < _platforms.Count; i++)
            _platforms[i].PulseOnBeat();
    }

    public bool IsInsideBeatWindow() => _insideWindow;
}