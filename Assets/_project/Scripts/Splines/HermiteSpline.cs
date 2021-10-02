//===========================================================================================================================================================================================================================================================================
// Name:                HermiteSpline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
// Brief:               A spline containing additional tangents for the start and end point that dictate the curve of the spline
//============================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A spline containing additional tangents for the start and end point that dictate the curve of the spline
/// </summary>
public class HermiteSpline : Spline
{
    #region Public Variables
    /// <summary>
    /// The tangent connected to the start point local to the GameObject transform (property accessor)
    /// </summary>
    [SerializeField]
    [Tooltip("The tangent connected to the start point local to the GameObject transform")]
    private Vector3 startTangent;
    /// <summary>
    /// The tangent connected to the start point local to the GameObject transform (property accessor)
    /// </summary>
    public Vector3 StartTangent
    {
        get
        {
            return startTangent;
        }
        set
        {
            startTangent = value;
        }
    }


    /// <summary>
    /// The tangent connected to the end point local to the GameObject transform
    /// </summary>
    [SerializeField]
    [Tooltip("The tangent connected to the end point local to the start point")]
    private Vector3 endTangent;
    /// <summary>
    /// The tangent connected to the end point local to the GameObject transform (property accessor)
    /// </summary>
    public Vector3 EndTangent
    {
        get
        {
            return endTangent;
        }
        set
        {
            endTangent = value;
        }
    }
    #endregion

    #region Serialized Private Fields
    /// <summary>
    /// The name of positions that are sampled along the length of the spline to calculate its distance
    /// </summary>
    [SerializeField] [Min(float.Epsilon)] [Tooltip("The name of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0")]
    private float distancePrecision = 100f;
    #endregion

    #region Private Variables
    /// <summary>
    /// The total length the spline covers
    /// </summary>
    private float totalLength;
    #endregion

    #region Public Methods
    public override float GetTotalLength()
    {
        return totalLength;
    }

    public override Vector3 GetPointAtTime(float t)
    {
        // Making sure t is within parameters
        t = Mathf.Clamp01(t);

        // Hermite Spline Calculation
        float tsq = t * t;
        float tcub = tsq * t;

        float h00 = 2 * tcub - 3 * tsq + 1;
        float h01 = -2 * tcub + 3 * tsq;
        float h10 = tcub - 2 * tsq + t;
        float h11 = tcub - tsq;

        return h00 * WorldStartPoint + h10 * startTangent + h01 * WorldEndPoint + h11 * endTangent;
    }

    public void ConnectToStartOfHermiteSpline(Vector3 startPoint, Vector3 startTangent)
    {
        WorldEndPoint = startPoint;
        endTangent = startTangent;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Updates the total length to be correct based on the current spline
    /// </summary>
    private void UpdateLength()
    {
        float distance = 0.0f;

        // Sampling a given number of points to get the distance between them to get the whole length of the spline
        Vector3 lastPosition = StartPoint;
        float tIncrement = 1.0f / distancePrecision;
        for (float t = 0; t <= 1f; t += tIncrement)
        {
            Vector3 newPosition = GetPointAtTime(t);
            distance += Vector3.Distance(newPosition, lastPosition);
            lastPosition = newPosition;
        }
        totalLength = distance;
    }
    #endregion

    #region Unity Methods
    private void OnDrawGizmos()
    {
        UpdateWorldPositions();

        // Drawing start point and tangent 
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(WorldStartPoint, 0.1f);
        Gizmos.DrawCube(WorldStartPoint + startTangent, Vector3.one * 0.1f);
        Gizmos.DrawLine(WorldStartPoint, WorldStartPoint + startTangent);

        // Drawing end Point and tangent
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(WorldEndPoint, 0.1f);
        Gizmos.DrawCube(WorldEndPoint + endTangent, Vector3.one * 0.1f);
        Gizmos.DrawLine(WorldEndPoint, WorldEndPoint + endTangent);

        // Drawing line between all the sampled points to represent the spline
        float tIncrement = 1.0f / distancePrecision;
        for (float f = 0.0f; f < 1f;)
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.red, f);
            Gizmos.DrawLine(GetPointAtTime(f), GetPointAtTime(f += tIncrement));
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateLength();
    }

    protected override void Start()
    {
        base.Start();
        UpdateLength();
    }
    #endregion












}
