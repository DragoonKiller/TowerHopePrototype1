using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBubble : Skill
{
    public SkillBubbleConfig config;
    
    [SerializeField] DefenceBubble bubble;
    
    void Start()
    {   
        this.GetComponent<SkillMove>().enabled = false;
        
        bubble = Instantiate(config.bubbleSource, this.transform, false).GetComponent<DefenceBubble>();
        bubble.protagonist = protagonist;
        bubble.config = config;
        bubble.spec = spec;
        
        // Place it to the center of player.
        bubble.transform.localPosition = Vector3.zero;
    }
    
    void Update()
    {
        if(!Input.GetKey(KeyCode.Mouse0))
        {
            DestroyImmediate(this);
            return;
        }
    }
    
    void FixedUpdate()
    {
        float magicCost = 0f.Max(1.0f - config.efficiencyPerNatureStone * spec.Count(StoneType.Nature));
        protagonist.inventory.curWand.curSlot.ConsumeMagic(Time.fixedDeltaTime * config.magicConsumePerSec * magicCost);
        if(protagonist.inventory.curWand.curSlot.magic.LEZ())
        {
            DestroyImmediate(this);
            return;
        }
        
        // Do all things of velocity *in world coordinate system*.
        var curv = rd.velocity;
        var expv = config.speed * Vector2.up;
        var delta = expv - curv;
        if(delta.magnitude <= config.accPerPhysicsFrame) delta = Vector2.zero;
        else delta = delta.Len(delta.magnitude - config.accPerPhysicsFrame);
        var nxtv = expv - delta;
        rd.velocity = nxtv;
    }
    
    void OnDestroy()
    {
        DestroyImmediate(bubble.gameObject);
        this.GetComponent<SkillMove>().enabled = true;
    }
}
