using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDebugger : MonoBehaviour
{
    private Vector3 lastPosition;

    [SerializeField]
    private float currentAverageSpeed;
    [SerializeField]
    private List<float> speeds;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = transform.position - lastPosition;
        speeds.Add(currentVelocity.magnitude / Time.deltaTime);
        if (speeds.Count > 100)
        {
            speeds.RemoveAt(0);
        }
        lastPosition = transform.position;

        currentAverageSpeed = 0f;
        for (int i = 0; i < speeds.Count; ++i)
        {
            currentAverageSpeed += speeds[i];
        }
        currentAverageSpeed /= speeds.Count;
    }
}
