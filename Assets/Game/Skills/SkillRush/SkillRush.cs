using System;
using UnityEngine;

public class SkillRush : Skill
{
    public SkillRushConfig config;
    
    public Vector2 targetVelocity;
    public Vector2 curVelocity;
    
    [SerializeField] Color recordSpriteColor;
    [SerializeField] Color recordTrailStartColor;
    [SerializeField] Color recordTrailEndColor;
    
    [SerializeField] GameObject stickEffect;
    public bool stuck => stickEffect != null;
    public bool canBeStucked => lifeTime.GE(config.stickTimeLimit);
    
    float lifeTime;
    float stopTime;
    
    
    void Start()
    {
        var mousePos = Util.cursorWorldPosition;
        targetVelocity = ((Vector3)mousePos - transform.position).normalized * config.speed;
        curVelocity = targetVelocity * 0.5f;
        
        GetComponent<SkillMove>().enabled = false;
        
        recordSpriteColor = player.sprite.color;
        recordTrailStartColor = player.trail.startColor;
        recordTrailEndColor = player.trail.endColor;
        
        player.sprite.color = config.activeColor;
        player.trail.startColor = config.activeColor.A(player.trail.startColor);
        player.trail.endColor = config.activeColor.A(player.trail.endColor);
        
        lifeTime = 0f;
        stopTime = config.stopTime;
    }
    
    void Update()
    {
        // If stuck, there'll be no more time limits for this skill.
        if(!stuck) lifeTime += Time.deltaTime;
        
        stopTime -= Time.deltaTime;
        
        // Release left mouse button, stop the skill.
        if((!Input.GetKey(KeyCode.Mouse0) && stopTime <= 0f) || lifeTime.GE(config.lifeTime))
        {
            DestroyImmediate(this);
            return;
        }
    }
    
    void FixedUpdate()
    {
        // Consume player's magic.
        // Do not allow magic recover when using this skill.
        if(!stuck)
        {
            player.ConsumeMagic(Time.fixedDeltaTime * config.magicConsumePerSec);
        }
        else
        {
            player.ConsumeMagic(Time.fixedDeltaTime * config.stickMagicConsumePerSec);
        }
        
        
        if(player.inventory.carryingWand.curSlot.magic.LEZ())
        {
            DestroyImmediate(this);
            return;
        }
        
        // If player ran into something, the protagonist will stick on it,
        //   until player stop the skill or the magic is used up.
        
        if(!stuck && player.recentContacts.Count > 0 && canBeStucked)
        {
            ContactPoint2D terrainContact = new ContactPoint2D();
            foreach(var c in player.recentContacts)
            {
                // The collided object was destroyed.
                // It must not be a terrain object.
                if(c.collider == null) continue;
                if(c.collider.gameObject == null) continue;
                
                if(c.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    terrainContact = c;
                    break;
                }
            }
            
            if(terrainContact.collider != null)
            {
                stickEffect = Instantiate(config.stickEffectTemplate);
                stickEffect.transform.rotation = Quaternion.FromToRotation(Vector2.up, terrainContact.normal);
                stickEffect.transform.position = terrainContact.point;
                
                // Stop the protagonist.
                curVelocity = Vector2.zero;
            }
        }
        
        // DEPRECATED FEATURE... [
        // If the protagonist run into something, velocity on that direction will be reduced.
        // Vector2 cur = curVelocity;
        // foreach(var i in player.recentContacts)
        // {
        //     if(i.normal.Dot(curVelocity).GEZ()) continue;
        //     var localCoord = new CoordSys(i.normal.RotHalfPi(), i.normal);
        //     curVelocity = localCoord.LocalToWorld(localCoord.WorldToLocal(curVelocity).Y(0));
        // }
        // ] DEPRECATED FEATURE
        
        rd.velocity = curVelocity;
        
        // DEPRECATED FEATURE... [
        // The velocity will recover...
        // curVelocity += (targetVelocity - curVelocity) * (1.0f - (1.0f - speedRecoverRate).Pow(Time.fixedDeltaTime));
        // ] DEPRECATED FEATURE
        
        
    }
    
    void OnDestroy()
    {
        player.sprite.color = recordSpriteColor;
        player.trail.startColor = recordTrailStartColor;
        player.trail.endColor = recordTrailEndColor;
        if(stickEffect) DestroyImmediate(stickEffect);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<SkillMove>().enabled = true;
    }
}
