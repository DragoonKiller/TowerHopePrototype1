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
        bubble.player = player;
        bubble.config = config;
        
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
        player.ConsumeMagic(Time.fixedDeltaTime * (player.magicRecoverRate + config.magicConsumePerSec));
        if(player.magic.LEZ())
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
