using System;
using UnityEngine;

public class DisasterLavaflow : Disaster
{
    public float[] heights;
    public float floodPerSec;
    int curHeight;
    public float restHeight;
    
    // Foreach triggering, simply move the altitude up. 
    public override void Trigger()
    {
        // Do nothing if reach the top...
        if(curHeight == heights.Length) return;
        
        restHeight += heights[curHeight];
        curHeight += 1;
    }
    
    public void Update()
    {
        var delta = restHeight - 0f.Max(restHeight - floodPerSec * Time.deltaTime);
        this.transform.position += delta * Vector3.up;
        restHeight -= delta;
    }
    
}
