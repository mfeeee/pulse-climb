using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private LevelData levelData;

    [Header("Beat Window")]
    [SerializeField, Range(0.05f, 0.4f)] private float windowHalfSize = 0.15f;

    [SerializeField] private AudioSource musicSource;

    public event System.Action OnBeat;

    private float _beatInterval;
    private float _beatTimer;
    private bool  _insideWindow;

    private readonly List<PlatformBehavior> _platforms = new List<PlatformBehavior>();

    private void Awake()
    {
        Instance = this;

        // BPM vem do LevelData — não hardcoded
        _beatInterval = 60f / levelData.bpm;
    }

    private void Start()
    {
        if (musicSource != null)
        {
            musicSource.clip = levelData.music;
            musicSource.Play();
        }
    }

    public void RegisterPlatform(PlatformBehavior pb)   => _platforms.Add(pb);
    public void UnregisterPlatform(PlatformBehavior pb) => _platforms.Remove(pb);

    private void Update()
    {
        _beatTimer += Time.deltaTime;

        _insideWindow = _beatTimer >= (_beatInterval - windowHalfSize) &&
                        _beatTimer <= (_beatInterval + windowHalfSize);

        if (_beatTimer >= _beatInterval)
        {
            _beatTimer -= _beatInterval;
            FireBeat();
        }
    }

    private void FireBeat()
    {
        OnBeat?.Invoke();
        foreach (var pb in _platforms) pb.PulseOnBeat();
    }

    public bool IsInsideBeatWindow() => _insideWindow;
}