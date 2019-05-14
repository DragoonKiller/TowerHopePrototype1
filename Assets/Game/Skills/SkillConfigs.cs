using System;
using UnityEngine;

public abstract class SkillConfig : ScriptableObject
{
    // Skill range limit.
    // Use negative float for infinity range / no range limits.
    // Unity inspector does not support float? tpye.
    public float range;
    public float magicRequired;
    
    public float rangePerIndicatorStone;
    public float efficiencyPerNatureStone;
    
    public abstract Skill Build(Protagonist x, SkillSpec spec);
    
    protected Skill UseSkill<T>(Protagonist protagonist, Action<T> otherAction) where T : Skill
    {
        if(protagonist.gameObject.GetComponent<T>() != null) return protagonist.gameObject.GetComponent<T>();
        var skill = protagonist.gameObject.AddComponent<T>();
        otherAction(skill);
        return skill;
    }
}
