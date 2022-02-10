using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public GameObject PlayerPrefab, LevelGeneratorPrefab, UIManagerPrefab;
    public LevelSO[] listOfLevels;

    private UIManager uiManager;
    private GameObject playerGO, levelGO, uiManagerGO;
    private LevelGenerator levelGenerator;
    private PlayerController playerController;
    private int currentLevel;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        Initialize();
    }
    private void Initialize()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        uiManagerGO = Instantiate(UIManagerPrefab, Vector3.zero, Quaternion.identity);
        uiManager = uiManagerGO.GetComponent<UIManager>();
        levelGO = Instantiate(LevelGeneratorPrefab, Vector3.zero, Quaternion.identity);
        levelGenerator = levelGO.GetComponent<LevelGenerator>();
        levelGenerator.SetUpLevels(listOfLevels, currentLevel);
        playerGO = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        playerController = playerGO.GetComponent<PlayerController>();
        cinemachine.LookAt = playerController.GetComponent<Transform>();
        cinemachine.Follow = playerController.GetComponent<Transform>();
        uiManager.Initialize(listOfLevels.Length);
    }
    public void Restart()
    {
        Destroy(playerGO);
        Destroy(levelGO);
        Destroy(uiManagerGO);
        Initialize();    }
    public void OnLevelLoaded()
    {
        playerController.Initialize(levelGenerator.GetPathCreator(), levelGenerator.GetEndPoint());
        uiManager.HideInitialPanel();
    }
    public void OnLevelStart()
    {
        uiManager.OnLevelStart();
    }
    public void OnLevelFailed()
    {
        playerController.LevelFailed();
        uiManager.ShowFailedLevelScreen();
    }
    public void OnLevelFinished()
    {
        if(PlayerPrefs.GetInt("maxLevel", 0) <= currentLevel)
        {
            PlayerPrefs.SetInt("maxLevel", currentLevel + 1);
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
            uiManager.UpdateAvailableLevels();
        }
    }

    public void OnLevelSwitch()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        Restart();
    }

}