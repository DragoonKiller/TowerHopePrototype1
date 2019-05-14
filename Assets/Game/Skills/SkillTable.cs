using System;
using UnityEngine;


/// Define the map from [MagicStone composition] to [Skill].
[CreateAssetMenu(fileName = "SkillTable", menuName = "ScriptableObjects/Skill Table", order = 17)]
public class SkillTable : ScriptableObject
{
    [Serializable]
    public class SkillData
    {
        public string name;
        public SkillSpec spec;
        public Sprite sprite;
        public SkillConfig config;
    }
    
    public SkillData[] data;
    
    public SkillData this[SkillSpec x]
    {
        get
        {
            foreach(var i in data) if(i.spec.SameSkill(x)) return i;
            return null;
        }
    }
    
    public SkillData this[string x]
    {
        get
        {
            foreach(var i in data) if(i.name.ToLower() == x.ToLower()) return i;
            return null; 
        }
    }
    
    public Skill BuildFromSpec(SkillSpec spec, Protagonist x)
    {
        foreach(var i in data) if(i.spec.SameSkill(spec)) return i.config.Build(x, spec);
        return null;
    }
}
