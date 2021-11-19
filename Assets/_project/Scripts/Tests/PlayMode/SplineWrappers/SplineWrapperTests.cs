using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using SleepyCat.Utility.Splines;

public class SplineWrapperTests : MonoBehaviour
{
    public static IEnumerator TestSettingLocalStartPosition(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.zero;
        testedSpline.LocalStartPosition = Vector3.up;
        Assert.AreEqual(Vector3.up, testedSpline.LocalStartPosition);
        Assert.AreEqual(Vector3.up, testedSpline.WorldStartPosition);
        testedSpline.transform.position = Vector3.down;
        testedSpline.UpdateWorldPositions();
        Assert.AreEqual(Vector3.up, testedSpline.LocalStartPosition);
        Assert.AreEqual(Vector3.zero, testedSpline.WorldStartPosition);
        return null;
    }

    public static IEnumerator TestSettingLocalEndPosition(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.zero;
        testedSpline.LocalEndPosition = Vector3.up;
        Assert.AreEqual(Vector3.up, testedSpline.LocalEndPosition);
        Assert.AreEqual(Vector3.up, testedSpline.WorldEndPosition);
        testedSpline.transform.position = Vector3.down;
        testedSpline.UpdateWorldPositions();
        Assert.AreEqual(Vector3.up, testedSpline.LocalEndPosition);
        Assert.AreEqual(Vector3.zero, testedSpline.WorldEndPosition);
        return null;
    }

    public static IEnumerator TestSettingWorldPositionStartWithoutLocal(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.one;
        testedSpline.SetWorldStartPointWithoutLocal(Vector3.up);
        Assert.AreEqual(Vector3.up, testedSpline.WorldStartPosition);
        Assert.AreEqual(Vector3.zero, testedSpline.LocalStartPosition);
        return null;
    }

    public static IEnumerator TestSettingWorldPositionEndWithoutLocal(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.one;
        testedSpline.SetWorldEndPointWithoutLocal(Vector3.up);
        Assert.AreEqual(Vector3.up, testedSpline.WorldEndPosition);
        Assert.AreEqual(Vector3.zero, testedSpline.LocalEndPosition);
        return null;
    }

    public static IEnumerator TestSettingWorldPositionStartWithLocal(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.one;
        testedSpline.SetWorldStartPointAndUpdateLocal(Vector3.up);
        Assert.AreEqual(Vector3.up, testedSpline.WorldStartPosition);
        Assert.AreEqual(new Vector3(-1f, 0, -1f), testedSpline.LocalStartPosition);
        return null;
    }

    public static IEnumerator TestSettingWorldPositionEndWithLocal(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.one;
        testedSpline.SetWorldEndPointAndUpdateLocal(Vector3.up);
        Assert.AreEqual(Vector3.up, testedSpline.WorldEndPosition);
        Assert.AreEqual(new Vector3(-1f, 0f, -1f), testedSpline.LocalEndPosition);
        return null;
    }

    public static IEnumerator TestUpdatingWorldPositions(SplineWrapper testedSpline)
    {
        testedSpline.transform.position = Vector3.zero;
        testedSpline.LocalEndPosition = Vector3.up;
        testedSpline.LocalStartPosition = Vector3.right;
        testedSpline.transform.position = Vector3.down;
        testedSpline.UpdateWorldPositions();
        Assert.AreEqual(Vector3.zero, testedSpline.WorldEndPosition);
        Assert.AreEqual(new Vector3(1f, -1f, 0f), testedSpline.WorldStartPosition);
        return null;
    }
}
