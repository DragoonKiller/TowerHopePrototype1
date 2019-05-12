using System;
using UnityEngine;

/// This script take control of the protagonist object when player is reviving.
/// It is created when proagonist is dead (but the object is not removed).
public class PlayerReviving : MonoBehaviour
{
    public GameObject reviveBeacon;
    
    public float reviveTime;
    [SerializeField] float timer;
    
    void Start()
    {
        // Begin reviving instantly.
        var particleGen = this.GetComponentInChildren<DeathParticleGen>();
        particleGen.GenParticles();
        GetComponent<PlayerController>().enabled = false;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= reviveTime)
        {
            DestroyImmediate(this);
            return;
        }
    }
    
    void OnDestroy()
    {
        this.transform.position = reviveBeacon.transform.position;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
