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
public class LineSpline : Spline
{
    public override Vector3 GetPointAtTime(float t)
    {
        return Vector3.Lerp(WorldStartPoint, WorldEndPoint, t);
    }

    public override float GetTotalLength()
    {
        return Vector3.Distance(WorldStartPoint, WorldEndPoint);
    }

    public void OnDrawGizmos()
    {
        // Drawing the 2 points and a line between them
        UpdateWorldPositions();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(WorldStartPoint, 0.1f);
        Gizmos.DrawSphere(WorldEndPoint, 0.1f);
        Gizmos.DrawLine(WorldStartPoint, WorldEndPoint);
    }
}
