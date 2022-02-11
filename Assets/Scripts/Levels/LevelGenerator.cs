using PathCreation;
using PathCreation.Examples;
using System.Collections;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject wallPrefab;
    public GameObject cheesePrefab;
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
    public void SetUpLevels(LevelSO[] levels, int currentLevel) //SetUp possible levels and SetUp level
    {
        Levels = levels;
        SwitchLevel(currentLevel);
    }
    public void SwitchLevel(int currentLevel)
    {
        SetUpLevelConfig(currentLevel);
    }

    private void SetUpLevelConfig(int level)
    {
        //Set seed or generate a random one for the selected level
        if (Levels.Length == 0)
        {
            Debug.LogWarning("LevelGenerator doesn't have Levels assigned.");
            return;
        }
        currentLevel = Levels[level];

        if (currentLevel.RandomizeSeed || currentLevel.CurrentSeed.ToString() == null) //If there's no seed, add one
        {
            currentLevel.CurrentSeed = Random.state.GetHashCode();
        }
        Random.InitState(currentLevel.CurrentSeed);
        StartCoroutine(Initialize());
    }
    private IEnumerator Initialize() //Start the level generation
    {
        yield return new WaitForSeconds(1f);
        if (currentLevel == null)
        {
            Debug.LogWarning("LevelGenerator doesn't have a Level configuration assigned.");
            yield return null;
        }
        Vector3 newPoint = Vector3.zero;
        listOfPoints = new Vector3[currentLevel.NumberOfPoints];

        lastPlaceablePoint = Mathf.RoundToInt(currentLevel.NumberOfPoints * 0.9f); //Get a 90% position of the level for the game, the rest will have nothing
        for (int i = 0; i < currentLevel.NumberOfPoints; i++)
        {
            newPoint += initialDirection;
            float randomRotation = Random.Range(0, 2) * 2 - 1;
            if (currentLevel.HasCurves && ShouldGenerateCurve() && i < lastPlaceablePoint) //Check if should generate curves and does so
                initialDirection += (rotateDirection * randomRotation);

            listOfPoints[i] = newPoint;
        }

        pathCreator.bezierPath = GenerateBezierPath(listOfPoints, false, 1); //Generates the complete level path
        pathCreator.bezierPath.GlobalNormalsAngle = 90f; //Sets the paths normals to 90 degrees
        roadMeshCreator.roadWidth = currentLevel.roadWidth; //Sets the road width with the current level parameters
        roadMeshCreator.TriggerUpdate(); //Updates the road visual
        StartCoroutine(SpawnPrefabs()); //Spawn prefabs
        StartCoroutine(SpawnEndArc()); //Spawn end arc
        StartCoroutine(SpawnWall()); //Spawn edge wall
    }
    private IEnumerator SpawnEndArc() //Spawn the end arc for the level
    {
        for (int i = 0; i < 5; i++) //Spawn 5 arcs
        {
            yield return new WaitForEndOfFrame();
            int pos = (lastPlaceablePoint * 3) + i + 1; //Starting at the last placeable position
            Transform endArc = Instantiate(endArcPrefab, pathCreator.bezierPath.GetPoint(pos),
                                            Quaternion.identity, transform).GetComponent<Transform>();
            endArc.LookAt(pathCreator.bezierPath.GetPoint(pos + 1)); //Straigth them
            endArc.rotation *= Quaternion.Euler(0, 90, 0);  //All orient them so they are in the perfect position
        }
        
    }
    private IEnumerator SpawnPrefabs() //Spawns all prefabs, cubes, obstacles and cheese
    {
        yield return new WaitForEndOfFrame();
        for (int i = 5; i < lastPlaceablePoint; i++)
        {
            //For evey if, if should spawn the prefab, spawn it at the current position and straight them to the path
            if (ShouldGenerateCube()) 
            {
                Transform cube = Instantiate(cubePrefab, pathCreator.bezierPath.GetPoint(i * 3), Quaternion.identity, transform).GetComponent<Transform>();
                cube.LookAt(pathCreator.bezierPath.GetPoint(i * 3 + 1));
            }
            else if (ShouldGenerateObstacle())
            {
                int randomObstacle = Random.Range(0, possibleObstacles.Length);
                Transform obstacle = Instantiate(possibleObstacles[randomObstacle], pathCreator.bezierPath.GetPoint(i * 3), Quaternion.identity, transform).GetComponent<Transform>();
                obstacle.LookAt(pathCreator.bezierPath.GetPoint(i * 3 + 1));
            }else if (ShouldGenerateCheese())
            {
                Transform cheese = Instantiate(cheesePrefab, pathCreator.bezierPath.GetPoint(i * 3), Quaternion.identity, transform).GetComponent<Transform>();
                cheese.LookAt(pathCreator.bezierPath.GetPoint(i * 3 + 1));
            }
        }
        levelLoaded.Raise(); //Raise level loaded event
    }
    private IEnumerator SpawnWall() //Spawns edge walls
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < pathCreator.bezierPath.NumPoints - 2; i += 1)
        {

            Transform wall1 = Instantiate(wallPrefab, pathCreator.bezierPath.GetPoint(i), Quaternion.identity, transform).GetComponent<Transform>();
            wall1.LookAt(pathCreator.bezierPath.GetPoint(i + 1));
            wall1.position += wall1.transform.right * (currentLevel.roadWidth + 0.5f);

            Transform wall2 = Instantiate(wallPrefab, pathCreator.bezierPath.GetPoint(i), Quaternion.identity, transform).GetComponent<Transform>();
            wall2.LookAt(pathCreator.bezierPath.GetPoint(i + 1));
            wall2.position += wall1.transform.right * -(currentLevel.roadWidth + 0.5f);
        }
    }
    private bool ShouldGenerateCurve() //Check if should generate curves with current level generation difficulty
    {
        return Random.Range(0, currentLevel.CurveGenerationDifficulty) == 1;
    }
    private bool ShouldGenerateObstacle() //Check if should generate Obstacles with current level generation difficulty
    {
        return Random.Range(0, currentLevel.ObstacleGenerationDifficulty) == 1;
    }
    private bool ShouldGenerateCube() //Check if should generate Cubes with current level generation difficulty
    {
        return Random.Range(0, currentLevel.CubesGenerationDifficulty) == 1;
    }
    private bool ShouldGenerateCheese() //Check if should generate Cheeses with current level generation difficulty
    {
        return Random.Range(0, currentLevel.CheeseGenerationDifficulty) == 1;
    }
    BezierPath GenerateBezierPath(Vector3[] points, bool closedPath, float vertexSpacing) //Generate the BezierPath for the path.
    {
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xyz);

        return bezierPath;
    }

    public PathCreator GetPathCreator() //Returns the current pathCreator component
    {
        return pathCreator;
    }
}
