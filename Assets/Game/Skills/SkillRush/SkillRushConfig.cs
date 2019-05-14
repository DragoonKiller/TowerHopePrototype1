﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillRushConfig", menuName = "ScriptableObjects/Skills/Skill Rush Config", order = 3)]
public class SkillRushConfig : SkillConfig
{
    public float speed;
    public float lifeTime;
    public float accRate;
    public float stopTime;
    public float magicConsumePerUse;
    public float stickTimeLimit;
    public float stickMagicConsumePerSec;
    public GameObject stickEffectTemplate;
    public Color activeColor;
    
    public float magicConsumePerSec => magicConsumePerUse / lifeTime;  
    
    public override Skill Build(Protagonist x, SkillSpec spec)
    {
        var skill = UseSkill<SkillRush>(x, (s) =>
        {
            s.config = this;
        });
        return skill;
    }
}
