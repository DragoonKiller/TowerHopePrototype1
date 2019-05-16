using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillNoneConfig", menuName = "ScriptableObjects/Skills/Skill None Config", order = -1)]
public class SkillNoneConfig : SkillConfig
{
    public override Type skillType => throw new NotSupportedException();
    public override bool isNone => true;
}
