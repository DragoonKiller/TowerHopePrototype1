using System;
using System.Collections.Generic;
using UnityEngine;

public class VulkflyBullet : MonoBehaviour
{
    public VulkflyConfig config;
    public Protagonist protagonist;
    
    public bool shouldDestroy = false;
    public float lifeTime;
    
    [Header("Internal Information")]
    [SerializeField] public Vector2 targetFacing;
    
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    
    void Update()
    {
        if(shouldDestroy)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
    }
    
    void FixedUpdate()
    {
        targetFacing = protagonist.transform.position - this.transform.position;
        rd.velocity = this.transform.rotation * Vector2.down * config.bulletSpeed;
        RotateToTargetFacing();
        
        Debug.DrawRay(this.transform.position, targetFacing, Color.red);
        
        lifeTime += Time.fixedDeltaTime;
        if(lifeTime >= config.bulletLifeTime) shouldDestroy = true;
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        Instantiate(config.bulletExplodeTemplate, this.gameObject.transform.position, Quaternion.identity);
        shouldDestroy = true;
    }
    
    void RotateToTargetFacing()
    {
        var a = Vector2.SignedAngle(this.transform.rotation * Vector2.down, targetFacing);
        a = a.Sgn() * a.Abs().Clamp(0f, config.bulletAngularSpeed * Time.deltaTime);
        this.transform.rotation *= Quaternion.Euler(0, 0, a);
    }
    
    
}
