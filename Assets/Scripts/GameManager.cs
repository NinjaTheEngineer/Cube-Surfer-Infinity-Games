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
        Initialize();
    }
    private void Initialize() //Initializes all game objects needed to play the game
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
    public void Restart() //Restarts the game deleting every essential game object and initializing the game again
    {
        Destroy(playerGO);
        Destroy(levelGO);
        Destroy(uiManagerGO);
        Initialize();    
    }
    public void OnLevelLoaded() //When the level is loaded initializes the player and communicates to the UIManager
    {
        playerController.Initialize(levelGenerator.GetPathCreator());
        uiManager.HideInitialPanel();
    }
    public void OnLevelStart() //When the level starts tell the UIManager to update the UI
    {
        uiManager.OnLevelStart();
    }
    public void OnLevelFailed() //Listen to the level failed even and communicates it to the playerController and the UIManager
    {
        playerController.LevelFailed();
        uiManager.ShowFailedLevelScreen();
    }
    public void OnLevelFinished() //Listen to the level finished event and handles what happens
    {

        if (PlayerPrefs.GetInt("maxLevel", 0) < listOfLevels.Length) //Saves the maxLevel reached
            PlayerPrefs.SetInt("maxLevel", currentLevel + 1); 
 
        if(currentLevel < listOfLevels.Length - 1 && PlayerPrefs.GetInt("maxLevel", 0) > currentLevel) //Sets the new currentLevel and updates UI
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
            uiManager.UpdateAvailableLevels();
            uiManager.ShowNextLevelScreen();
        }
        else //If all levels are completed, show the end game screen 
        {
            uiManager.ShowEndGameScreen();
        }

    }

    public void OnCheeseCollected() //Handles the cheese collected event
    {
        cheeseCollectedAmount = PlayerPrefs.GetInt("cheeseAmount", 0);
        cheeseCollectedAmount++;
        PlayerPrefs.SetInt("cheeseAmount", cheeseCollectedAmount); //Saves the current amount of cheese collected
        UpdateCheeseAmount();
    }
    private void UpdateCheeseAmount() //Updates the cheese to the UI
    {
        cheeseCollectedAmount = PlayerPrefs.GetInt("cheeseAmount", 0);
        uiManager.UpdateCheeseAmount(cheeseCollectedAmount);
    }
    public void OnLevelSwitch() //Handles what happens on level switch event
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        Restart();
    }
    public void GoToNextLevel() //Goes to the next level
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        Restart();
    }

}