using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using SleepyCat.Utility.Splines;

public class BezierCurveTests
{
    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [Test]
    public void GetPointAtTime1Point()
    {
        BezierCurve curve = new BezierCurve();
        curve.StartPosition = new Vector3(-4f, -4f, 1);
        curve.FirstControlPoint = new Vector3(-2f, 4f, 8f);
        curve.EndPosition = new Vector3(2f, -4f, 5f);
        curve.SetIsTwoControlPoint(false);

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
        BezierCurve curve = new BezierCurve();
        curve.StartPosition = new Vector3(-4f, -4f, 1);
        curve.EndPosition = new Vector3(4, 4f, 2f);
        curve.SetIsTwoControlPoint(true);
        curve.FirstControlPoint = new Vector3(-2f, 4f, 8f);
        curve.SecondControlPoint = new Vector3(2f, -4f, 5f);

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

    [Test]
    public void TestGet1PointLength()
    {
        BezierCurve curve = new BezierCurve();
        curve.SetIsTwoControlPoint(false);
        curve.StartPosition = new Vector3(0f, 0f, 10f);
        curve.EndPosition = new Vector3(0, 0f, 0f);
        curve.FirstControlPoint = new Vector3(0f, 0f, 5f);
        for (curve.distancePrecision = 5f; curve.distancePrecision <= 25f; curve.distancePrecision += 0.25f)
        {
            Assert.AreEqual(10f, Mathf.Round(curve.GetTotalLength()));
        }

    }

    [Test]
    public void TestGet2PointLength()
    {
        BezierCurve curve = new BezierCurve();
        curve.SetIsTwoControlPoint(true);
        curve.StartPosition = new Vector3(0f, 0f, 10f);
        curve.EndPosition = new Vector3(0, 0f, 0f);
        curve.FirstControlPoint = new Vector3(0f, 0f, 5f);
        curve.SecondControlPoint = new Vector3(0f, 0f, 5f);
        for (curve.distancePrecision = 5f; curve.distancePrecision <= 25f; curve.distancePrecision += 0.25f)
        {
            Assert.AreEqual(10f, Mathf.Round(curve.GetTotalLength()));
        }
    }

    private Vector3 RoundVectorComponentsToDecimalPrecision(Vector3 orginalVector, int numberOfDecimals)
    {
        return new Vector3((float)System.Math.Round(orginalVector.x, numberOfDecimals), (float)System.Math.Round(orginalVector.y, numberOfDecimals), (float)System.Math.Round(orginalVector.z, numberOfDecimals));
    }
}
