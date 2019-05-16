using System;
using UnityEngine;

public abstract class SkillConfig : ScriptableObject
{
    public string skillName;
    public SkillSpec spec;
    public Sprite sprite;
    
    // Skill range limit.
    // Use negative float for infinity range / no range limits.
    // Unity inspector does not support float? tpye.
    public float range;
    public float magicRequired;
    
    public float rangePerIndicatorStone;
    public float efficiencyPerNatureStone;
    
    public abstract Type skillType { get; }
    
    public virtual bool isNone => false;
}
