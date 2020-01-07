using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumClock : MonoBehaviour
{
    private Transform gear1;
    private Transform gear2;
    private Transform gear3;
    private Transform gear4;

    private Transform minute;
    private Transform hour;

    float smooth = 5.0f;

    public float speed = 20;

    public HandAction handAction;

    private float lastChanged = 0f;
    private float timeToChange = 2f;

    void Start()
    {
        if (handAction == HandAction.Forward)
        {
            speed *= -1;
        }
        gear1 = transform.GetChild(1).transform;
        gear2 = transform.GetChild(2).transform;
        gear3 = transform.GetChild(3).transform;
        gear4 = transform.GetChild(4).transform;

        hour = transform.GetChild(5).transform;
        minute = transform.GetChild(6).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (handAction == HandAction.Forward)
        {
            Spin();
        }
        else if (handAction == HandAction.Backward)
        {
            Spin();
        }
        else if (handAction == HandAction.Random && lastChanged + timeToChange < Time.time)
        {
            timeToChange = Random.Range(1f, 4f);
            lastChanged = Time.time;
            RandomLocation();
        }
    }

    private void Spin()
    {
        Quaternion target = gear1.rotation * Quaternion.Euler(0, 0, -speed / 2);
        gear1.rotation = Quaternion.Slerp(gear1.rotation, target, Time.deltaTime * smooth);

        target = gear2.rotation * Quaternion.Euler(0, 0, speed / 3);
        gear2.rotation = Quaternion.Slerp(gear2.rotation, target, Time.deltaTime * smooth);

        target = gear3.rotation * Quaternion.Euler(0, 0, speed);
        gear3.rotation = Quaternion.Slerp(gear3.rotation, target, Time.deltaTime * smooth);

        target = gear4.rotation * Quaternion.Euler(0, 0, speed / 8);
        gear4.rotation = Quaternion.Slerp(gear4.rotation, target, Time.deltaTime * smooth);

        target = minute.rotation * Quaternion.Euler(0, 0, speed);
        minute.rotation = Quaternion.Slerp(minute.rotation, target, Time.deltaTime * smooth);

        target = hour.rotation * Quaternion.Euler(0, 0, speed / 12);
        hour.rotation = Quaternion.Slerp(hour.rotation, target, Time.deltaTime * smooth);
    }

    private void RandomLocation()
    {
        /*   float location = Random.Range(0f, 12f);
           float prevRotation = hour.rotation.z / 30;
           float difference = prevRotation - location;

           Quaternion target = Quaternion.Euler(0, 0, location * 30);
           hour.rotation = Quaternion.Slerp(hour.rotation, target, Time.deltaTime * smooth);

           target = Quaternion.Euler(0, 0, location / 10);
           */

        Quaternion target = gear1.rotation * Quaternion.Euler(0, 0, -speed / 2);
        gear1.rotation = Quaternion.Slerp(gear1.rotation, target, Time.deltaTime * smooth);

        target = gear2.rotation * Quaternion.Euler(0, 0, speed / 3);
        gear2.rotation = Quaternion.Slerp(gear2.rotation, target, Time.deltaTime * smooth);

        target = gear3.rotation * Quaternion.Euler(0, 0, speed);
        gear3.rotation = Quaternion.Slerp(gear3.rotation, target, Time.deltaTime * smooth);

        target = gear4.rotation * Quaternion.Euler(0, 0, speed / 8);
        gear4.rotation = Quaternion.Slerp(gear4.rotation, target, Time.deltaTime * smooth);


        float location = Random.Range(0f, 12f);
        hour.rotation = Quaternion.Euler(0, 0, location * 30);
        Debug.Log(location);
        location %= 1;
        Debug.Log(location);
        location *= 100 * 3.6f;
        Debug.Log(location);
        minute.rotation = Quaternion.Euler(0, 0, location);


    }
}

public enum HandAction
{
    Forward,
    Backward,
    Random
}
