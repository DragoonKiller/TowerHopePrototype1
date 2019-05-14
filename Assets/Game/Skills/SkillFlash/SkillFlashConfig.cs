using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillFlashConfig", menuName = "ScriptableObjects/Skills/Skill Flash Config", order = 2)]
public class SkillFlashConfig : SkillConfig
{
    public float hideTime;
    public float appearTime;
    public float maxDist;
    public float swapDist;
    public float collisionPreventingDist;
    public float magicConsumePerUse;
    public float magicConsumePerSwap;
    public GameObject destinationAbsorb;
    public float renderScales;
    
    public override Skill Build(Protagonist x, SkillSpec spec) => UseSkill<SkillFlash>(x, (s) => { s.config = this; });
}
