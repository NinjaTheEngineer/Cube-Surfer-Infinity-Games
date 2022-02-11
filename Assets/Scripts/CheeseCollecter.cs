using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseCollecter : MonoBehaviour
{
    [SerializeField] private GameEvent cheeseCollected;
    private void OnTriggerEnter(Collider collision) //To check if a cheese is collected
    {
        if (collision.CompareTag("Cheese"))
        {
            Destroy(collision.gameObject);
            cheeseCollected.Raise();
        }
    }
}
