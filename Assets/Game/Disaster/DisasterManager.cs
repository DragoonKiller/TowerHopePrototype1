using System;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public Disaster disaster;
    public float countdown;
    public float minCountdown;
    public float maxCountdown;
    
    void Update()
    {
        countdown = 0f.Max(countdown - Time.deltaTime);
        if(countdown == 0f)
        {
            disaster.Trigger();
            countdown += UnityEngine.Random.Range(minCountdown, maxCountdown);
        }
    }
}
