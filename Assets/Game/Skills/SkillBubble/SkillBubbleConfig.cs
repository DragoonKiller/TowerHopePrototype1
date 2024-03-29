﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillBubbleConfig", menuName = "ScriptableObjects/Skills/Skill Bubble Config", order = 4)]
public class SkillBubbleConfig : SkillConfig
{
    public GameObject bubbleSource;
    public float speed;
    public float accPerPhysicsFrame;
    public float magicConsumePerSec;
    public float collisionSpeed;
    
    public override Type skillType => typeof(SkillBubble);
    
}
