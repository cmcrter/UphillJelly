//===========================================================================================================================================================================================================================================================================
// Name:                LineSplineTests.cs
// Author:              Matthew Mason
// Date Created:        16-Nov-2021
// Date Last Modified:  16-Nov-2021
// Last Modified By:    Matthew Mason
// Brief:               A class containing tests for the line spline
//============================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using L7Games.Utility.Splines;


public class LineSplineWrapperTests
{
    [UnityTest]
    public IEnumerator TestSettingLocalStartPosition()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingLocalStartPosition(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingLocalEndPosition()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingLocalEndPosition(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionStartWithoutLocal()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionStartWithoutLocal(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionEndWithoutLocal()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionEndWithoutLocal(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionStartWithLocal()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionStartWithLocal(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionEndWithLocal()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionEndWithLocal(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestUpdatingWorldPositions()
    {
        LineSplineWrapper lineSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestUpdatingWorldPositions(lineSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestGetTotalLength()
    {
        LineSplineWrapper lineSpline = CreateTestedComponent();
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(10f, 0, 0f));
        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(-10f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetTotalLength(), 20f);

        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(3f, 4f, 0f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(0f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetTotalLength(), 5f);

        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(0f, 6f, 8f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(0f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetTotalLength(), 10f);
        return null;
    }

    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [UnityTest]
    public IEnumerator TestGetPointAtTime()
    {
        LineSplineWrapper lineSpline = CreateTestedComponent();
        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(10f, 0, 0f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(-10f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.0f), new Vector3(10f, 0, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.25f), new Vector3(5f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.5f), new Vector3(0f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.75f), new Vector3(-5f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(1f), new Vector3(-10f, 0f, 0f));

        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(3f, 4f, 0f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(0f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.0f), new Vector3(3f, 4f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.25f), new Vector3(2.25f, 3f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.5f), new Vector3(1.5f, 2f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.75f), new Vector3(0.75f, 1f, 0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(1f), new Vector3(0f, 0f, 0f));

        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(0f, 0.0f, 0.0f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(0f, 6f, 8f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.0f), new Vector3(0f, 0.0f, 0.0f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.25f), new Vector3(0f, 1.5f, 2f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.5f), new Vector3(0f, 3f, 4f));
        Assert.AreEqual(lineSpline.GetPointAtTime(0.75f), new Vector3(0f, 4.5f, 6f));
        Assert.AreEqual(lineSpline.GetPointAtTime(1f), new Vector3(0f, 6f, 8f));
        return null;
    }

    /// <summary>
    /// Test for the get direction function
    /// </summary>
    [UnityTest]
    public IEnumerator TestGetDirection()
    {
        LineSplineWrapper lineSpline = CreateTestedComponent();
        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(10f, 0, 0f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(-10f, 0f, 0f));
        Assert.AreEqual(lineSpline.GetDirection(0.0f), new Vector3(-1f, 0.0f, 0.0f));
        Assert.AreEqual(lineSpline.GetDirection(0.5f), new Vector3(-1f, 0.0f, 0.0f));
        Assert.AreEqual(lineSpline.GetDirection(1.0f), new Vector3(-1f, 0.0f, 0.0f));

        lineSpline.SetWorldStartPointAndUpdateLocal(new Vector3(0f, 0.0f, 0.0f));
        lineSpline.SetWorldEndPointAndUpdateLocal(new Vector3(0f, 6f, 8f));
        Assert.AreEqual(lineSpline.GetDirection(0.0f), new Vector3(0.0f, 0.6f, 0.8f));
        Assert.AreEqual(lineSpline.GetDirection(0.5f), new Vector3(0.0f, 0.6f, 0.8f));
        Assert.AreEqual(lineSpline.GetDirection(1.0f), new Vector3(0.0f, 0.6f, 0.8f));
        return null;
    }

    private LineSplineWrapper CreateTestedComponent()
    {
        return new GameObject().AddComponent<LineSplineWrapper>();
    }
}
