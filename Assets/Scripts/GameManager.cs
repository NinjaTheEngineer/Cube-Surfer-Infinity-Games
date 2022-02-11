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
    private int currentLevel, cheeseCollectedAmount;

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
        UpdateCheeseAmount();
    }
    public void Restart()
    {
        Destroy(playerGO);
        Destroy(levelGO);
        Destroy(uiManagerGO);
        Initialize();    
    }
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
        Debug.Log("maxlevel -> " + PlayerPrefs.GetInt("maxLevel", 0) + "__ currentLevel-> " + currentLevel);

        if (PlayerPrefs.GetInt("maxLevel", 0) < listOfLevels.Length)
            PlayerPrefs.SetInt("maxLevel", currentLevel + 1); 
 
        if(currentLevel < listOfLevels.Length - 1 && PlayerPrefs.GetInt("maxLevel", 0) > currentLevel)
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
            uiManager.UpdateAvailableLevels();
            uiManager.ShowNextLevelScreen();
        }
        else
        {
            uiManager.ShowEndGameScreen();
        }

    }

    public void OnCheeseCollected()
    {
        cheeseCollectedAmount = PlayerPrefs.GetInt("cheeseAmount", 0);
        cheeseCollectedAmount++;
        PlayerPrefs.SetInt("cheeseAmount", cheeseCollectedAmount);
        UpdateCheeseAmount();
    }
    private void UpdateCheeseAmount()
    {
        cheeseCollectedAmount = PlayerPrefs.GetInt("cheeseAmount", 0);
        uiManager.UpdateCheeseAmount(cheeseCollectedAmount);
    }
    public void OnLevelSwitch()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        Restart();
    }
    public void GoToNextLevel()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        Restart();
    }

}