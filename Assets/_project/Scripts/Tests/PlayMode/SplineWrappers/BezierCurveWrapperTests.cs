using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using SleepyCat.Utility.Splines;

public class BezierCurveWrapperTests
{
    [UnityTest]
    public IEnumerator TestSettingLocalStartPosition()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingLocalStartPosition(BezierCurveSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingLocalEndPosition()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingLocalEndPosition(BezierCurveSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionStartWithoutLocal()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionStartWithoutLocal(BezierCurveSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionEndWithoutLocal()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionEndWithoutLocal(BezierCurveSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionStartWithLocal()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionStartWithLocal(BezierCurveSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestSettingWorldPositionEndWithLocal()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestSettingWorldPositionEndWithLocal(BezierCurveSplineWrapper);
        return null;
    }

    [UnityTest]
    public IEnumerator TestUpdatingWorldPositions()
    {
        BezierCurveWrapper BezierCurveSplineWrapper = CreateTestedComponent();
        SplineWrapperTests.TestUpdatingWorldPositions(BezierCurveSplineWrapper);
        return null;
    }

    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [Test]
    public void GetPointAtTime1Point()
    {
        BezierCurveWrapper curve = CreateTestedComponent();
        curve.SetWorldStartPointAndUpdateLocal(new Vector3(-4f, -4f, 1));
        curve.SetWorldControlPoint(BezierCurve.firstControlPointIndex, new Vector3(-2f, 4f, 8f));
        curve.SetWorldEndPointAndUpdateLocal(new Vector3(2f, -4f, 5f));
        curve.IsUsingTwoControlPoints = false;

        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.0f), 3));
        Assert.AreEqual(new Vector3(-2.875f, -1f, 3.875f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.25f), 3));
        Assert.AreEqual(new Vector3(-1.5f, 0f, 5.5f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.5f), 3));
        Assert.AreEqual(new Vector3(0.125f, -1f, 5.875f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.75f), 3));
        Assert.AreEqual(new Vector3(2f, -4f, 5f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(1.0f), 3));

        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(-5f), 3));
        Assert.AreEqual(new Vector3(2f, -4f, 5f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(20.2f), 3));
    }

    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [Test]
    public void GetPointAtTime2Point()
    {
        BezierCurveWrapper curve = CreateTestedComponent();
        curve.SetWorldStartPointAndUpdateLocal(new Vector3(-4f, -4f, 1));
        curve.SetWorldEndPointAndUpdateLocal(new Vector3(4, 4f, 2f));
        curve.IsUsingTwoControlPoints = true;
        curve.SetWorldControlPoint(BezierCurve.firstControlPointIndex, new Vector3(-2f, 4f, 8f));
        curve.SetWorldControlPoint(BezierCurve.secondControlPointIndex, new Vector3(2f, -4f, 5f));

        // Valid Inputs
        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.0f), 3));
        Assert.AreEqual(new Vector3(-2.188f, -0.5f, 4.531f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.25f), 3));
        Assert.AreEqual(new Vector3(0f, 0f, 5.25f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.5f), 3));
        Assert.AreEqual(new Vector3(2.188f, 0.5f, 4.094f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.75f), 3));
        Assert.AreEqual(new Vector3(4f, 4f, 2f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(1.0f), 3));

        // Invalid Inputs
        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(-5f), 3));
        Assert.AreEqual(new Vector3(4f, 4f, 2f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(20.2f), 3));
    }

    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [Test]
    public void GetLocalPointAtTime1Point()
    {
        BezierCurveWrapper curve = CreateTestedComponent();

        curve.gameObject.transform.position = Vector3.one;
        curve.LocalStartPosition = new Vector3(-4f, -4f, 1);
        curve.SetLocalControlPointValues(BezierCurve.firstControlPointIndex, new Vector3(-2f, 4f, 8f));
        curve.LocalEndPosition = new Vector3(2f, -4f, 5f);
        curve.IsUsingTwoControlPoints = false;

        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.0f), 3));
        Assert.AreEqual(new Vector3(-2.875f, -1f, 3.875f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.25f), 3));
        Assert.AreEqual(new Vector3(-1.5f, 0f, 5.5f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.5f), 3));
        Assert.AreEqual(new Vector3(0.125f, -1f, 5.875f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.75f), 3));
        Assert.AreEqual(new Vector3(2f, -4f, 5f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(1.0f), 3));

        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(-5f), 3));
        Assert.AreEqual(new Vector3(2f, -4f, 5f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(20.2f), 3));
    }

    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [Test]
    public void GetLocalPointAtTime2Point()
    {
        BezierCurveWrapper curve = CreateTestedComponent();

        curve.gameObject.transform.position = Vector3.one;
        curve.LocalStartPosition = new Vector3(-4f, -4f, 1);
        curve.LocalEndPosition = new Vector3(4, 4f, 2f);
        curve.IsUsingTwoControlPoints = true;
        curve.SetLocalControlPointValues(BezierCurve.firstControlPointIndex, new Vector3(-2f, 4f, 8f));
        curve.SetLocalControlPointValues(BezierCurve.secondControlPointIndex, new Vector3(2f, -4f, 5f));

        // Valid Inputs
        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.0f), 3));
        Assert.AreEqual(new Vector3(-2.188f, -0.5f, 4.531f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.25f), 3));
        Assert.AreEqual(new Vector3(0f, 0f, 5.25f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.5f), 3));
        Assert.AreEqual(new Vector3(2.188f, 0.5f, 4.094f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(0.75f), 3));
        Assert.AreEqual(new Vector3(4f, 4f, 2f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(1.0f), 3));

        // Invalid Inputs
        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(-5f), 3));
        Assert.AreEqual(new Vector3(4f, 4f, 2f), RoundVectorComponentsToDecimalPrecision(curve.GetLocalPointAtTime(20.2f), 3));
    }

    [Test]
    public void TestGet1PointLength()
    {
        BezierCurveWrapper curve = CreateTestedComponent();
        curve.IsUsingTwoControlPoints = false;
        curve.SetWorldStartPointAndUpdateLocal(new Vector3(0f, 0f, 10f));
        curve.SetWorldEndPointAndUpdateLocal(new Vector3(0, 0f, 0f));
        curve.SetWorldControlPoint(BezierCurve.firstControlPointIndex, new Vector3(0f, 0f, 5f));
        for (curve.DistancePrecision = 5f; curve.DistancePrecision <= 25f; curve.DistancePrecision += 0.25f)
        {
            Assert.AreEqual(10f, Mathf.Round(curve.GetTotalLength()));
        }

    }

    [Test]
    public void TestGet2PointLength()
    {
        BezierCurveWrapper curve = CreateTestedComponent();
        curve.IsUsingTwoControlPoints = false;
        curve.SetWorldStartPointAndUpdateLocal(new Vector3(0f, 0f, 10f));
        curve.SetWorldEndPointAndUpdateLocal(new Vector3(0, 0f, 0f));
        curve.SetWorldControlPoint(BezierCurve.firstControlPointIndex, new Vector3(0f, 0f, 5f));
        curve.SetWorldControlPoint(BezierCurve.secondControlPointIndex, new Vector3(0f, 0f, 5f));
        for (curve.DistancePrecision = 5f; curve.DistancePrecision <= 25f; curve.DistancePrecision += 0.25f)
        {
            Assert.AreEqual(10f, Mathf.Round(curve.GetTotalLength()));
        }
    }

    private BezierCurveWrapper CreateTestedComponent()
    {
        return new GameObject().AddComponent<BezierCurveWrapper>();
    }

    private Vector3 RoundVectorComponentsToDecimalPrecision(Vector3 orginalVector, int numberOfDecimals)
    {
        return new Vector3((float)System.Math.Round(orginalVector.x, numberOfDecimals), (float)System.Math.Round(orginalVector.y, numberOfDecimals), (float)System.Math.Round(orginalVector.z, numberOfDecimals));
    }
}
