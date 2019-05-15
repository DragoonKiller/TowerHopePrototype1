using System;
using UnityEngine;


/// Take inputs of player.
/// Reaction of using skills and change states.
public class PlayerController : MonoBehaviour
{
    public Protagonist protagonist;
    public Inventory inventory;
    public SkillTable skillTable;
    
    Wand wand => inventory.curWand;
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    
    void Update()
    {
        ChangeSpec();
        
        // The controller will do nothing if the protagonist does not allow the control...
        if(protagonist.requireControl.registered)
        {
            Stop();
            return;
        }
        Move();
        SelectSkill();
        UseSkill();
    }
    
    void ChangeSpec()
    {
        if(!Input.GetKeyDown(KeyCode.Tab)) return;
        
        if(protagonist.requireControl.Registered(this))
        {
            protagonist.requireControl.Remove(this);
            return;
        }
        
        if(!protagonist.requireControl.registered)
        {
            if(!protagonist.skillMove.enabled) return;
            if(!protagonist.skillMove.standingStable) return;
            protagonist.requireControl.Register(this);
            return;
        }
        
    }
    
    void Stop()
    {
        protagonist.skillMove.commandLeft = false;
        protagonist.skillMove.commandRight = false;
        protagonist.skillMove.commandJump = false;
    }
    
    void Move()
    {
        protagonist.skillMove.commandLeft = Input.GetKey(KeyCode.A);
        protagonist.skillMove.commandRight = Input.GetKey(KeyCode.D);
        protagonist.skillMove.commandJump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
    }
    
    void SelectSkill()
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
            if(protagonist.curSkillState) DestroyImmediate(protagonist.curSkillState);
            protagonist.curSkillState = this.GetComponent<Skill>();
        }
    }
    
    void UseSkill()
    {
        if(!Input.GetKeyDown(KeyCode.Mouse0)) return;
        
        if(wand.skillPrepared)
        {
            Skill skill = protagonist.gameObject.AddComponent(skillTable[wand.curSkillSpec].skillType) as Skill;
            protagonist.curSkillState = skill;
            skill.config = skillTable[wand.curSkillSpec];
        }
    }
    
}
