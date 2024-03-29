using System;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public bool active;
    public GameObject target;
    [Range(0, 1)] public float closeRatePerFrame;
    
    public Vector3 offset;
    
    public float minSpeed;
    
    [Range(0, 0.5f)] public float limitRatio;
    
    Camera cam => this.GetComponent<Camera>();
    
    [SerializeField] public Vector2 localP;
    
    void Update()
    {
        if(active)
        {
            var curPos = (Vector2)(this.transform.position - offset);
            var targetPos = (Vector2)target.transform.position;
            
            var relativePos = curPos.To(targetPos);
            var localCoord = new CoordSys(Vector2.right * cam.orthographicSize * cam.aspect, Vector2.up * cam.orthographicSize);
            var localRelativePos = localCoord.WorldToLocal(relativePos);
            localP = localRelativePos;
            localRelativePos.x = localRelativePos.x.Clamp(-limitRatio, limitRatio);
            localRelativePos.y = localRelativePos.y.Clamp(-limitRatio, limitRatio);
            var clampedRelativePos = localCoord.LocalToWorld(localRelativePos);
            
            var lb = localCoord.WorldToLocal(curPos) + Vector2.left * limitRatio + Vector2.down * limitRatio;
            var rb = localCoord.WorldToLocal(curPos) + Vector2.right * limitRatio + Vector2.down * limitRatio;
            var lt = localCoord.WorldToLocal(curPos) + Vector2.left * limitRatio + Vector2.up * limitRatio;
            var rt = localCoord.WorldToLocal(curPos) + Vector2.right * limitRatio + Vector2.up * limitRatio;
            lb = localCoord.LocalToWorld(lb);
            rb = localCoord.LocalToWorld(rb);
            lt = localCoord.LocalToWorld(lt);
            rt = localCoord.LocalToWorld(rt);
            Debug.DrawLine(lb, rb, Color.red);
            Debug.DrawLine(lt, rt, Color.red);
            Debug.DrawLine(lb, lt, Color.red);
            Debug.DrawLine(rb, rt, Color.red);
            
            var clampedPos = (Vector2)targetPos - clampedRelativePos;
            
            this.transform.position = (Vector3)clampedPos + offset;
            
            var targetMove = (Vector2)targetPos - clampedPos;
            var moveDist = targetMove.magnitude * closeRatePerFrame;
            moveDist.UpdMax(minSpeed * Time.deltaTime);
            moveDist.UpdMin(targetMove.magnitude);
            
            this.transform.position = offset + (Vector3)(clampedPos + targetMove.normalized * moveDist);
        }
        
    }
    
    
    
}
