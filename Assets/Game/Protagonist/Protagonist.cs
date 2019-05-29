using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls the behaviour of a player.
public class Protagonist : MonoBehaviour
{
    public ProtagonistConfig config;
    public Inventory inventory;
    
    public SkillMove skillMove;
    
    public Skill curSkillState;
    
    public GameObject reviveBeacon;
    
    // Is there any script that required the overall control of protagonist?
    public Util.Register<MonoBehaviour> requireControl = new Util.Register<MonoBehaviour>();
    
    public Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    public PolygonCollider2D col => this.GetComponentInChildren<PolygonCollider2D>();
    public SpriteRenderer sprite;
    public TrailRenderer trail => this.GetComponentInChildren<TrailRenderer>();
    
    void Start()
    {
        reviveBeacon.transform.position = this.transform.position;
    }
    
    
    void OnCollisionEnter2D(Collision2D c)
    {
        // Only Monsters, MonsterBullets, Disasters can hurt protagonist.
        if(((1 << c.collider.gameObject.layer) & LayerMask.GetMask("Monster", "MonsterBullet", "Disaster")) == 0) return;
        
        DestroyPlayer();
    }
    
    /// Player is dead and should be revived.
    /// The player object will not be removed but behaviours are limited.
    void DestroyPlayer()
    {
        if(curSkillState != null) Destroy(curSkillState);
        var rev = this.gameObject.AddComponent<StateReviving>();
        rev.reviveBeacon = reviveBeacon;
        rev.config = config;
    }
    
}
