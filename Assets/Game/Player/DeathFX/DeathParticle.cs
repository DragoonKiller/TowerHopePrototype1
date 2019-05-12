using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Define the behaviour of the death particle gen.
[RequireComponent(typeof(SpriteRenderer))]
public class DeathParticle : MonoBehaviour
{
    public Vector2 velocity;
    public float lifeTime;
    [SerializeField] float timer;
    
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    void Start()
    {
        timer = 0f;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        
        AdjustDirection();
        
        var alpha = 1.0f - timer / lifeTime;
        rd.color = rd.color.A(alpha);
    }
    
    void FixedUpdate()
    {
        this.transform.position += (Vector3)(velocity * Time.fixedDeltaTime + 0.5f * Physics2D.gravity * Time.fixedDeltaTime.Sqr());
        velocity += Physics2D.gravity * Time.fixedDeltaTime;
    }
    
    void AdjustDirection()
    {
        // The basic sprite is down direction, so the first parameter is down.
        this.transform.rotation = Quaternion.FromToRotation(Vector2.down, velocity);
    }
    
}
