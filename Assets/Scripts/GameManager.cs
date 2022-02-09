using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public GameObject PlayerPrefab, LevelGenerator;
    public LevelSO[] listOfLevels;

    private GameObject playerGO, levelGO;
    private LevelGenerator levelGenerator;
    private PlayerController playerController;
    private int numberOfLevelsCompleted;

    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        numberOfLevelsCompleted = GetNumberOfLevelsCompleted();
        levelGO = Instantiate(LevelGenerator, Vector3.zero, Quaternion.identity);
        levelGenerator = levelGO.GetComponent<LevelGenerator>();
        levelGenerator.SetUpLevels(listOfLevels, numberOfLevelsCompleted);
        playerGO = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        playerController = playerGO.GetComponent<PlayerController>();
        cinemachine.LookAt = playerController.GetComponent<Transform>();
        cinemachine.Follow = playerController.GetComponent<Transform>();
    }
    public void Restart()
    {
        Destroy(playerGO);
        Destroy(levelGO);
        Initialize();
    }
    public void OnLevelLoaded()
    {
        playerController.Initialize(levelGenerator.GetPathCreator(), levelGenerator.GetEndPoint());
    }

    private int GetNumberOfLevelsCompleted()
    {
        return PlayerPrefs.GetInt("LevelsCompleted", 0);
    }
    private void SetNumberOfLevelsCompleted(int numberOfLevels)
    {
        PlayerPrefs.SetInt("LevelsCompleted", numberOfLevels);
    }

    public void OnLevelFailed()
    {
        playerController.LevelFailed();
    }

}