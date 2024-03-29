using System;
using UnityEngine;

public abstract class Wand : MonoBehaviour
{
    public SkillTable skillTable;
    public StoneSlot[] stoneSlots;
    public SkillSlot[] skillsSlots;
    
    // Notice the inheritance Wand script will always unable to access the internal variable.
    [SerializeField] private int _curSkillId;
    public int curSkillId
    {
        get => _curSkillId;
        set => _curSkillId = value.ModSys(skillsSlots.Length.Max(1));
    }
    
    public SkillSlot curSlot => skillsSlots[curSkillId];
    public SkillConfig curSkillConfig => skillTable[curSkillSpec];
    public SkillSpec curSkillSpec => curSlot.spec;
    public WandDisplay display => this.GetComponent<WandDisplay>();
    public bool skillPrepared => curSlot.magic >= curSkillConfig.magicRequired / curSlot.stoneCount;
}
