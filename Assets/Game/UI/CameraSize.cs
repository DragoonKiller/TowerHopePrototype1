using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


/// TODO: make size changin dynamic and automatic.
[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class CameraSize : MonoBehaviour
{
    public float targetSize;
    public Protagonist protagonist;
    
    public float minSize;
    public float maxSize;
    [Range(0f, 1f)] public float sizeApprochRate;
    public float minApprochRate;
    
    public int testRayCount;
    
    Camera cam => this.GetComponent<Camera>();
    
    [Header("Information")]
    public float testedDist;
    
    void Update()
    {
        targetSize = ComputeTargetSize().Clamp(minSize, maxSize);
        float curSize = cam.orthographicSize;
        float nxtSize = targetSize * sizeApprochRate + curSize * (1.0f - sizeApprochRate);
        if((nxtSize - curSize).Abs() < minApprochRate) nxtSize = curSize + minApprochRate * (nxtSize - curSize).Sgn();
        cam.orthographicSize = nxtSize;
    }
    
    float ComputeTargetSize()
    {
        var dists = new List<float>(testRayCount);
        for(int i=0; i<testRayCount; i++)
        {
            Vector2 dir = Vector2.right.Rot(2.0f * Mathf.PI * i / testRayCount);
            var hitDist = maxSize;
            var hit = Physics2D.Raycast(protagonist.transform.position, dir, hitDist, LayerMask.GetMask("Terrain"));
            
            Debug.DrawRay(
                protagonist.transform.position,
                hit.collider == null ? dir * maxSize : hit.point - (Vector2)protagonist.transform.position,
                Color.white
            );
            
            if(hit.collider != null) hitDist = hit.distance;
            dists.Add(hitDist);
        }
        
        dists.Sort();
        
        float avgDist = 0f;
        float sumDist = 0f;
        for(int i=0; i<dists.Count; i++)
        {
            avgDist += dists[i] * 1f.Max(2f * i / dists.Count);
            sumDist += 1f.Max(2f * i / dists.Count);
        }
        avgDist /= sumDist;
        
        testedDist = avgDist;
        return avgDist;
    }
}
