//===========================================================================================================================================================================================================================================================================
// Name:                LineSpline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
// Brief:               A child of the spline component that is actually a line between 2 points
//============================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A child of the spline component that is actually a line between 2 points
/// </summary>
[System.Serializable]
public class LineSpline : Spline
{
    [SerializeField]
    private Vector3 startPoint;
    [SerializeField]
    private Vector3 endPoint;

    public override Vector3 GetEndPoint()
    {
        return endPoint;
    }

    public override Vector3 GetPointAtTime(float t)
    {
        return Vector3.Lerp(startPoint, endPoint, t);
    }

    public override Vector3 GetStartPoint()
    {
        return startPoint;
    }

    public override float GetTotalLength()
    {
        return Vector3.Distance(startPoint, endPoint);
    }

    public override void SetEndPoint(Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }

    public override void SetStartPoint(Vector3 startPoint)
    {
        this.startPoint = startPoint;
    }
}
