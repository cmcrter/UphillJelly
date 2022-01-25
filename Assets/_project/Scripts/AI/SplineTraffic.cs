using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTraffic : MonoBehaviour
{
    #region Structures
    /// <summary>
    /// A structure holding information about a prefab and its weighting for spawning and it upwards positioning offset
    /// </summary>
    [System.Serializable]
    public struct SpawnablePrefab
    {
        /// <summary>
        /// The prefab to instantiate
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// How much it is weight towards spawn this object 
        /// </summary>
        public float spawnWeight;
        /// <summary>
        /// The upwards offset 
        /// </summary>
        public float upwardsOffset;
    }

    /// <summary>
    /// A structure holding information 
    /// </summary>
    private struct MovingObject
    {
        /// <summary>
        /// The GameObject that is being moved
        /// </summary>
        public GameObject gameObjectMoving;
        /// <summary>
        /// How far above the spline point it should be moved in its upwards directions
        /// </summary>
        public float upwardsOffset;
    }
    #endregion

    #region Serialized Private Fields
    [SerializeField]
    [Tooltip("If the object should follow the x position of the spline")]
    private bool followX = false;
    [SerializeField]
    [Tooltip("If the object should follow the y position of the spline")]
    private bool followY = false;
    [SerializeField]
    [Tooltip("If the object should follow the z position of the spline")]
    private bool followZ = false;

    [SerializeField]
    [Tooltip("How fast the object will move along the spline")]
    private float speed = 1.0f;
    [SerializeField] 
    [Tooltip("The amount of time between spawning traffic objects")]
    private float timeBetweenSpawns = 1f;

    [SerializeField] 
    [Tooltip("The prefabs that can be spawned to move along the splines")]
    private SpawnablePrefab[] spawnablePrefabs;

    [SerializeField]
    [Tooltip("The spline wrapper to follow along")]
    private L7Games.Utility.Splines.SplineWrapper splineInUse = null;
    #endregion

    #region Private Variables
    /// <summary>
    /// The unit interval for how far along the spline sequence it is
    /// </summary>
    private float[] currentTValues;
    /// <summary>
    /// How much the unit interval to position along the spline increases every frame
    /// </summary>
    private float tIncrementPerSecond = Mathf.Infinity;

    /// <summary>
    /// The next index in the objects moved array that is free to spawn a new object 
    /// </summary>
    private int nextFreeIndex = 0;

    /// <summary>
    /// The object that are currently being moved along the spline
    /// </summary>
    private MovingObject[] objectsMoved;

    /// <summary>
    /// The timer tracking how long until the next object can be spawned
    /// </summary>
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

    /// <summary>
    /// The number of spawn-able prefab
    /// </summary>
    public int SpawnablePrefabsCount 
    {
        get
        {
            if (spawnablePrefabs == null)
            {
                spawnablePrefabs = new SpawnablePrefab[0];
            }
            return spawnablePrefabs.Length;
        }
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Returns the prefab weighting for a spawn able prefab at the given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetPrefabWeightAtIndex(int index)
    {
        if (index < 0 || index >= spawnablePrefabs.Length)
        {
            int i;
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
        return spawnablePrefabs[index].spawnWeight;
    }

    public void SetPrefabWeightAtIndex(int index, float newValue)
    {
        if (index < 0 || index >= spawnablePrefabs.Length)
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
        spawnablePrefabs[index].spawnWeight = newValue;
    }

    public GameObject GetPrefabAtIndex(int index)
    {
        if (index < 0 || index >= spawnablePrefabs.Length)
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
        return spawnablePrefabs[index].prefab;
    }

    public void SetPrefabAtIndex(int index, GameObject newPrefab)
    {
        if (index < 0 || index >= spawnablePrefabs.Length)
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
        spawnablePrefabs[index].prefab = newPrefab;
    }

    public void AdjustPrefabsAndWeightsCount(int newCount)
    {
        SpawnablePrefab[] newPrefabArray = new SpawnablePrefab[newCount];
        System.Array.Copy(spawnablePrefabs, newPrefabArray, Mathf.Min(spawnablePrefabs.Length, newCount));
        spawnablePrefabs = newPrefabArray;
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
        splineInUse = splineSequenceToFollow;
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
        if (splineInUse != null)
        {
            tIncrementPerSecond = speed / splineInUse.GetTotalLength();
        }
    }

    private void UpdateMovedObject(int indexMoved)
    {
        if (objectsMoved == null)
        {
            return;
        }

        if (objectsMoved[indexMoved].gameObjectMoving == null)
        {
            return;
        }

        float newTChange = Time.deltaTime * tIncrementPerSecond;
        if (currentTValues[indexMoved] + newTChange < 1)
        {
            // Clamping it at the max value and min values of a unit interval

            // Check the length of the next increment
            Vector3 nextPoint = splineInUse.GetPointAtTime(currentTValues[indexMoved] + Time.deltaTime * tIncrementPerSecond);
            Vector3 currentPoint = splineInUse.GetPointAtTime(currentTValues[indexMoved]);
            Vector3 velocity = nextPoint - currentPoint;


            // Ideally the distance change should be speed * time.deltaTime
            float desiredDistance = speed * Time.deltaTime;
            float currentDistanceChange = velocity.magnitude;

            float desiredChange = desiredDistance / currentDistanceChange;
            newTChange = Time.deltaTime * tIncrementPerSecond * desiredChange;
            currentTValues[indexMoved] = Mathf.Clamp01(currentTValues[indexMoved] + newTChange); // add length to this calculation
        }
        else
        {
            currentTValues[indexMoved] = Mathf.Clamp01(currentTValues[indexMoved] + newTChange); // add length to this calculation
        }

        // Destroy the object when it reaches the end


        if (tIncrementPerSecond > 0f)
        {
            if (currentTValues[indexMoved] == 1f)
            {
                currentTValues[indexMoved] = 0f;
                GameObject.Destroy(objectsMoved[indexMoved].gameObjectMoving);
                return;
            }
        }

        // Setting the required position values to given time
        if (splineInUse != null)
        {
            Vector3 targetPosition = splineInUse.GetPointAtTime(currentTValues[indexMoved]);
            if (!followX)
            {
                targetPosition.x = objectsMoved[indexMoved].gameObjectMoving.transform.position.x;
            }
            if (!followY)
            {
                targetPosition.y = objectsMoved[indexMoved].gameObjectMoving.transform.position.y;
            }
            if (!followZ)
            {
                targetPosition.z = objectsMoved[indexMoved].gameObjectMoving.transform.position.z;
            }
            //gameObjectsMoved[indexMoved].transform.forward = splineInUse.GetDirection(currentTValues[indexMoved], newTChange);
            objectsMoved[indexMoved].gameObjectMoving.transform.position = targetPosition;
            objectsMoved[indexMoved].gameObjectMoving.transform.position += objectsMoved[indexMoved].gameObjectMoving.transform.up * objectsMoved[indexMoved].upwardsOffset;
            objectsMoved[indexMoved].gameObjectMoving.transform.forward = splineInUse.GetDirection(newTChange);
        }
        else
        {
            Debug.LogError("SplineSequence not assigned when referenced in Spline Mover update function", gameObject);
        }

        //transform.forward = splineInUse.GetDirection(currentTValues[indexMoved], 0.01f);
    }

    private GameObject SpawnObject()
    {
        spawnTimer = new L7Games.Timer(timeBetweenSpawns);
        int randomValue = GetWeightedPrefabIndexToSpawn();
        Debug.Log(randomValue);
        GameObject returnedObject  = GameObject.Instantiate(spawnablePrefabs[randomValue].prefab);
        returnedObject.transform.position = splineInUse.WorldStartPosition;
        objectsMoved[nextFreeIndex].gameObjectMoving = returnedObject;
        objectsMoved[nextFreeIndex].upwardsOffset = spawnablePrefabs[randomValue].upwardsOffset;
        currentTValues[nextFreeIndex] = 0f;
        ++nextFreeIndex;
        if (nextFreeIndex == objectsMoved.Length)
        {
            nextFreeIndex = 0;
        }
        return returnedObject;
    }

    private int GetWeightedPrefabIndexToSpawn()
    {
        float currentLowerBounds = 0f;

        float weightsTotals = 0f;
        for (int i = 0; i < spawnablePrefabs.Length; ++i)
        {
            weightsTotals += spawnablePrefabs[i].spawnWeight;
        }
        float randomValue = Random.Range(0.0f, weightsTotals);
        for (int i = 0; i < spawnablePrefabs.Length; ++i)
        {
            if (randomValue <= currentLowerBounds + spawnablePrefabs[i].spawnWeight)
            {
                return i;
            }
            currentLowerBounds += spawnablePrefabs[i].spawnWeight;
        }
        return spawnablePrefabs.Length - 1;
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        int numberOfObjectsThatWillExisitAtOnce = Mathf.CeilToInt((splineInUse.GetTotalLength() / speed) / timeBetweenSpawns) + 1;
        objectsMoved = new MovingObject[numberOfObjectsThatWillExisitAtOnce];
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
        for (int i = 0; i < objectsMoved.Length; ++i)
        {
            UpdateMovedObject(i);
        }

    }

    private void OnValidate()
    {
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
