////////////////////////////////////////////////////////////
// File: MainMenuNavigation
// Author: Charles Carter
// Date Created: 02/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 02/02/22
// Brief: The general going between menu points (as a general state machine)
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuNavigation : MonoBehaviour
{
    #region Variables

    static MainMenuNavigation instance;

    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private MainMenuPoint currentPoint;
    [SerializeField]
    private PlayerInput playerInput;

    private Coroutine coLerp;
    private Coroutine coIdle;

    [SerializeField]
    private float transitionTime = 2f;

    //Idle Variables
    [SerializeField]
    private float idleSwayAmount = 1.25f;
    [SerializeField]
    private float movementDecay = 1f;
    [SerializeField]
    private float timeToMove = 12f;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
        }

        instance = this;

        playerInput = playerInput ?? GetComponent<PlayerInput>();

        //Setting the state to be the inspectors current one
        GoToState(currentPoint);
    }

    private void OnEnable()
    {
        playerInput.actions["Navigate"].performed += Navigate;
    }

    private void OnDisable()
    {
        playerInput.actions["Navigate"].performed -= Navigate;
    }

    #endregion

    #region Public Methods

    //Going to a state designated by the monobehaviour on that state
    public void GoToState(MainMenuPoint nextPoint)
    {
        StartLerpToPoint(nextPoint);
    }

    /// <summary>
    /// Controls pressed to go between sections
    /// </summary>
    public void Navigate(InputAction.CallbackContext callbackContext)
    {
        if(coLerp != null)
            return;

        Vector2 v = callbackContext.action.ReadValue<Vector2>();

        //Getting whether it's left or right and going to there
        if(Mathf.Abs(v.x) != Mathf.Abs(v.y))
        {
            //Since only x or y can be not 0 here, this logic makes sense
            if(v.x != 0)
            {
                if(v.x < 0)
                {
                    Debug.Log("Left", this);
                    //left
                }
                else
                {
                    Debug.Log("Right", this);
                    //right
                }
            }
            else if(v.y < 0)
            {
                Debug.Log("Down", this);
                //down
            }
            else
            {
                Debug.Log("Up", this);
                //up
            }
        }
    }

    #endregion

    #region Private Methods

    void StartLerpToPoint(MainMenuPoint nextPoint)
    {
        if(coLerp != null)
            return;

        StopIdle();

        coLerp = StartCoroutine(Co_LerpToPoint(nextPoint));
    }

    void StopLerpToPoint()
    {
        if(coLerp != null)
        {
            StopCoroutine(coLerp);
        }
    }

    private IEnumerator Co_LerpToPoint(MainMenuPoint nextPoint)
    {
        nextPoint.gameObject.SetActive(true);

        if(currentPoint != nextPoint)
        {
            currentPoint.gameObject.SetActive(false);
        }

        float amountToMove = (currentPoint.transform.position - nextPoint.transform.position).magnitude;
        float speedToMove = amountToMove / transitionTime;

        while(camera.transform.position != nextPoint.transform.position && camera.transform.rotation != nextPoint.transform.rotation)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, nextPoint.transform.position, Time.deltaTime * speedToMove);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, nextPoint.transform.rotation, Time.deltaTime * speedToMove);
            yield return null;
        }

        //Updating the state variable
        currentPoint = nextPoint;

        //Starting to Idle
        StartIdle();
        coLerp = null;
    }

    void StartIdle()
    {
        StopIdle();
        coIdle = StartCoroutine(Co_IdleCamera());
    }

    void StopIdle()
    {
        if(coIdle != null)
        {
            StopCoroutine(coIdle);
        }
    }

    private IEnumerator Co_IdleCamera()
    {
        float movementTime = 0f;
        Vector3 targetPosition = new Vector3();

        while(gameObject.activeSelf)
        {
            //We only get a new position after we finished transitioning to our other point.
            if(movementTime <= 0)
            {
                targetPosition = currentPoint.transform.position + Random.insideUnitSphere * idleSwayAmount;
                movementTime = timeToMove;
            }
            else
            {
                //We use Lerp to smoothly transiton from our current position to the next.
                camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, (timeToMove / movementDecay) * Time.deltaTime);
                movementTime -= Time.deltaTime;
            }

            yield return null;
        }

        coIdle = null;
    }

    #endregion
}
