using System;
using System.Collections;
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
    
    public override Type skillType => typeof(SkillRush);
    
    public float magicConsumePerSec => magicConsumePerUse / lifeTime;  
}
