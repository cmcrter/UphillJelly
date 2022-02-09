////////////////////////////////////////////////////////////
// File: MainMenuNavigation
// Author: Charles Carter
// Date Created: 02/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 04/02/22
// Brief: The general going between menu points (as a general state machine)
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MainMenuNavigation : MonoBehaviour
{
    #region Variables

    static MainMenuNavigation instance;

    [Header("General Movement Variables")]
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

    [Header("Idle Variables")]
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

    [Header("Options Variables")]
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown textureDropdown;
    public TMP_Dropdown aaDropdown;
    public Slider volumeSlider;
    private float currentVolume;
    Resolution[] resolutions;

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

    private void Start()
    {
        if(!resolutionDropdown)
        {
            return;
        }

        //Setting up the resolution options
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                //This is the current one (to be saved later)
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
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
                        //Debug.Log(currentPoint.LeftPoint.name);
                        GoToState(currentPoint.LeftPoint);
                    }
                }
                else
                {
                    //Debug.Log("Right", this);
                    //right
                    if(currentPoint.RightPoint != null)
                    {
                        //Debug.Log(currentPoint.RightPoint.name);
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

    public void PlayGame()
    {
        //Use currently selected level to make loading data and go to the loading screen
        
    }

    #region Options Section

    /// <summary>
    /// Setting the general master volume (TO DO: Expand this to the FMOD way)
    /// </summary>
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    public void SetQuality(int qualityIndex)
    {
        if(qualityIndex != 6)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        switch(qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }

        qualityDropdown.value = qualityIndex;
    }

    #endregion

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
