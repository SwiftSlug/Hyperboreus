using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatFood : MonoBehaviour {

    public bool eaten = false;
    public float regenTime = 10.0f;
    public float lastEaten = 0.0f;

    public void Eat()
    {
        eaten = true;
        lastEaten = Time.time;
    }

    public void Update()
    {
        if (Time.time > (lastEaten + regenTime))
        {
            eaten = false;
        }
    }

}
