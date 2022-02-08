using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Create New Level")]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    private string levelName;

    public string GameSeed = "Default";
    public int CurrentSeed = 0;
    public bool RandomizeSeed = true;

    public int NumberOfPoints;
    public float roadWidth;
    public bool HasCurves = false;
    [Range(0, 100)] public int CurveGenerationDifficulty;
    [Range(2, 10)] public int CubesGenerationDifficulty = 3;
    [Range(5, 20)] public int ObstacleGenerationDifficulty = 5;

}
