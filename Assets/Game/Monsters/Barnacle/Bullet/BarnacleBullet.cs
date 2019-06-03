using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BarnacleBullet : MonsterBullet
{
    public BarnacleConfig config;
    
    [SerializeField] bool shouldDestroy = false;
    
    float timer;
    
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    void FixedUpdate()
    {
        if(shouldDestroy)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        
        float curSpeed = rd.velocity.magnitude;
        float nxtSpeed = curSpeed * (1.0f - config.accRatePerPhysFrame) + config.targetSpeed;
        rd.velocity = this.transform.rotation * Vector2.down * nxtSpeed;
        
        timer += Time.fixedDeltaTime;
        if(timer > config.bulletLifeTime) shouldDestroy = true;
    }
    
    // If it hits anything...
    void OnCollisionEnter2D(Collision2D c)
    {
        Instantiate(config.barnacleBulletExplodeTemplate, this.transform.position, this.transform.rotation);
        shouldDestroy = true;
    }
}
