using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab, LevelGenerator;
    public LevelSO[] listOfLevels;

    private LevelGenerator levelGenerator;
    private PlayerController playerController;
    private int numberOfLevelsCompleted;

    private void Awake()
    {
        numberOfLevelsCompleted = GetNumberOfLevelsCompleted();
        levelGenerator = Instantiate(LevelGenerator, Vector3.zero, Quaternion.identity).GetComponent<LevelGenerator>();
        levelGenerator.SetUpLevels(listOfLevels, numberOfLevelsCompleted);
        playerController = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
    }

    public void OnLevelWasLoaded()
    {
        playerController.Initialize(levelGenerator.GetPathCreator());
    }

    private int GetNumberOfLevelsCompleted()
    {
        return PlayerPrefs.GetInt("LevelsCompleted", 0);
    }
    private void SetNumberOfLevelsCompleted(int numberOfLevels)
    {
        PlayerPrefs.SetInt("LevelsCompleted", numberOfLevels);
    }

}