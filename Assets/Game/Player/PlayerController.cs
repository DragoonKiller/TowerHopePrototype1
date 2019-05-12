using System;
using UnityEngine;

/// Take control of the player, affect the protagonist's state and behaviour.
/// This will determine protagonist's state,
///   and its control-acceptance for that skill.
public class PlayerController : MonoBehaviour
{
    PlayerState player => GetComponent<PlayerState>();
    Wand wand => player.inventory.carryingWand;
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    
    void Start()
    {
        
    }
    
    void Update()
    {
        // Change selected skill using keyboard.
        int prevSkill = wand.curSkillId;
        if(Input.GetKeyDown(KeyCode.Q)) wand.curSkillId -= 1;
        if(Input.GetKeyDown(KeyCode.E)) wand.curSkillId += 1;
        if(Input.GetKeyDown(KeyCode.Alpha1)) wand.curSkillId = 0;
        if(Input.GetKeyDown(KeyCode.Alpha2)) wand.curSkillId = 1;
        if(Input.GetKeyDown(KeyCode.Alpha3)) wand.curSkillId = 2;
        
        // Cutoff current non-move skill if player manually selected another skill.
        if(prevSkill != wand.curSkillId)
        {
            if(player.curSkillState) DestroyImmediate(player.curSkillState);
            player.curSkillState = this.GetComponent<Skill>();
        }
        
        // Use skill with mouse.
        if(Input.GetKeyDown(KeyCode.Mouse0)) UseSkill();
    }
    
    void FixedUpdate()
    {
    
    }
    
    void UseSkill()
    {
        if(wand.curSlot.magic.G(wand.curSkillConfig.magicRequired))
        {
            player.curSkillState = wand.curSkillConfig.Build(player);
        }
    }
    
}
