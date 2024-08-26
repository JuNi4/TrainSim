using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    public float DayDurationInSeconds = 0;
    private float timeRate = 0;

    // Start is called before the first frame update
    void Start()
    {
        // calculate the rate at wich the daytime progresses
        timeRate = 360 / DayDurationInSeconds;
        Screen.brightness = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the time
        gameObject.transform.Rotate(Time.deltaTime * timeRate, 0, 0);
    }
}
