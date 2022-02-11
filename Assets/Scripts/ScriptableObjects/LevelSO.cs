using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Create New Level")]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    private string levelName;

    public string GameSeed = "Default";
    public int CurrentSeed = 0;
    public bool RandomizeSeed = true; //If should generate a random seed or no

    public int NumberOfPoints; //Number of points in the level road, could be its length
    public float roadWidth; //Road width 
    public bool HasCurves = false; //If there is curves or no
    [Range(0, 100)] public int CurveGenerationDifficulty; //Difficulty in generating the curves, the higher the least probable is to generate a curve
    [Range(2, 5)] public int CheeseGenerationDifficulty = 2;//Difficulty in generating the cheese, the higher the least probable is to generate a cheese
    [Range(3, 10)] public int CubesGenerationDifficulty = 3;//Difficulty in generating the cube, the higher the least probable is to generate a cube
    [Range(5, 20)] public int ObstacleGenerationDifficulty = 5;//Difficulty in generating the obstacle, the higher the least probable is to generate a obstacle

}
