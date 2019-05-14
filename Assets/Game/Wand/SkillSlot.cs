using System;
using UnityEngine;

/// The descriptor for skill slot.
/// Collect plugged stones' information.
public class SkillSlot : MonoBehaviour
{
    
    // The slots for this skill.
    // Multiple skill slots may use the same slot.
    public StoneSlot[] slots;
    
    public int slotCount => slots.Length;
    
    public StoneSlot this[int k] => slots[k];
    
    public int stoneCount
    {
        get
        {
            int cc = 0;
            for(int i=0; i<slots.Length; i++) if(!slots[i].empty) cc++;
            return cc;
        }
    }
    
    public SkillSpec spec
    {
        get
        {
            var res = new SkillSpec();
            for(int i=0; i<slots.Length; i++) res[i] = slots[i].stoneType;
            return res;
        }
    }
    
    public float magic
    {
        get
        {
            // No magic if no stone.
            if(stoneCount == 0) return 0f;
            
            float x = 1e6f;
            foreach(var sl in slots) if(!sl.empty) x.UpdMin(sl.magic);
            return x;
        }
    }
    
    public float maxMagic
    {
        get
        {
            // No magic if no stone.
            if(stoneCount == 0) return 0f;
            
            float x = 1e6f;
            foreach(var sl in slots) if(!sl.empty) x.UpdMin(sl.maxMagic);
            return x;
        }
    }
    
    /// Add magic to all stones of this slots.
    public void AddMagic(float x)
    {
        float consume = x / stoneCount;
        foreach(var s in slots) s.magic += consume; 
    }
    
    /// Consume magic to all stones of this slots.
    /// If consumption is greater than the rest magic, they will be ignored.
    public void ConsumeMagic(float x) => AddMagic(-x);
    
    /// Consume magic to all stones of this slots, or do nothing if magic is not enough.
    public bool TryConsumeMagic(float x)
    {
        if(magic < x) return false;
        ConsumeMagic(x);
        return true;
    }
    
}
