using PathCreation;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public Transform leftCheckPos, rightCheckPos, endCheckPos;
    public LayerMask WhatIsWall, WhatIsEnd;
    [SerializeField] private GameEvent levelFinished, levelStarted;

    private PathCreator pathCreator;
    private Vector3 perpendicularVector;
    private Vector3 touchBeginPos;
    private Vector3 touchMovedPos;
    private Touch touch;
    private float initYOffset = 0.36f;
    private float distanceTravelled;
    private float horizontalMovement;

    private bool playerInitialized = false;
    private bool gameStarted = false;
    private bool levelFailed = false;
    private bool leftCheck = false;
    private bool rightCheck = false;

    private void Update() //Handles all logic
    {
        CheckForLevelStart();
        HandlePlayerInput();
        WallChecks();
        EndCheck();
    }

    private void FixedUpdate() //Handles physics
    {
        if ( gameStarted && !levelFailed)
        {
            distanceTravelled += Speed * Time.deltaTime;
            HandleMovementAndRotation();
        }
    }
    public void HandlePlayerInput() //Handles player input in android and unity editor
    {
#if UNITY_ANDROID  //If playing in android device
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchBeginPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                touchMovedPos = touch.position;
                if (touchMovedPos.x > touchBeginPos.x + 10f)
                    horizontalMovement = 1;
                else if (touchMovedPos.x < touchBeginPos.x - 10f)
                    horizontalMovement = -1;
                else
                    horizontalMovement = 0;
            }
            else if(touch.phase == TouchPhase.Stationary)
            {
                horizontalMovement = 0;
                touchBeginPos = touch.position;
            }
        }
        else
        {
            horizontalMovement = 0;
        }
#endif

#if UNITY_EDITOR //If playing in the unity editor

        horizontalMovement = Input.GetAxisRaw("Horizontal");

#endif
    }
    private void CheckForLevelStart() //Check for the player first input to start to move
    {
        if ((horizontalMovement > 0f || horizontalMovement < 0f) && !levelFailed)
        {
            gameStarted = true;
            levelStarted.Raise();
        }
    }
    public void Initialize(PathCreator pathCreator) //Initializes the player with the current path
    {
        this.pathCreator = pathCreator;
        Vector3 initiaPos = pathCreator.path.GetPointAtDistance(0);
        initiaPos.y += initYOffset;
        transform.position = initiaPos;
        transform.rotation = pathCreator.path.GetRotationAtDistance(0);
    }
    private void HandleMovementAndRotation() //Handle player movement and rotation in the path
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
    public void LevelFailed() //Set level to failed
    {
        levelFailed = true;
    }
    private void WallChecks() //Check if colliding with the edge walls
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
    private void EndCheck() //Check if the player has reached the end of the level
    {
        if ((Physics.Raycast(endCheckPos.position, endCheckPos.right, 5f, WhatIsEnd) ||
            Physics.Raycast(endCheckPos.position, -endCheckPos.right, 5f, WhatIsEnd)) && gameStarted)
            EndReached();
    }
    private void EndReached() //Handles end reached
    {
        playerInitialized = false;
        gameStarted = false;
        levelFinished.Raise(); //Raises end reach event
    }
}
