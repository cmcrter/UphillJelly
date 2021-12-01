using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using SleepyCat.Utility.Splines;

public class HermiteSplineTests
{
    [Test]
    public void TestGet1PointLength()
    {
        HermiteSpline curve = new HermiteSpline();
        curve.StartPosition = new Vector3(0f, 0f, 10f);
        curve.StartTangent = curve.StartPosition;
        curve.EndPosition = new Vector3(0, 0f, 0f);
        for (curve.DistancePrecision = 5f; curve.DistancePrecision <= 25f; curve.DistancePrecision += 0.25f)
        {
            Assert.AreEqual(10f, Mathf.Round(curve.GetTotalLength()));
        }
    }
}
