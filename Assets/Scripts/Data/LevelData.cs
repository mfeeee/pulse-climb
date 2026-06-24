using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "PulseClimb/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Música")]
    public AudioClip music;
    [Range(60f, 200f)] public float bpm = 120f;

    [Header("Dificuldade")]
    [Tooltip("Quantos beats entre cada plataforma. Menor = mais difícil.")]
    [Range(0.5f, 4f)] public float beatsPerPlatform = 2f;

    [Tooltip("Erros consecutivos para recuar uma plataforma.")]
    [Range(1, 5)] public int errorsToGoBack = 2;

    [Tooltip("Espaçamento vertical entre plataformas.")]
    [Range(2f, 6f)] public float verticalSpacing = 3f;

    [Tooltip("Tamanho do pool de plataformas reutilizáveis.")]
    [Range(10, 30)] public int poolSize = 15;

    // Calculado a partir da duração da música e do BPM — lido em runtime
    public int TotalPlatforms
    {
        get
        {
            if (music == null) return 50; // fallback seguro
            float totalBeats = (music.length / 60f) * bpm;
            return Mathf.CeilToInt(totalBeats / beatsPerPlatform);
        }
    }
}