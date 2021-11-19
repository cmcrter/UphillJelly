using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using SleepyCat.Utility.Splines;

public class BezierCurveTests
{
    ///// <summary>
    ///// Test for the GetTotalLength function
    ///// </summary>
    //[Test]
    //public void GetTotalLength()
    //{
    //    LineSpline lineSpline = new LineSpline();
    //    lineSpline.StartPosition = new Vector3(10f, 0, 0f);
    //    lineSpline.EndPosition = new Vector3(-10f, 0f, 0f);
    //    Assert.AreEqual(lineSpline.GetTotalLength(), 20f);
    //}

    /// <summary>
    /// Tests for the get point at time function
    /// </summary>
    [Test]
    public void GetPointAtTime1Point()
    {
        BezierCurve curve = new BezierCurve();
        curve.StartPosition = new Vector3(-4f, -4f, 1);
        curve.controlPoints[0] = new Vector3(-2f, 4f, 8f);
        curve.EndPosition = new Vector3(2f, -4f, 5f);
        curve.SetIsTwoControlPoint(false);

        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.0f), 3));
        Assert.AreEqual(new Vector3(-2.875f, -1f, 3.875f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.25f), 3));
        Assert.AreEqual(new Vector3(-1.5f, 0f, 5.5f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.5f), 3));
        Assert.AreEqual(new Vector3(0.125f, -1f, 5.875f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.75f), 3));
        Assert.AreEqual(new Vector3(2f, -4f, 5f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(1.0f), 3));
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
        curve.controlPoints[0] = new Vector3(-2f, 4f, 8f);
        curve.controlPoints[1] = new Vector3(2f, -4f, 5f);
        Assert.AreEqual(new Vector3(-4f, -4f, 1f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.0f), 3));
        Assert.AreEqual(new Vector3(-2.188f, -0.5f, 4.531f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.25f), 3));
        Assert.AreEqual(new Vector3(0f, 0f, 5.25f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.5f), 3));
        Assert.AreEqual(new Vector3(2.188f, 0.5f, 4.094f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(0.75f), 3));
        Assert.AreEqual(new Vector3(4f, 4f, 2f), RoundVectorComponentsToDecimalPrecision(curve.GetPointAtTime(1.0f), 3));
    }

    [Test]
    public void TestGetLength()
    {
        BezierCurve curve = new BezierCurve();
        curve.SetIsTwoControlPoint(false);
        curve.StartPosition = new Vector3(0f, 0f, 10f);
        curve.EndPosition = new Vector3(0, 0f, 0f);
        curve.controlPoints[0] = new Vector3(0f, 0f, 5f);
        curve.distancePrecision = 1000f;
        Assert.AreEqual(10f, Mathf.Round(curve.GetTotalLength()));

        //List<Vector3> testValue = new List<Vector3>(11);
        //testValue.Add(new Vector3(-4, -4, 1));
        //testValue.Add(new Vector3(-3.58f , -2.56f , 2.3f   ));
        //testValue.Add(new Vector3(-3.12f , -1.44f , 3.4f   ));
        //testValue.Add(new Vector3(-2.62f , -0.64f , 4.3f   ));
        //testValue.Add(new Vector3(-2.08f , -0.16f , 5f   ));
        //testValue.Add(new Vector3(-1.5f , 0f , 5.5f   ));
        //testValue.Add(new Vector3(-0.88f , -0.16f , 5.8f   ));
        //testValue.Add(new Vector3(-0.22f , -0.64f , 5.9f   ));
        //testValue.Add(new Vector3(0.48f , -1.44f , 5.8f   ));
        //testValue.Add(new Vector3(1.22f , -2.56f , 5.5f   ));
        //testValue.Add(new Vector3(2, -4, 5));

        curve.distancePrecision = 11f;
        curve.StartPosition = new Vector3(-4f, -4f, 1f);
        curve.EndPosition = new Vector3(2f, -4f, 5f);
        curve.controlPoints[0] = new Vector3(-2f, 4f, 8f);
        Assert.AreEqual(12.39773f, curve.GetTotalLength());

        //float distance = 0f;
        //for (int i = 0; i < testValue.Count - 1; ++i)
        //{
        //    distance += Vector3.Distance(testValue[i], testValue[i + 1]); 
        //}
        //Debug.Log(distance);


    }

    private Vector3 RoundVectorComponentsToDecimalPrecision(Vector3 orginalVector, int numberOfDecimals)
    {
        return new Vector3((float)System.Math.Round(orginalVector.x, numberOfDecimals), (float)System.Math.Round(orginalVector.y, numberOfDecimals), (float)System.Math.Round(orginalVector.z, numberOfDecimals));
    }
}
