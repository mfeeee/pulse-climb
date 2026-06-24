using UnityEngine;

public static class LevelSelector
{
    public static LevelData Selected { get; private set; }

    public static void Select(LevelData data) => Selected = data;
}
