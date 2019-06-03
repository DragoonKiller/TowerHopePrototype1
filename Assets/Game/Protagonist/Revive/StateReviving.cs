using System;
using UnityEngine;

/// This script take control of the protagonist object when player is reviving.
/// It is created when proagonist is dead (but the object is not removed).
public class StateReviving : MonoBehaviour
{
    public ProtagonistConfig config;
    public GameObject reviveBeacon;
    
    Protagonist protagonist => this.GetComponent<Protagonist>();
    [SerializeField] float timer;
    
    void Start()
    {
        foreach(var tr in protagonist.trails) tr.enabled = false;
        
        // Begin reviving instantly.
        var particleGen = this.GetComponentInChildren<DeathParticleGen>();
        particleGen.GenParticles();
        
        // Disable player control.
        protagonist.requireControl.Add(this);
        
        // Hide player sprite.
        protagonist.sprite.enabled = false;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        float rate = timer / config.reviveTime;
        
        if(rate >= 1f)
        {
            DestroyImmediate(this);
            return;
        }
        
    }
    
    void OnDestroy()
    {
        // Player is revived and is shown again.
        protagonist.sprite.enabled = true;
        
        // Place player to the correct location.
        if(this.gameObject != null) this.transform.position = reviveBeacon.transform.position;
        
        foreach(var tr in protagonist.trails)
        {
            tr.enabled = true;
            tr.Clear();
        }
        
        // Stop the player.
        protagonist.rd.velocity = Vector2.zero;
        
        // Enable player control.
        protagonist.requireControl.Remove(this);
    }
}
