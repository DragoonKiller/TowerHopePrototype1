using System;
using UnityEngine;

public abstract class SkillConfig : ScriptableObject
{
    // Skill range limit.
    // Use negative float for infinity range / no range limits.
    // Unity inspector does not support float? tpye.
    public float range;
    public float magicRequired;
    
    public abstract Skill Build(Protagonist x, SkillSpec spec);
    
    protected Skill UseSkill<T>(Protagonist player, Action<T> otherAction) where T : Skill
    {
        if(player.gameObject.GetComponent<T>() != null) return player.gameObject.GetComponent<T>();
        var skill = player.gameObject.AddComponent<T>();
        otherAction(skill);
        return skill;
    }
}
