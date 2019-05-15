using System;
using UnityEngine;


/// Define the map from [MagicStone composition] to [Skill].
[CreateAssetMenu(fileName = "SkillTable", menuName = "ScriptableObjects/Skill Table", order = 17)]
public class SkillTable : ScriptableObject
{
    public SkillConfig[] data;
    
    public SkillConfig this[SkillSpec x]
    {
        get
        {
            foreach(var i in data) if(i.spec.SameSkill(x)) return i;
            return null;
        }
    }
    
    public SkillConfig this[string x]
    {
        get
        {
            foreach(var i in data) if(i.name.ToLower() == x.ToLower()) return i;
            return null; 
        }
    }
}
