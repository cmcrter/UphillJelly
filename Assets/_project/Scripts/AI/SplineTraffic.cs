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
        /// The unit interval for how far along the spline sequence it is
        /// </summary>
        public float currentTValue;
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

    #region Public Functions
    /// <summary>
    /// Returns the prefab weighting for a spawn able prefab at the given index of the spawn-able prefabs array
    /// </summary>
    /// <param name="index">The index of the prefab to get the weighting of</param>
    /// <returns>The prefab weighting for a spawn able prefab at the given index of the spawn-able prefabs array</returns>
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

    /// <summary>
    /// Returns the prefab GameObject at the given index of the spawn-able prefabs array
    /// </summary>
    /// <param name="index">The index of the spawn-able prefabs array to get the prefab from</param>
    /// <returns>the prefab GameObject at the given index of the spawn-able prefabs array</returns>
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

    /// <summary>
    /// Adjust the currentTValue based on it moving over by a given number of seconds
    /// </summary>
    /// <param name="seconds">The number of seconds to offset the position based on</param>
    public void AddPositionBySeconds(float seconds, int indexToMove)
    {
        UpdateTIncrement();
        objectsMoved[indexToMove].currentTValue += tIncrementPerSecond * seconds;
    }
    /// <summary>
    /// Adjust the size of the spawn able prefabs array, copying any elements that fit into the new size
    /// </summary>
    /// <param name="newCount">The new number of element the spawn-able prefab array should be</param>
    public void AdjustSpawnablePrefabsCount(int newCount)
    {
        SpawnablePrefab[] newPrefabArray = new SpawnablePrefab[newCount];
        System.Array.Copy(spawnablePrefabs, newPrefabArray, Mathf.Min(spawnablePrefabs.Length, newCount));
        spawnablePrefabs = newPrefabArray;
    }

    /// <summary>
    /// Set GameObject prefab at index of the spawn-able prefabs array
    /// </summary>
    /// <param name="index">The index to change the game object array of</param>
    /// <param name="newPrefab">The new prefab to change to</param>
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

    /// <summary>
    /// Set the prefab spawning weight at the given index to the given value
    /// </summary>
    /// <param name="index">The index of the prefab to adjust the spawning weight of</param>
    /// <param name="newValue">The new value to set the new weight too</param>
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
    #endregion

    #region Private Functions
    /// <summary>
    /// Spawn a weighted randomly selected object to move along the spline
    /// </summary>
    /// <returns>The game object instantiated during the spawn</returns>
    private GameObject SpawnObject()
    {
        // Reset timer
        spawnTimer = new L7Games.Timer(timeBetweenSpawns);
        // Select and spawn random prefab
        int randomValue = GetWeightedPrefabIndexToSpawn();
        GameObject returnedObject  = GameObject.Instantiate(spawnablePrefabs[randomValue].prefab);
        returnedObject.transform.position = splineInUse.WorldStartPosition;
        
        // Add it to the moved objects
        objectsMoved[nextFreeIndex].gameObjectMoving = returnedObject;
        objectsMoved[nextFreeIndex].upwardsOffset = spawnablePrefabs[randomValue].upwardsOffset;

        // Reset the object position
        objectsMoved[nextFreeIndex].currentTValue = 0f;
        ++nextFreeIndex;
        if (nextFreeIndex == objectsMoved.Length)
        {
            nextFreeIndex = 0;
        }
        return returnedObject;
    }

    /// <summary>
    /// Returns a random index from the spawn able object array, taking into account the weighting into selection
    /// </summary>
    /// <returns>A random index from the spawn able object array, taking into account the weighting into selection</returns>
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

    /// <summary>
    /// Updates for a single object moving along the spline
    /// </summary>
    /// <param name="indexMoved">The index of the object to move along the spline</param>
    private void UpdateMovedObject(int indexMoved)
    {
        // The object or objects don't exist then there is nothing to move
        if (objectsMoved == null)
        {
            return;
        }

        if (objectsMoved[indexMoved].gameObjectMoving == null)
        {
            return;
        }

        float newTChange = Time.deltaTime * tIncrementPerSecond;
        if (objectsMoved[indexMoved].currentTValue + newTChange < 1)
        {
            // Check the length of the next increment
            Vector3 nextPoint = splineInUse.GetPointAtTime(objectsMoved[indexMoved].currentTValue + Time.deltaTime * tIncrementPerSecond);
            Vector3 currentPoint = splineInUse.GetPointAtTime(objectsMoved[indexMoved].currentTValue);
            Vector3 velocity = nextPoint - currentPoint;


            // Ideally the distance change should be speed * time.deltaTime
            float desiredDistance = speed * Time.deltaTime;
            float currentDistanceChange = velocity.magnitude;

            // adjust how far it actually moves based on it current speed
            float desiredChange = desiredDistance / currentDistanceChange;
            newTChange = Time.deltaTime * tIncrementPerSecond * desiredChange;
            objectsMoved[indexMoved].currentTValue = Mathf.Clamp01(objectsMoved[indexMoved].currentTValue + newTChange); // add length to this calculation
        }
        else
        {
            objectsMoved[indexMoved].currentTValue = Mathf.Clamp01(objectsMoved[indexMoved].currentTValue + newTChange); // add length to this calculation
        }

        // Destroy the object if it reached the end of the spline
        if (tIncrementPerSecond > 0f)
        {
            if (objectsMoved[indexMoved].currentTValue == 1f)
            {
                objectsMoved[indexMoved].currentTValue = 0f;
                GameObject.Destroy(objectsMoved[indexMoved].gameObjectMoving);
                return;
            }
        }

        // Setting the required position and forward values to calculated time
        if (splineInUse != null)
        {
            Vector3 targetPosition = splineInUse.GetPointAtTime(objectsMoved[indexMoved].currentTValue);
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

            objectsMoved[indexMoved].gameObjectMoving.transform.position = targetPosition;
            objectsMoved[indexMoved].gameObjectMoving.transform.position += objectsMoved[indexMoved].gameObjectMoving.transform.up * objectsMoved[indexMoved].upwardsOffset;
            objectsMoved[indexMoved].gameObjectMoving.transform.forward = splineInUse.GetDirection(newTChange);
        }
        else
        {
            Debug.LogError("SplineSequence not assigned when referenced in Spline Mover update function", gameObject);
        }
    }
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
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Set up the object pool to allow for all object that will exist along the spline at once
        int numberOfObjectsThatWillExisitAtOnce = Mathf.CeilToInt((splineInUse.GetTotalLength() / speed) / timeBetweenSpawns) + 1;
        objectsMoved = new MovingObject[numberOfObjectsThatWillExisitAtOnce];
        // Start the spawn timer 
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
    #endregion
}
