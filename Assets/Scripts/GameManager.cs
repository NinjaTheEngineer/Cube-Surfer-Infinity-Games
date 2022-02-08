using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab, LevelGenerator;
    public LevelSO[] listOfLevels;

    private LevelGenerator levelGenerator;
    private PlayerController playerController;

    private void Awake()
    {
        levelGenerator = Instantiate(LevelGenerator, Vector3.zero, Quaternion.identity).GetComponent<LevelGenerator>();
        levelGenerator.SetUpLevels(listOfLevels);
        playerController = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        playerController.Initialize(levelGenerator.GetPathCreator());
    }
    private void Start()
    {
        levelGenerator.SetUpLevels(listOfLevels);
    }

}