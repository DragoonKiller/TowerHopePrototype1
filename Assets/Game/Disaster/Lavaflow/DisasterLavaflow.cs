using System;
using UnityEngine;

public class DisasterLavaflow : Disaster
{
    public float stopHeight;
    public float speed;
    
    public override void Trigger()
    {
        // Do nothing...
    }
    
    void FixedUpdate()
    {
        
        if(this.transform.position.y >= stopHeight)
        {
            this.transform.position = this.transform.position.Y(stopHeight);
            return;
        }
        
        this.transform.Translate(speed * Time.fixedDeltaTime * Vector3.up);
    }
    
}
