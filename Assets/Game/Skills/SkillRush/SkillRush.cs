using System;
using UnityEngine;

public class SkillRush : Skill
{
    public new SkillRushConfig config => base.config as SkillRushConfig;
    
    public Vector2 targetVelocity;
    public Vector2 curVelocity;
    
    [SerializeField] Color recordSpriteColor;
    [SerializeField] Color recordTrailStartColor;
    [SerializeField] Color recordTrailEndColor;
    
    [SerializeField] GameObject stickEffect;
    public bool stuck => stickEffect != null;
    
    float lifeTimer;
    float allowStopTimer;
    
    void Start()
    {
        var mousePos = Util.cursorWorldPosition;
        targetVelocity = ((Vector3)mousePos - transform.position).normalized * config.speed;
        curVelocity = targetVelocity * 0.5f;
        
        this.GetComponent<SkillMove>().enabled = false;
        
        recordSpriteColor = protagonist.sprite.color;
        foreach(var trail in protagonist.trails) recordTrailStartColor = trail.startColor;
        foreach(var trail in protagonist.trails) recordTrailEndColor = trail.endColor;
        
        protagonist.sprite.color = config.activeColor;
        foreach(var trail in protagonist.trails) trail.startColor = config.activeColor.A(trail.startColor);
        foreach(var trail in protagonist.trails) trail.endColor = config.activeColor.A(trail.endColor);
        
        lifeTimer = 0f;
        allowStopTimer = config.stopTime * (1.0f + config.rangePerIndicatorStone * spec.Count(StoneType.Indicator));
    }
    
    void Update()
    {
        // If stuck, there'll be no more time limits for this skill.
        if(!stuck) lifeTimer += Time.deltaTime;
        
        allowStopTimer -= Time.deltaTime;
        
        // Mouse button is not released, but time's up.
        if(!stuck && lifeTimer.GE(config.lifeTime))
        {
            if(!TryGrab())
            {
                DestroyImmediate(this);
                return;
            }
        }
        
        // Release left mouse button, or times up, stop the skill.
        if(!Input.GetKey(KeyCode.Mouse0) && allowStopTimer <= 0f)
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
        
        rd.velocity = curVelocity;
    }
    
    void OnDestroy()
    {
        protagonist.sprite.color = recordSpriteColor;
        foreach(var trail in protagonist.trails) trail.startColor = recordTrailStartColor;
        foreach(var trail in protagonist.trails) trail.endColor = recordTrailEndColor;
        if(stickEffect) DestroyImmediate(stickEffect);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<SkillMove>().enabled = true;
    }
    
    bool TryGrab()
    {
        // If player ran into something, the protagonist will stick on it,
        //   until player stop the skill or the magic is used up.
        if(!stuck && contactDetector.recentContacts.Count > 0)
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
                rd.velocity = curVelocity;
                return true;
            }
        }
        
        return false;
    }
}
