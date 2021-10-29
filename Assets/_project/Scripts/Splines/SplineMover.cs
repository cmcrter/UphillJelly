//===========================================================================================================================================================================================================================================================================
// Name:                SplineMover.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
// Brief:               A component for making an object move along a given spline
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component for making an object move along a given spline
/// </summary>
public class SplineMover : MonoBehaviour
{
    #region Public Fields
    /// <summary>
    /// How fast the object will move along the SplineSequence 
    /// </summary>
    [SerializeField]
    [Tooltip("How fast the object will move along the SplineSequence")]
    private float speed = 1.0f;
    /// <summary>
    /// How fast the object will move along the SplineSequence (property accessor)
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
            UpdateTIncrement();
        }
    }
    #endregion

    #region Serialized Private Fields
    /// <summary>
    /// If the object should follow the x position of the spline
    /// </summary>
    [SerializeField] [Tooltip("If the object should follow the x position of the spline")]
    private bool followX = false;
    /// <summary>
    /// If the object should follow the y position of the spline
    /// </summary>
    [SerializeField] [Tooltip("If the object should follow the y position of the spline")]
    private bool followY = false;
    /// <summary>
    /// If the object should follow the z position of the spline
    /// </summary>
    [SerializeField] [Tooltip("If the object should follow the z position of the spline")]
    private bool followZ = false;
    /// <summary>
    /// If the object should return to the beginning of the spline once it at the end
    /// </summary>
    [SerializeField] [Tooltip("If the object should return to the beginning of the spline once it at the end")]
    private bool loop = true;



    /// <summary>
    /// The SplineSequence to follow along
    /// </summary>
    [SerializeField]
    [Tooltip("The spline sequence to follow along")]
    private SplineSequence splineSequence = null;
    #endregion

    #region Private Variables
    /// <summary>
    /// The unit interval for how far along the spline sequence it is
    /// </summary>
    private float currentTValue = 0.0f;

    /// <summary>
    /// How much the unit interval to position along the spline increases every frame
    /// </summary>
    private float tIncrementPerSecond = Mathf.Infinity;
    #endregion

    #region Public Functions
    /// <summary>
    /// Adjust the currentTValue based on it moving over by a given number of seconds
    /// </summary>
    /// <param name="seconds">The number of seconds to offset the position based on</param>
    public void AddPositionBySeconds(float seconds)
    {
        UpdateTIncrement();
        Debug.Log("Time Delay: " + seconds.ToString());
        currentTValue += tIncrementPerSecond * seconds;
    }

    public void SetNewSeqeunceToFollow(SplineSequence splineSequenceToFollow, float startT = 0.0f)
    {
        splineSequence = splineSequenceToFollow;
        currentTValue = startT;
        UpdateTIncrement();
    }
    #endregion

    #region Private Functions
    /// <summary>
    /// Adjusts the tIncrementPerSecond based on speed and the splineSequence's total length
    /// </summary>
    private void UpdateTIncrement()
    {
        if (splineSequence != null)
        {
            tIncrementPerSecond = speed / splineSequence.totalLength;
        }
    }
    #endregion

    #region Unity Methods
    // Update is called once per frame
    void Update()
    {
        UpdateTIncrement();
        if (currentTValue + Time.deltaTime * tIncrementPerSecond < 1)
        {
            // Clamping it at the max value and min values of a unit interval

            // Check the length of the next increment
            Vector3 nextPoint = splineSequence.GetLengthBasedPoint(currentTValue + Time.deltaTime * tIncrementPerSecond);
            Vector3 currentPoint = splineSequence.GetLengthBasedPoint(currentTValue);
            Vector3 velocity = nextPoint - currentPoint;


            // Ideally the distance change should be speed * time.deltaTime
            float desiredDistance = speed * Time.deltaTime;
            float currentDistanceChange = velocity.magnitude;

            float desiredChange = desiredDistance / currentDistanceChange;
            currentTValue = Mathf.Clamp01(currentTValue + Time.deltaTime * tIncrementPerSecond * desiredChange); // add length to this calculation
        }
        else
        {
            currentTValue = Mathf.Clamp01(currentTValue + Time.deltaTime * tIncrementPerSecond); // add length to this calculation
        }
        
        if (loop)
        {
            if (tIncrementPerSecond > 0f)
            {
                if (currentTValue == 1f)
                {
                    currentTValue = 0f;
                }
            }
            else if (tIncrementPerSecond < 0f)
            {
                if (currentTValue == 0f)
                {
                    currentTValue = 1f;
                }
            }
        }

        // Setting the required position values to given time
        if (splineSequence != null)
        {
            Vector3 targetPosition = splineSequence.GetLengthBasedPoint(currentTValue);
            if (!followX)
            {
                targetPosition.x = transform.position.x;
            }
            if (!followY)
            {
                targetPosition.y = transform.position.y;
            }
            if (!followZ)
            {
                targetPosition.z = transform.position.z;
            }
            transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("SplineSequence not assigned when referenced in Spline Mover update function", gameObject);
        }

        transform.forward = splineSequence.GetDirection(currentTValue, 0.01f);
    }

    private IEnumerator waitDebug(float seconds)
    {
        yield return new WaitForSeconds(10.0f);
        UpdateTIncrement();
        Debug.Log("Time Delay: " + seconds.ToString());
        currentTValue += tIncrementPerSecond * seconds;
        currentTValue = Mathf.Clamp01(currentTValue);
        if (loop)
        {
            if (tIncrementPerSecond > 0f)
            {
                if (currentTValue == 1f)
                {
                    currentTValue = 0f;
                }
            }
            else if (tIncrementPerSecond < 0f)
            {
                if (currentTValue == 0f)
                {
                    currentTValue = 1f;
                }
            }
        }
    }
    #endregion
}
