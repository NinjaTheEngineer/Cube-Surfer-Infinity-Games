using System.Collections.Generic;
using UnityEngine;

public class CubeStack : MonoBehaviour
{
    public float initY = 0.36f;
    public float cubeHeight = 0.75f;

    [SerializeField]
    private Transform playerModelTransform;
    private List<GameObject> stackedCubes;

    private int amountOfCubesStacked = 0;
    private bool isOnTopOfObstacle = false;

    private void Start()
    {
        stackedCubes = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            amountOfCubesStacked++;
            playerModelTransform.localPosition = new Vector3(0, playerModelTransform.localPosition.y + cubeHeight, 0);
            stackedCubes.Add(other.gameObject);
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, amountOfCubesStacked * cubeHeight, 0);
            other.transform.localRotation = Quaternion.identity;
        }
        else if (other.CompareTag("Obstacle") && !isOnTopOfObstacle)
        {
            isOnTopOfObstacle = true;
            amountOfCubesStacked--;
            int lastCube = stackedCubes.Count - 1;
            transform.localPosition = new Vector3(transform.localPosition.x, cubeHeight, transform.localPosition.z);
            stackedCubes[lastCube].tag = "UsedCube";
            stackedCubes[lastCube].transform.SetParent(null);
            Vector3 cubePos = stackedCubes[lastCube].transform.localPosition;
            stackedCubes[lastCube].transform.localPosition = new Vector3(cubePos.x, initY, cubePos.z);
            stackedCubes.RemoveAt(lastCube);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(">TriggerExit -> " + other.tag);
        if (other.CompareTag("Obstacle") && isOnTopOfObstacle)
        {
            isOnTopOfObstacle = false;
            other.tag = "PassedObstacle";
            playerModelTransform.localPosition = new Vector3(0, playerModelTransform.localPosition.y - cubeHeight, 0);
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }

    }
}
