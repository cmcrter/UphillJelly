//===========================================================================================================================================================================================================================================================================
// Name:                SplineSequence.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
// Brief:               A component for getting a sequence of splines and lerping through them
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.Splines;

    /// <summary>
    /// A component for getting a sequence of splines and lerping through them
    /// </summary>
    public class SplineSequence : SplineWrapper
    {
    #region Serialized Private Fields
    /// <summary>
    /// If checked it will make sure that the ends of each spline connect to the start of the next one
    /// </summary>
    [SerializeField] [Tooltip("If checked it will make sure that the ends of each spline connect to the start of the next one")]
    private bool connectEnds = false;

    /// <summary>
    /// If checked it will make sure that the end of the last spline connects to the start of the first one
    /// </summary>
    [SerializeField] [Tooltip("If checked it will make sure that the end of the last spline connects to the start of the first one")]
    private bool isCircuit = false;

    /// <summary>
    /// The different splines contained within the sequence
    /// </summary>
    [SerializeField]
    [Tooltip("The different splines contained within the sequence")]
    private List<SplineWrapper> containedSplines = new List<SplineWrapper>();
    #endregion

    #region Properties
    #region Overrides
    public override Vector3 LocalEndPosition
    {
        get
        {
            if (containedSplines.Count > 0)
            {
                if (containedSplines[containedSplines.Count - 1] != null)
                {
                    return transform.InverseTransformPoint(containedSplines[containedSplines.Count - 1].WorldEndPosition);
                }
            }
            return Vector3.zero;
        }
        set
        {
            if (containedSplines.Count > 0)
            {
                if (containedSplines[containedSplines.Count - 1] != null)
                {
                    containedSplines[containedSplines.Count - 1].SetWorldEndPointAndUpdateLocal(transform.TransformPoint(value));
                }
            }
        }
    }
    public override Vector3 LocalStartPosition
    {
        get
        {
            if (containedSplines.Count > 0)
            {
                if (containedSplines[0] != null)
                {
                    return transform.InverseTransformPoint(containedSplines[0].WorldStartPosition);
                }
            }
            return Vector3.zero;
        }
        set
        {
            if (containedSplines.Count > 0)
            {
                if (containedSplines[0] != null)
                {
                    containedSplines[containedSplines.Count - 1].SetWorldStartPointWithoutLocal(transform.TransformPoint(value));
                }
            }
        }
    }
    public override Vector3 WorldEndPosition
    {
        get
        {
            if (containedSplines.Count > 0)
            {
                if (containedSplines[containedSplines.Count - 1] != null)
                {
                    return containedSplines[containedSplines.Count - 1].WorldEndPosition;
                }
            }
            return transform.position;
        }
    }
    public override Vector3 WorldStartPosition
    {
        get
        {
            if (containedSplines.Count > 0)
            {
                if (containedSplines[0] != null)
                {
                    return containedSplines[0].WorldStartPosition;
                }
            }
            return transform.position;
        }
    }
    #endregion
    /// <summary>
    /// The total length of all the splines combined
    /// </summary>
    public float totalLength
    {
        get;
        private set;
    }
    #endregion

    #region Public Methods
    #region Overrides
    public override float GetTotalLength()
    {
        return totalLength;
    }

    public override Vector3 GetLocalPointAtTime(float t)
    {
        return transform.InverseTransformPoint(GetLengthBasedPoint(t));
    }

    public override Vector3 GetPointAtTime(float t)
    {
        return GetLengthBasedPoint(t);
    }

    public override void SetWorldEndPointAndUpdateLocal(Vector3 endPoint)
    {
        if (containedSplines.Count > 0)
        {
            containedSplines[containedSplines.Count - 1].SetWorldEndPointAndUpdateLocal(endPoint);
        }
    }

    public override void SetWorldStartPointAndUpdateLocal(Vector3 startPoint)
    {
        if (containedSplines.Count > 0)
        {
            containedSplines[0].SetWorldStartPointAndUpdateLocal(startPoint);
        }
    }
    public override void SetWorldEndPointWithoutLocal(Vector3 endPoint)
    {
        if (containedSplines.Count > 0)
        {
            if (containedSplines[0] != null)
            {
                containedSplines[0].SetWorldEndPointWithoutLocal(endPoint);
            }
        }
    }

    public override void SetWorldStartPointWithoutLocal(Vector3 startPoint)
    {
        if (containedSplines.Count > 0)
        {
            if (containedSplines[0] != null)
            {
                containedSplines[0].SetWorldStartPointWithoutLocal(startPoint);
            }
        }
    }

    public override void UpdateWorldPositions()
    {
        UpdateSplines();
    }

    public override Vector3 GetDirection(float t)
    {
        int index = GetIndexOfSplineAtT(t, out float splineTValue);
        if (index > -1)
        {
            return containedSplines[index].GetDirection(splineTValue);
        }
        return base.GetDirection(t);
    }

    public override Vector3 GetDirection(float t, float stepDistance)
    {
        int index = GetIndexOfSplineAtT(t, out float splineTValue);
        if (index > -1)
        {
            return containedSplines[index].GetDirection(splineTValue, stepDistance);
        }
        return base.GetDirection(t, stepDistance);
    }
    #endregion

    /// <summary>
    /// Returns how many spline are within the this sequence
    /// </summary>
    /// <returns>How many spline are within the this sequence</returns>
    public int GetNumberOfSplines()
    {
        return containedSplines.Count;
    }

    public int GetIndexOfSplineAtT(float t)
    {
        // Ensuring t is a unit interval
        t = Mathf.Clamp01(t);

        // Using t to find the length along the spline sequence to aim for
        float targetLength = totalLength * t;

        for (int index = 0; index < containedSplines.Count; ++index)
        {
            if (containedSplines[index] != null)
            {
                float sectionDistance = containedSplines[index].GetTotalLength();
                // Checking if the target length shorter than this spline
                if (targetLength - sectionDistance < 0f)
                {
                    return index;
                }
                // Otherwise remove of the length of the section from the target length
                else
                {
                    targetLength -= sectionDistance;
                }
            }
            else
            {
                Debug.LogWarning("A spline was null when getting length based point", gameObject);
            }
        }
        // Return the end point of the last spline if the target length was not within any of the splines
        if (containedSplines.Count > 0)
        {
            return containedSplines.Count - 1;
        }
        else
        {
            Debug.LogError("SplineSequence contained no splines when GetLengthBasedPoint was called", gameObject);
            return -1;
        }
    }

    public int GetIndexOfSplineAtT(float t, out float tOnSpline)
    {
        // Ensuring t is a unit interval
        t = Mathf.Clamp01(t);

        // Using t to find the length along the spline sequence to aim for
        float targetLength = totalLength * t;

        for (int index = 0; index < containedSplines.Count; ++index)
        {
            if (containedSplines[index] != null)
            {
                float sectionDistance = containedSplines[index].GetTotalLength();
                // Checking if the target length shorter than this spline
                if (targetLength - sectionDistance < 0f)
                {
                    tOnSpline = targetLength / sectionDistance;
                    return index; 
                }
                // Otherwise remove of the length of the section from the target length
                else
                {
                    targetLength -= sectionDistance;
                }
            }
            else
            {
                Debug.LogWarning("A spline was null when getting length based point", gameObject);
            }
        }
        // Return the end point of the last spline if the target length was not within any of the splines
        if (containedSplines.Count > 0)
        {
            tOnSpline = 1f;
            return containedSplines.Count - 1;
        }
        else
        {
            Debug.LogError("SplineSequence contained no splines when GetLengthBasedPoint was called", gameObject);
            tOnSpline = -1f;
            return -1;
        }
    }

    /// <summary>
    /// Returns a spline at the given index
    /// </summary>
    /// <param name="index">The index to get the spline at</param>
    /// <returns>A spline at the given index, will be null if it is out of range</returns>
    public SplineWrapper GetSpline(int index)
    {
        if (containedSplines.Count > index && index > 0)
        {
            return containedSplines[index];
        }
        return null;
    }

    /// <summary>
    /// Returns a point at the given unit interval along the set of splines, giving all the splines the same weighting in terms of length
    /// </summary>
    /// <param name="t">The unit interval for how far along the point is in the sequence</param>
    /// <returns>The point at the given unit interval along the set of splines assume, giving all the splines the same weighting in terms of length</returns>
    public Vector3 GetPoint(float t)
    {
        // Ensuring t is a unit interval
        t = Mathf.Clamp01(t);

        // Calculating which spline the point is on and how far it is along that spline
        int numberOfSections = containedSplines.Count - 1;
        float longT = (float)numberOfSections * t;  // Get the value for how many splines it is along the sequence
        int index = Mathf.FloorToInt(longT);        // Get the index for the spline that the point is on
        float newT = longT - (float)index;          // Get the unit interval for how far along the point on that spline

        if (containedSplines[index] != null)
        {
            return containedSplines[index].GetPointAtTime(newT); // Use this index and unit interval to get the point from the spline
        }
        else
        {
            Debug.Log("A spline at index " + index.ToString() + " was null when referenced in GetPoint method", gameObject);
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Returns a point at the given unit interval along the set of splines, weighting all the splines based on their total length
    /// </summary>
    /// <param name="t">The unit interval for how far along the point is in the sequence</param>
    /// <returns>The point at the given unit interval along the set of splines assume, weighting all the splines based on their total length</returns>
    public Vector3 GetLengthBasedPoint(float t)
    {
        // Ensuring t is a unit interval
        t = Mathf.Clamp01(t);

        // Using t to find the length along the spline sequence to aim for
        float targetLength = totalLength * t;

        for (int index = 0; index < containedSplines.Count; ++index)
        {
            if (containedSplines[index] != null)
            {
                float sectionDistance = containedSplines[index].GetTotalLength();
                // Checking if the target length shorter than this spline
                if (targetLength - sectionDistance < 0f)
                {
                    return containedSplines[index].GetPointAtTime(targetLength / sectionDistance);
                }
                // Otherwise remove of the length of the section from the target length
                else
                {
                    targetLength -= sectionDistance;
                }
            }
            else
            {
                Debug.LogWarning("A spline was null when getting length based point", gameObject);
            }
        }
        // Return the end point of the last spline if the target length was not within any of the splines
        if (containedSplines.Count > 0)
        {
            return containedSplines[containedSplines.Count - 1].WorldStartPosition;
        }
        else
        {
            Debug.LogError("SplineSequence contained no splines when GetLengthBasedPoint was called", gameObject);
            return Vector3.zero;
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Updates the positions of splines based on the given booleans and contained splines
    /// </summary>
    private void UpdateSplines()
    {
        if (connectEnds)
        {
            if (containedSplines.Count > 1)
            {
                for (int i = 0; i < containedSplines.Count - 1; ++i)
                {
                    if (containedSplines[i] != null && containedSplines[i + 1] != null)
                    {
                        containedSplines[i].SetWorldEndPointAndUpdateLocal(containedSplines[i + 1].WorldStartPosition);
                    }

                }
            }
        }
        if (isCircuit)
        {
            if (containedSplines.Count > 1)
            {
                if (containedSplines[containedSplines.Count - 1] != null && containedSplines[0] != null)
                {
                    containedSplines[containedSplines.Count - 1].SetWorldEndPointAndUpdateLocal(containedSplines[0].WorldStartPosition);
                }
            }
        }
        UpdateTotalDistance();
    }

    /// <summary>
    /// Update the total distance based on the contained splines
    /// </summary>
    private void UpdateTotalDistance()
    {
        totalLength = 0;
        for (int i = 0; i < containedSplines.Count; ++i)
        {
            if (containedSplines[i] != null)
            {
                totalLength += containedSplines[i].GetTotalLength();
            }
            else
            {
                Debug.LogWarning("A spline was null when updating distance", gameObject);
            }
        }
    }
    #endregion

    #region Unity Methods
    private void OnDrawGizmos()
    {
        UpdateSplines();
    }


    #endregion
}
