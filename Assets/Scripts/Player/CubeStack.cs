using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeStack : MonoBehaviour
{
    public float initY = 0.36f;
    public float cubeHeight = 0.75f;

    [SerializeField]
    private Transform playerModelTransform;
    [SerializeField]
    private GameEvent onCollectedCube, onObstacleHit, onLevelFailed;
    private List<GameObject> stackedCubes;

    private int amountOfCubesStacked = 0;
    private bool isOnTopOfObstacle = false;

    private void Start() //Initialize stacked cubes list
    {
        stackedCubes = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other) //Check if collision trigger with cubes or obstacles
    {
        if (other.CompareTag("Cube"))
        {
            CollectingCube(other);
        }
        else if (other.CompareTag("Obstacle") && !isOnTopOfObstacle)
        {
            if (amountOfCubesStacked.Equals(0)) //If no cubes are stacked fails the level
            {
                LevelFailed();
                return;
            }
            ObstacleHit();
        }
    }
    private void CollectingCube(Collider collider) //Collects cube and handles player position and cube
    {
        onCollectedCube.Raise(); //Raises collected cube event
        amountOfCubesStacked++;
        playerModelTransform.localPosition = new Vector3(0, playerModelTransform.localPosition.y + cubeHeight, 0);
        stackedCubes.Add(collider.gameObject);
        collider.transform.SetParent(transform);
        collider.transform.localPosition = new Vector3(0, amountOfCubesStacked * cubeHeight, 0);
        collider.transform.localRotation = Quaternion.identity;
    }
    private void ObstacleHit() //Handles obstacle hit, leaving one cube behind if possible
    {
        onObstacleHit.Raise(); //Raises obstacle hit event
        isOnTopOfObstacle = true;
        amountOfCubesStacked--;
        int lastCube = stackedCubes.Count - 1;
        transform.localPosition = new Vector3(transform.localPosition.x, cubeHeight, transform.localPosition.z);
        stackedCubes[lastCube].tag = "UsedCube";
        stackedCubes[lastCube].transform.SetParent(null);
        Destroy(stackedCubes[lastCube].gameObject, 4f);
        Vector3 cubePos = stackedCubes[lastCube].transform.localPosition;
        stackedCubes[lastCube].transform.localPosition = new Vector3(cubePos.x, initY, cubePos.z);
        stackedCubes.RemoveAt(lastCube);
    }
    private void LevelFailed() //Raises of level ended
    {
        onLevelFailed.Raise();
    }

    private void OnTriggerExit(Collider other) //Checks when out of obstacle to restore player position
    {
        if (other.CompareTag("Obstacle") && isOnTopOfObstacle)
        {
            isOnTopOfObstacle = false;
            other.tag = "Untagged";
            playerModelTransform.localPosition = new Vector3(0, playerModelTransform.localPosition.y - cubeHeight, 0);
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }
    }
}
