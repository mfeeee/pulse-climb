using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private LevelData levelData;

    [Header("Beat Window")]
    [SerializeField, Range(0.05f, 0.4f)] private float windowHalfSize = 0.25f;

    [Header("Áudio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField, Range(0f, 1f)] private float stopBeforeEndSeconds = 0.3f; // para N segundos antes do fim

    public event System.Action OnBeat;
    public float CurrentBpm => levelData != null ? levelData.bpm : 120f;

    private float _beatInterval;
    private float _beatTimer;
    private bool  _insideWindow;
    private bool  _ready;
    private bool  _musicStopped;

    private readonly List<PlatformBehavior> _platforms = new List<PlatformBehavior>();

    private void Awake()
    {
        Instance = this;

        if (LevelSelector.Selected != null)
            levelData = LevelSelector.Selected;

        if (levelData == null)
        {
            Debug.LogError("[BeatManager] levelData é null!");
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
            Debug.LogError("[BeatManager] musicSource é null!");
        }
    }

    public void RegisterPlatform(PlatformBehavior pb)   => _platforms.Add(pb);
    public void UnregisterPlatform(PlatformBehavior pb) => _platforms.Remove(pb);

    private void Update()
    {
        if (!_ready || _musicStopped) return;

        _beatTimer += Time.deltaTime;

        _insideWindow = _beatTimer >= (_beatInterval - windowHalfSize) &&
                        _beatTimer <= (_beatInterval + windowHalfSize);

        if (_beatTimer >= _beatInterval)
        {
            _beatTimer -= _beatInterval;
            FireBeat();
        }

        // Verifica fim da música
        if (musicSource != null && levelData.music != null)
        {
            float timeLeft = levelData.music.length - musicSource.time;

            // Para a música N segundos antes do fim (para vitória)
            // GameManager chama StopMusic() antes — esse bloco só atua se a música chegou ao fim sozinha
            if (timeLeft <= 0.05f)
                OnMusicEnd();
        }
    }

    private void FireBeat()
    {
        OnBeat?.Invoke();
        foreach (var pb in _platforms) pb.PulseOnBeat();
    }

    // Chamado pelo GameManager quando o jogador vence — para a música antes do som de vitória
    public void StopMusic(float fadeSeconds = 0.3f)
    {
        if (_musicStopped) return;
        _musicStopped = true;
        StartCoroutine(FadeOut(fadeSeconds));
    }

    private IEnumerator FadeOut(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed     = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // restaura para o próximo play
    }

    private void OnMusicEnd()
    {
        _musicStopped = true;
        musicSource.Stop();
        GameManager.Instance.TriggerGameOver(); // música acabou sem chegar no topo = game over
    }

    public bool IsInsideBeatWindow() => _ready && !_musicStopped && _insideWindow;

    public void PauseMusic()
    {
        if (musicSource != null) musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null) musicSource.UnPause();
    }
}