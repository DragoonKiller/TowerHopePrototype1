using System;
using UnityEngine;

[ExecuteAlways]
public class SkillSlotDisplay : MonoBehaviour
{
    public StoneDisplayConfig config;
    public SkillTable skillTable;
    SkillSlot slot => this.GetComponent<SkillSlot>();
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    // Inspecting only.
    public string skillName;
    
    void Update()
    {
        var data = skillTable[slot.spec];
        if(data == null) skillName = "Undefined";
        else skillName = data.name;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach(var i in slot.slots)
        {
            Gizmos.DrawWireCube(i.transform.position, Vector3.one);
        }
    }
    
}
