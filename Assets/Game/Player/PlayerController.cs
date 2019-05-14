using System;
using UnityEngine;


/// Take inputs of player.
/// Reaction of using skills and change states.
public class PlayerController : MonoBehaviour
{
    public Protagonist player;
    public Inventory inventory;
    
    Wand wand => inventory.carryingWand;
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    
    void Update()
    {
        // The controller will do nothing if the protagonist does not allow the control...
        if(player.requireControl.registered) return;
        
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
    
    void UseSkill()
    {
        if(wand.skillPrepared)
        {
            player.curSkillState = wand.curSkillConfig.Build(player, wand.curSkillSpec);
        }
    }
    
}
