using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicRecoverManager : MonoBehaviour
{
    public Protagonist protagonist;
    public Inventory inventory;
    public ContactDetector detector;
    
    public bool allowRecover;
    
    public float staticRecoverMultiply;
    
    public float recoverRateIncreasePerSec;
    public float recoverRateDecreasePerSec;
    
    [Tooltip("After leaving the ground,\nyou still have this duration of time to recover at normal speed.")]
    public float recoverReduceDelay;
    
    [Header("Information")]
    [SerializeField] float recoverRate;
    [SerializeField] float reduceDelay;
    
    void FixedUpdate()
    {
        if(!allowRecover) return;
        
        if(detector.recentContacts.Count != 0)
        {
            recoverRate = 1f.Min(recoverRate + Time.fixedDeltaTime * recoverRateIncreasePerSec);
            reduceDelay = recoverReduceDelay;
        }
        else
        {
            reduceDelay = 0f.Max(reduceDelay - Time.fixedDeltaTime);
        }
        
        if(reduceDelay == 0f)
        {
            recoverRate = 0f.Max(recoverRate - Time.fixedDeltaTime * recoverRateDecreasePerSec);
        }
        
        foreach(var ss in inventory.curWand.stoneSlots)
        {
            var s = ss.stone;
            if(s == null) continue;
            ss.magic += staticRecoverMultiply * Time.fixedDeltaTime * s.magicRecoverRate * recoverRate;
        }
    }
    
    
}
