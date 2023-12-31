using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] GameObject handMinutes;
    [SerializeField] GameObject handHours;
    [SerializeField] GameObject coverOfClock;
    [SerializeField] AudioClip soundOfClock;

    public void SetNewTime(float newTimeInHours)
    {
        float hours = (float)Math.Floor(newTimeInHours % 12f);
        hours = hours / 12f;
        float minutes = newTimeInHours % 1f;
        Debug.Log($"Clock: SetNewTime: hours={hours} minutes={minutes}");

        Vector3 rotation = handHours.transform.eulerAngles;
        rotation.z = -360f * hours;
        handHours.transform.eulerAngles = rotation;

        rotation = handMinutes.transform.eulerAngles;
        rotation.z = -360f * minutes;
        handMinutes.transform.eulerAngles = rotation;

        AudioManager.instance.PlaySound(soundOfClock);
    }

    public void ShowCoverOfClock(bool show)
    {
        coverOfClock.SetActive(show);
    }
}
