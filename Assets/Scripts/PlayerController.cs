using PathCreation;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public Transform leftCheckPos, rightCheckPos, endCheckPos;
    public LayerMask WhatIsWall, WhatIsEnd;

    [SerializeField] private GameEvent levelFinished;

    private PathCreator pathCreator;
    private Vector3 perpendicularVector;

    private float initYOffset = 0.36f;
    private float distanceTravelled;
    private float horizontalMovement;

    private bool gameStarted = false;
    private bool leftCheck = false;
    private bool rightCheck = false;
    
    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        WallChecks();
        EndCheck();
    }

    private void FixedUpdate()
    {
        if (gameStarted)
        {
            distanceTravelled += Speed * Time.deltaTime;
            HandleMovementAndRotation();
        }
    }
    public void Initialize(PathCreator pathCreator, int endPoint)
    {
        this.pathCreator = pathCreator;
        Vector3 initiaPos = pathCreator.path.GetPointAtDistance(0);
        initiaPos.y += initYOffset;
        transform.position = initiaPos;
        transform.rotation = pathCreator.path.GetRotationAtDistance(0);
        StartGame();
    }

    private void HandleMovementAndRotation()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;
        perpendicularVector = Vector3.Cross(transform.forward, Vector3.up).normalized;

        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

        if (horizontalMovement != 0)
        {
            if (horizontalMovement > 0 && rightCheck || horizontalMovement < 0 && leftCheck)
                return;

            transform.position += perpendicularVector * -horizontalMovement * Time.deltaTime * Speed;
        }
    }

    public void LevelFailed()
    {
        gameStarted = false;
    }

    private void WallChecks()
    {
        if (Physics.Raycast(leftCheckPos.position, -leftCheckPos.right, 0.5f, WhatIsWall))
        {
            leftCheck = true;
        }
        else
        {
            leftCheck = false;
        }
        if (Physics.Raycast(rightCheckPos.position, rightCheckPos.right, 0.5f, WhatIsWall))
        {
            rightCheck = true;
        }
        else
        {
            rightCheck = false;
        }
    }
    private void EndCheck()
    {
        if (Physics.Raycast(endCheckPos.position, endCheckPos.right, 5f, WhatIsEnd) && gameStarted)
            EndReached();
    }
    private void EndReached()
    {
        levelFinished.Raise();
        gameStarted = false;
    }

    public void StartGame()
    {
        gameStarted = true;
    }
}
