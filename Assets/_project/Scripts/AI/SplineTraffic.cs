using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTraffic : MonoBehaviour
{
    #region Public Fields
    /// <summary>
    /// How fast the object will move along the SplineSequence 
    /// </summary>
    [SerializeField]
    [Tooltip("How fast the object will move along the SplineSequence")]
    private float speed = 1.0f;

    #endregion

    #region Serialized Private Fields
    /// <summary>
    /// If the object should follow the x position of the spline
    /// </summary>
    [SerializeField]
    [Tooltip("If the object should follow the x position of the spline")]
    private bool followX = false;
    /// <summary>
    /// If the object should follow the y position of the spline
    /// </summary>
    [SerializeField]
    [Tooltip("If the object should follow the y position of the spline")]
    private bool followY = false;
    /// <summary>
    /// If the object should follow the z position of the spline
    /// </summary>
    [SerializeField]
    [Tooltip("If the object should follow the z position of the spline")]
    private bool followZ = false;

    [SerializeField] [Tooltip("The amount of time between spawning traffic objects")]
    private float timeBetweenSpawns = 1f;

    [SerializeField] [Tooltip("The prefab that can be spawned to move along the splines")]
    private GameObject[] prefabs;


    [SerializeField]
    [Tooltip("The prefab that can be spawned to move along the splines")]
    private float[] prefabsWeights;

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
    private float[] currentTValues;

    private GameObject[] gameObjectsMoved;



    private int nextFreeIndex = 0;

    /// <summary>
    /// How much the unit interval to position along the spline increases every frame
    /// </summary>
    private float tIncrementPerSecond = Mathf.Infinity;

    private L7Games.Timer spawnTimer;
    #endregion

    #region Public Properties
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

    public int PrefabsAndWeightsCount 
    {
        get
        {
            return prefabs.Length;
        }
    }
    #endregion

    #region Public Functions
    public float GetPrefabWeightAtIndex(int index)
    {
        if (index < 0 || index >= prefabsWeights.Length)
        {
            #if UNITY_EDITOR || DEVELOPEMENT_BUILD
            if (index > 0)
            {
                Debug.LogError("Negative Index given to GetPrefabWeightAtIndex Function", this);
            }
            else
            {
                Debug.LogError("Index greater or equal to the Prefab weight array length given to GetPrefabWeightAtIndex Function", this);
            }
            return 0f;
            #endif
        }
        return prefabsWeights[index];
    }

    public void SetPrefabWeightAtIndex(int index, float newValue)
    {
        if (index < 0 || index >= prefabsWeights.Length)
        {
            #if UNITY_EDITOR || DEVELOPEMENT_BUILD
            if (index > 0)
            {
                Debug.LogError("Negative Index given to GetPrefabWeightAtIndex Function", this);
            }
            else
            {
                Debug.LogError("Index greater or equal to the Prefab weight array length given to GetPrefabWeightAtIndex Function", this);
            }
            #endif
            return;
        }
        prefabsWeights[index] = newValue;
    }

    public GameObject GetPrefabAtIndex(int index)
    {
        if (index < 0 || index >= prefabs.Length)
        {
            #if UNITY_EDITOR || DEVELOPEMENT_BUILD
            if (index > 0)
            {
                Debug.LogError("Negative Index given to GetPrefabWeightAtIndex Function", this);
            }
            else
            {
                Debug.LogError("Index greater or equal to the Prefab weight array length given to GetPrefabWeightAtIndex Function", this);
            }
            #endif
            return null;
        }
        return prefabs[index];
    }

    public void SetPrefabAtIndex(int index, GameObject newPrefab)
    {
        if (index < 0 || index >= prefabs.Length)
        {
            #if UNITY_EDITOR || DEVELOPEMENT_BUILD
            if (index > 0)
            {
                Debug.LogError("Negative Index given to GetPrefabWeightAtIndex Function", this);
            }
            else
            {
                Debug.LogError("Index greater or equal to the Prefab weight array length given to GetPrefabWeightAtIndex Function", this);
            }
            return;
            #endif
        }
        prefabs[index] = newPrefab;
    }

    public void AdjustPrefabsAndWeightsCount(int newCount)
    {
        GameObject[] newPrefabArray = new GameObject[newCount];
        float[] newWeightsArray = new float[newCount];
        System.Array.Copy(prefabs, newPrefabArray, Mathf.Min(prefabs.Length, newCount));
        System.Array.Copy(prefabsWeights, newWeightsArray, Mathf.Min(prefabs.Length, newCount));
        prefabs = newPrefabArray;
        prefabsWeights = newWeightsArray;
    }

    /// <summary>
    /// Adjust the currentTValue based on it moving over by a given number of seconds
    /// </summary>
    /// <param name="seconds">The number of seconds to offset the position based on</param>
    public void AddPositionBySeconds(float seconds, int indexToMove)
    {
        UpdateTIncrement();
        Debug.Log("Time Delay: " + seconds.ToString());
        currentTValues[indexToMove] += tIncrementPerSecond * seconds;
    }

    public void SetNewSeqeunceToFollow(SplineSequence splineSequenceToFollow, int indexToMove, float startT = 0.0f)
    {
        splineSequence = splineSequenceToFollow;
        currentTValues[indexToMove] = startT;
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

    private void UpdateMovedObject(int indexMoved)
    {
        if (gameObjectsMoved[indexMoved] == null)
        {
            return;
        }

        if (currentTValues[indexMoved] + Time.deltaTime * tIncrementPerSecond < 1)
        {
            // Clamping it at the max value and min values of a unit interval

            // Check the length of the next increment
            Vector3 nextPoint = splineSequence.GetLengthBasedPoint(currentTValues[indexMoved] + Time.deltaTime * tIncrementPerSecond);
            Vector3 currentPoint = splineSequence.GetLengthBasedPoint(currentTValues[indexMoved]);
            Vector3 velocity = nextPoint - currentPoint;


            // Ideally the distance change should be speed * time.deltaTime
            float desiredDistance = speed * Time.deltaTime;
            float currentDistanceChange = velocity.magnitude;

            float desiredChange = desiredDistance / currentDistanceChange;
            currentTValues[indexMoved] = Mathf.Clamp01(currentTValues[indexMoved] + Time.deltaTime * tIncrementPerSecond * desiredChange); // add length to this calculation
        }
        else
        {
            currentTValues[indexMoved] = Mathf.Clamp01(currentTValues[indexMoved] + Time.deltaTime * tIncrementPerSecond); // add length to this calculation
        }

        // Destroy the object when it reaches the end


        if (tIncrementPerSecond > 0f)
        {
            if (currentTValues[indexMoved] == 1f)
            {
                currentTValues[indexMoved] = 0f;
                GameObject.Destroy(gameObjectsMoved[indexMoved]);
                return;
            }
        }

        // Setting the required position values to given time
        if (splineSequence != null)
        {
            Vector3 targetPosition = splineSequence.GetLengthBasedPoint(currentTValues[indexMoved]);
            if (!followX)
            {
                targetPosition.x = gameObjectsMoved[indexMoved].transform.position.x;
            }
            if (!followY)
            {
                targetPosition.y = gameObjectsMoved[indexMoved].transform.position.y;
            }
            if (!followZ)
            {
                targetPosition.z = gameObjectsMoved[indexMoved].transform.position.z;
            }
            gameObjectsMoved[indexMoved].transform.LookAt(targetPosition);
            gameObjectsMoved[indexMoved].transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("SplineSequence not assigned when referenced in Spline Mover update function", gameObject);
        }

        transform.forward = splineSequence.GetDirection(currentTValues[indexMoved], 0.01f);
    }

    private GameObject SpawnObject()
    {
        spawnTimer = new L7Games.Timer(timeBetweenSpawns);
        int randomValue = Random.Range(0, prefabs.Length);
        Debug.Log(randomValue);
        GameObject returnedObject  = GameObject.Instantiate(prefabs[randomValue]);
        returnedObject.transform.position = splineSequence.WorldStartPosition;
        gameObjectsMoved[nextFreeIndex] = returnedObject;
        currentTValues[nextFreeIndex] = 0f;
        ++nextFreeIndex;
        if (nextFreeIndex == gameObjectsMoved.Length)
        {
            nextFreeIndex = 0;
        }
        return returnedObject;
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        int numberOfObjectsThatWillExisitAtOnce = Mathf.CeilToInt((splineSequence.totalLength / speed) / timeBetweenSpawns) + 1;
        gameObjectsMoved = new GameObject[numberOfObjectsThatWillExisitAtOnce];
        currentTValues = new float[numberOfObjectsThatWillExisitAtOnce];
        spawnTimer = new L7Games.Timer(timeBetweenSpawns);
        spawnTimer.isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer.Tick(Time.deltaTime);
        if (!spawnTimer.isActive)
        {
            SpawnObject();
        }
        UpdateTIncrement();
        for (int i = 0; i < gameObjectsMoved.Length; ++i)
        {
            UpdateMovedObject(i);
        }

    }

    //private IEnumerator waitDebug(float seconds)
    //{

    //    yield return new WaitForSeconds(10.0f);
    //    UpdateTIncrement();
    //    Debug.Log("Time Delay: " + seconds.ToString());
    //    currentTValue += tIncrementPerSecond * seconds;
    //    currentTValue = Mathf.Clamp01(currentTValue);
    //    if (loop)
    //    {
    //        if (tIncrementPerSecond > 0f)
    //        {
    //            if (currentTValue == 1f)
    //            {
    //                currentTValue = 0f;
    //            }
    //        }
    //        else if (tIncrementPerSecond < 0f)
    //        {
    //            if (currentTValue == 0f)
    //            {
    //                currentTValue = 1f;
    //            }
    //        }
    //    }
    //}
    #endregion
}
