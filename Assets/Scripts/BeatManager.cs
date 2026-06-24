using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private LevelData levelData;

    [Header("Beat Window")]
    [SerializeField, Range(0.05f, 0.4f)] private float windowHalfSize = 0.25f;

    [SerializeField] private AudioSource musicSource;

    public event System.Action OnBeat;
    public float CurrentBpm => levelData != null ? levelData.bpm : 120f;

    private float _beatInterval;
    private float _beatTimer;
    private bool  _insideWindow;
    private bool  _ready;

    private readonly List<PlatformBehavior> _platforms = new List<PlatformBehavior>();

    private void Awake()
    {
        Instance = this;

        if (LevelSelector.Selected != null)
            levelData = LevelSelector.Selected;

        if (levelData == null)
        {
            Debug.LogError("[BeatManager] levelData é null! Atribua um LevelData no Inspector.");
            return;
        }

        _beatInterval = 60f / levelData.bpm;
        _ready = true;
    }

    private void Start()
    {
        if (!_ready) return;

        if (musicSource != null)
        {
            musicSource.clip = levelData.music;
            musicSource.Play();
        }
        else
        {
            Debug.LogError("[BeatManager] musicSource é null! Arraste o AudioSource no Inspector.");
        }
    }

    public void RegisterPlatform(PlatformBehavior pb)   => _platforms.Add(pb);
    public void UnregisterPlatform(PlatformBehavior pb) => _platforms.Remove(pb);

    private void Update()
    {
        if (!_ready) return;

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

    public bool IsInsideBeatWindow() => _ready && _insideWindow;
}