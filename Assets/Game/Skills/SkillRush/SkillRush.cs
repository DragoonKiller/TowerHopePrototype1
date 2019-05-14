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
        
        recordSpriteColor = protagonist.sprite.color;
        recordTrailStartColor = protagonist.trail.startColor;
        recordTrailEndColor = protagonist.trail.endColor;
        
        protagonist.sprite.color = config.activeColor;
        protagonist.trail.startColor = config.activeColor.A(protagonist.trail.startColor);
        protagonist.trail.endColor = config.activeColor.A(protagonist.trail.endColor);
        
        lifeTime = 0f;
        stopTime = config.stopTime * (1.0f + config.rangePerIndicatorStone * spec.Count(StoneType.Indicator));
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
        float magicCost = 0f.Max(1.0f - config.efficiencyPerNatureStone * spec.Count(StoneType.Nature));
        
        // Consume player's magic.
        // Do not allow magic recover when using this skill.
        if(!stuck)
        {
            protagonist.inventory.curWand.curSlot.ConsumeMagic(Time.fixedDeltaTime * config.magicConsumePerSec * magicCost);
        }
        else
        {
            protagonist.inventory.curWand.curSlot.ConsumeMagic(Time.fixedDeltaTime * config.stickMagicConsumePerSec * magicCost);
        }
        
        
        if(protagonist.inventory.curWand.curSlot.magic.LEZ())
        {
            DestroyImmediate(this);
            return;
        }
        
        // If player ran into something, the protagonist will stick on it,
        //   until player stop the skill or the magic is used up.
        
        if(!stuck && contactDetector.recentContacts.Count > 0 && canBeStucked)
        {
            ContactPoint2D terrainContact = new ContactPoint2D();
            foreach(var c in contactDetector.recentContacts)
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
        protagonist.sprite.color = recordSpriteColor;
        protagonist.trail.startColor = recordTrailStartColor;
        protagonist.trail.endColor = recordTrailEndColor;
        if(stickEffect) DestroyImmediate(stickEffect);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<SkillMove>().enabled = true;
    }
}
