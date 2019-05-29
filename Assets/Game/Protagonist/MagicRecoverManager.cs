using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicRecoverManager : MonoBehaviour
{
    public Protagonist protagonist;
    public Inventory inventory;
    public ContactDetector detector;
    
    public float recoverRateChangePerSec;
    public float recoverReduceDelay;
    
    [Header("Information")]
    [SerializeField] float recoverRate;
    [SerializeField] float reduceDelay;
    
    void FixedUpdate()
    {
        if(detector.recentContacts.Count != 0)
        {
            recoverRate = 1f.Min(recoverRate + Time.fixedDeltaTime * recoverRateChangePerSec);
            reduceDelay = recoverReduceDelay;
        }
        else
        {
            reduceDelay = 0f.Max(reduceDelay - Time.fixedDeltaTime);
        }
        
        if(reduceDelay == 0f)
        {
            recoverRate = 0f.Max(recoverRate - Time.fixedDeltaTime * recoverRateChangePerSec);
        }
        
        foreach(var ss in inventory.curWand.stoneSlots)
        {
            var s = ss.stone;
            if(s == null) continue;
            ss.magic += Time.fixedDeltaTime * s.magicRecoverRate * recoverRate;
        }
    }
    
    
}
