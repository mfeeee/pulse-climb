using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "PulseClimb/Difficulty Config")]
public class DifficultyConfig : ScriptableObject
{
    [System.Serializable]
    public struct DifficultyBlock
    {
        [Tooltip("Quantas plataformas neste bloco")]
        public int platformCount;
        [Tooltip("Gap vertical mínimo entre plataformas")]
        public float minVerticalGap;
        [Tooltip("Gap vertical máximo entre plataformas")]
        public float maxVerticalGap;
        [Tooltip("Variação lateral máxima (X)")]
        public float maxHorizontalOffset;
    }

    [Header("Blocos de Dificuldade")]
    public DifficultyBlock[] blocks;

    [Header("Plataforma")]
    public Vector3 platformScale = new Vector3(4f, 0.5f, 3f);
}