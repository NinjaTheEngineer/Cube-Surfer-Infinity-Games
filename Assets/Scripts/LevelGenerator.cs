using PathCreation;
using PathCreation.Examples;
using System.Collections;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject wallPrefab;
    public GameObject endArcPrefab;
    public GameObject[] possibleObstacles;

    private int lastPlaceablePoint;
    private LevelSO currentLevel;
    private LevelSO[] Levels;
    private PathCreator pathCreator;
    private RoadMeshCreator roadMeshCreator;
    private Vector3[] listOfPoints;
    private Vector3 rotateDirection = new Vector3(1, 0, 0);
    private Vector3 initialDirection = new Vector3(0, 0, 1);

    [SerializeField]
    private GameEvent levelLoaded;
    private void Awake() //Fetch gameObject components
    {
        roadMeshCreator = GetComponent<RoadMeshCreator>();
        pathCreator = GetComponent<PathCreator>();
    }
    public void SetUpLevels(LevelSO[] levels, int numberOfLevelsCompleted) //SetUp possible levels and SetUp level
    {
        Levels = levels;
        SetUpLevelConfig(numberOfLevelsCompleted);
    }

    private void SetUpLevelConfig(int numberOfLevelsCompleted)
    {
        //Set seed or generate a random one for the selected level
        if (Levels.Length == 0)
        {
            Debug.LogWarning("LevelGenerator doesn't have Levels assigned.");
            return;
        }
        currentLevel = numberOfLevelsCompleted > Levels.Length ?
                    Levels[numberOfLevelsCompleted - 1] : Levels[numberOfLevelsCompleted];

        if (currentLevel.RandomizeSeed || currentLevel.CurrentSeed.ToString() == null)
        {
            currentLevel.CurrentSeed = Random.state.GetHashCode();
        }
        Random.InitState(currentLevel.CurrentSeed);
        StartCoroutine(Initialize());
    }
    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1f);
        if (currentLevel == null)
        {
            Debug.LogWarning("LevelGenerator doesn't have a Level configuration assigned.");
            yield return null;
        }
        Vector3 newPoint = Vector3.zero;
        listOfPoints = new Vector3[currentLevel.NumberOfPoints];

        lastPlaceablePoint = Mathf.RoundToInt(currentLevel.NumberOfPoints * 0.9f);
        for (int i = 0; i < currentLevel.NumberOfPoints; i++)
        {
            newPoint += initialDirection;
            float randomRotation = Random.Range(0, 2) * 2 - 1;
            if (currentLevel.HasCurves && ShouldGenerateCurve() && i < lastPlaceablePoint)
                initialDirection += (rotateDirection * randomRotation);

            listOfPoints[i] = newPoint;
        }

        pathCreator.bezierPath = GenerateBezierPath(listOfPoints, false, 1);
        pathCreator.bezierPath.GlobalNormalsAngle = 90f;
        roadMeshCreator.TriggerUpdate();
        roadMeshCreator.roadWidth = currentLevel.roadWidth;
        StartCoroutine(SpawnObstaclesAndCubes());
        StartCoroutine(SpawnEndArc());
        StartCoroutine(SpawnWall());
    }
    private IEnumerator SpawnEndArc()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForEndOfFrame();
            int pos = (lastPlaceablePoint * 3) + i + 1;
            Transform endArc = Instantiate(endArcPrefab, pathCreator.bezierPath.GetPoint(pos),
                                            Quaternion.identity).GetComponent<Transform>();
            endArc.LookAt(pathCreator.bezierPath.GetPoint(pos + 1));
            endArc.rotation *= Quaternion.Euler(0, 90, 0);
        }
        
    }
    private IEnumerator SpawnObstaclesAndCubes()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 5; i < lastPlaceablePoint; i++)
        {
            if (ShouldGenerateCube())
            {
                Transform cube = Instantiate(cubePrefab, pathCreator.bezierPath.GetPoint(i * 3), Quaternion.identity).GetComponent<Transform>();
                cube.LookAt(pathCreator.bezierPath.GetPoint(i * 3 + 1));
            }
            else if (ShouldGenerateObstacle())
            {
                int randomObstacle = Random.Range(0, possibleObstacles.Length);
                Transform obstacle = Instantiate(possibleObstacles[randomObstacle], pathCreator.bezierPath.GetPoint(i * 3), Quaternion.identity).GetComponent<Transform>();
                obstacle.LookAt(pathCreator.bezierPath.GetPoint(i * 3 + 1));
            }
        }
    }
    private IEnumerator SpawnWall()
    {
        for (int i = 0; i < pathCreator.bezierPath.NumPoints - 2; i += 1)
        {
            yield return new WaitForEndOfFrame();

            Transform wall1 = Instantiate(wallPrefab, pathCreator.bezierPath.GetPoint(i), Quaternion.identity).GetComponent<Transform>();
            wall1.LookAt(pathCreator.bezierPath.GetPoint(i + 1));
            wall1.position += wall1.transform.right * (currentLevel.roadWidth + 0.2f);

            Transform wall2 = Instantiate(wallPrefab, pathCreator.bezierPath.GetPoint(i), Quaternion.identity).GetComponent<Transform>();
            wall2.LookAt(pathCreator.bezierPath.GetPoint(i + 1));
            wall2.position += wall1.transform.right * -(currentLevel.roadWidth + 0.2f);
        }
        levelLoaded.Raise();
    }
    private bool ShouldGenerateCurve()
    {
        return Random.Range(0, currentLevel.CurveGenerationDifficulty) == 1;
    }
    private bool ShouldGenerateObstacle()
    {
        return Random.Range(0, currentLevel.ObstacleGenerationDifficulty) == 1;
    }
    private bool ShouldGenerateCube()
    {
        return Random.Range(0, currentLevel.CubesGenerationDifficulty) == 1;
    }
    BezierPath GenerateBezierPath(Vector3[] points, bool closedPath, float vertexSpacing)
    {
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xyz);

        return bezierPath;
    }

    public PathCreator GetPathCreator()
    {
        return pathCreator;
    }
    public int GetEndPoint()
    {
        return lastPlaceablePoint;
    }
}
