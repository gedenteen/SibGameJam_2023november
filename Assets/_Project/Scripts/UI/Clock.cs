using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] GameObject handMinutes;
    [SerializeField] GameObject handHours;


    public void SetNewTime(float newTimeInHours)
    {
        float hours = (float)Math.Floor(newTimeInHours % 12.01f);
        hours = hours / 12f;
        float minutes = newTimeInHours % 1f;
        Debug.Log($"Clock: SetNewTime: hours={hours} minutes={minutes}");

        Vector3 rotation = handHours.transform.eulerAngles;
        rotation.z = -360f * hours;
        handHours.transform.eulerAngles = rotation;

        rotation = handMinutes.transform.eulerAngles;
        rotation.z = -360f * minutes;
        handMinutes.transform.eulerAngles = rotation;
    }
}
