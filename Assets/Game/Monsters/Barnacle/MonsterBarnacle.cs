using System;
using UnityEngine;

public class MonsterBarnacle : Monster
{
    public BarnacleConfig config;
    public Protagonist protagonist;
    public GameObject launchingPoint;
    [SerializeField] float cdTimer;
    
    void FixedUpdate()
    {
        if(protagonist != null && (protagonist.transform.position - this.transform.position).magnitude > config.reactDist) return;
        
        cdTimer = 0f.Max(cdTimer - Time.fixedDeltaTime);
        
        if(cdTimer == 0f)
        {
            var bullet = Instantiate(config.barnacleBulletTemplate);
            bullet.transform.rotation = Quaternion.FromToRotation(Vector2.down, this.transform.rotation * Vector2.down);
            bullet.transform.position = launchingPoint.transform.position;
            
            cdTimer = config.cooldown;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        const float dist = 1e6f;
        var hit = Physics2D.Raycast(launchingPoint.transform.position, launchingPoint.transform.rotation * Vector2.down, dist, LayerMask.GetMask("Terrain"));
        Gizmos.color = Color.red;
        if(hit.collider != null) Gizmos.DrawLine(launchingPoint.transform.position, hit.point);
        else Gizmos.DrawLine(launchingPoint.transform.position, launchingPoint.transform.rotation * Vector2.down * dist);
    }
    
    
}
