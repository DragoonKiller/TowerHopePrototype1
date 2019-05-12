using System;
using UnityEngine;

public class MonsterBarnacle : Monster
{
    public BarnacleConfig config;
    public GameObject launchingPoint;
    [SerializeField] float cdTimer;
    
    void FixedUpdate()
    {
        cdTimer = 0f.Max(cdTimer - Time.fixedDeltaTime);
        
        if(cdTimer == 0f)
        {
            var bullet = Instantiate(config.barnacleBulletTemplate);
            bullet.transform.rotation = Quaternion.FromToRotation(Vector2.down, this.transform.rotation * Vector2.down);
            bullet.transform.position = launchingPoint.transform.position;
            
            cdTimer = config.cooldown;
        }
    }
    
    
}
