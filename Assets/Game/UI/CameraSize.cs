using System;
using UnityEngine;


/// TODO: make size changin dynamic and automatic.
[RequireComponent(typeof(Camera))]
public class CameraSize : MonoBehaviour
{
    public float scrollRate;
    public float closeRatePerFrame;
    
    public float maxSize;
    public float minSize;
    
    [SerializeField] float curSize;
    
    Camera cam => this.GetComponent<Camera>();
    void Update()
    {
        float p = Input.mouseScrollDelta.y;
        if(p.EZ()) return;
        
        curSize = (curSize + p * scrollRate).Clamp(minSize, maxSize);
        
        cam.orthographicSize = cam.orthographicSize * (1.0f - closeRatePerFrame) + curSize * closeRatePerFrame;
    }
}
