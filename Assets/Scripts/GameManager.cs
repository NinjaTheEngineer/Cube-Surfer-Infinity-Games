using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject LevelGenerator;
    public LevelSO[] listOfLevels;

    private LevelGenerator levelGenerator;

    private void Awake()
    {
        levelGenerator = Instantiate(LevelGenerator, Vector3.zero, Quaternion.identity).GetComponent<LevelGenerator>();
        levelGenerator.SetUpLevels(listOfLevels);
    }
    private void Start()
    {
        levelGenerator.SetUpLevels(listOfLevels);
    }

}