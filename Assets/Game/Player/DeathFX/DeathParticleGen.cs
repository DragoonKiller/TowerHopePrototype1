using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleGen : MonoBehaviour
{
    public GameObject deathParticle;
    
    public int maxCount;
    public int minCount;
    
    public float maxScale;
    public float minScale;
    
    public float minSpeed;
    public float maxSpeed;
    
    public float maxLifeTime;
    public float minLifeTime;
    
    // A manual particle system, that allow particles live longer than the particle system.
    public void GenParticles()
    {
        int count = Random.Range(minCount, maxCount + 1);
        for(int i=0; i<count; i++)
        {
            var x = Instantiate(deathParticle);
            x.transform.position = this.transform.position;
            x.transform.localScale = Random.Range(minScale, maxScale) * Vector3.one;
            var p = x.GetComponent<DeathParticle>();
            p.velocity = Vector2.right.Rot(Random.Range(0, 90) * Mathf.Deg2Rad) * Random.Range(minSpeed, maxSpeed);
            p.lifeTime = Random.Range(minLifeTime, maxLifeTime);
        }
    }
    
}
