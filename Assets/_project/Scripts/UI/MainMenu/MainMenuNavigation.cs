////////////////////////////////////////////////////////////
// File: MainMenuNavigation
// Author: Charles Carter
// Date Created: 02/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 04/02/22
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
    private GameObject cameraParent;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private MainMenuPoint currentPoint;
    [SerializeField]
    private PlayerInput playerInput;

    private Coroutine coLerp;
    private Coroutine coIdle;

    [SerializeField]
    private float transitionTime = 2f;

    //Idle Variables
    //A gameobject for the camera to look at whilst swaying
    [SerializeField]
    private GameObject RotationObject;
    [SerializeField]
    private Vector3 initialRotLocalPos;
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

        initialRotLocalPos = RotationObject.transform.localPosition;
    }

    private void OnEnable()
    {
        playerInput.actions["Navigate"].performed += Navigate;
        playerInput.actions["Cancel"].performed += Cancel;
    }

    private void OnDisable()
    {
        playerInput.actions["Navigate"].performed -= Navigate;
        playerInput.actions["Cancel"].performed -= Cancel;
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
                    //Debug.Log("Left", this);
                    //left
                    if(currentPoint.LeftPoint != null)
                    {
                        Debug.Log(currentPoint.LeftPoint.name);
                        GoToState(currentPoint.LeftPoint);
                    }
                }
                else
                {
                    //Debug.Log("Right", this);
                    //right
                    if(currentPoint.RightPoint != null)
                    {
                        Debug.Log(currentPoint.RightPoint.name);
                        GoToState(currentPoint.RightPoint);
                    }
                }
            }
            else if(v.y < 0)
            {
                //Debug.Log("Down", this);
                //down
                if(currentPoint.DownPoint != null)
                {
                    GoToState(currentPoint.DownPoint);
                }
            }
            else
            {
                //Debug.Log("Up", this);
                //up
                if(currentPoint.UpPoint != null)
                {
                    GoToState(currentPoint.UpPoint);
                }
            }
        }
    }

    public void Cancel(InputAction.CallbackContext callbackContext)
    {
        if(currentPoint.BackPoint != null)
        {
            GoToState(currentPoint.BackPoint);
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
        if(nextPoint == null)
        {
            Debug.LogError("No Next Point Here", this);
            StopLerpToPoint();
            coLerp = null;
            yield return null;
        }

        nextPoint.gameObject.SetActive(true);

        if(currentPoint != nextPoint)
        {
            currentPoint.gameObject.SetActive(false);
        }

        float amountToMove = (currentPoint.transform.position - nextPoint.transform.position).magnitude;
        float speedToMove = amountToMove / transitionTime;

        while(cameraParent.transform.position != nextPoint.transform.position && cameraParent.transform.rotation != nextPoint.transform.rotation)
        {
            cameraParent.transform.position = Vector3.Lerp(cameraParent.transform.position, nextPoint.transform.position, Time.deltaTime * speedToMove);
            cameraParent.transform.rotation = Quaternion.Lerp(cameraParent.transform.rotation, nextPoint.transform.rotation, Time.deltaTime * speedToMove);
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
        Vector3 rotTargetPosition = new Vector3();

        Vector3 rotInitialPos = RotationObject.transform.position;

        while(gameObject.activeSelf)
        {
            //We only get a new position after we finished transitioning to our other point.
            if(movementTime <= 0)
            {
                targetPosition = currentPoint.transform.position + Random.insideUnitSphere * idleSwayAmount;
                rotTargetPosition = rotInitialPos + Random.insideUnitSphere * (idleSwayAmount * 5);
                movementTime = timeToMove;
            }
            else
            {
                //We use Lerp to smoothly transiton from our current position to the next.
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, (timeToMove / movementDecay) * Time.deltaTime);
                RotationObject.transform.position = Vector3.Lerp(RotationObject.transform.position, rotTargetPosition, (timeToMove / movementDecay) * Time.deltaTime);

                _camera.transform.LookAt(RotationObject.transform);
                movementTime -= Time.deltaTime;
            }

            yield return null;
        }

        coIdle = null;
    }

    #endregion
}
