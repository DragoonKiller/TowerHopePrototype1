using System;
using UnityEngine;

[ExecuteAlways]
public class SkillSlotDisplay : MonoBehaviour
{
    public WandDisplayConfig config;
    public SkillTable skillTable;
    SkillSlot slot => this.GetComponent<SkillSlot>();
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    public string skillName;
    
    void Update()
    {
        var data = skillTable[slot.spec];
        if(data == null) skillName = "Undefined";
        else skillName = data.name;
    }
}
