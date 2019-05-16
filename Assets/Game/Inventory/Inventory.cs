using System;
using System.Collections.Generic;
using UnityEngine;

/// The inventor (bag) manager for protagonist.
/// Each item will be a *Componenet* in a sub-type.
public class Inventory : MonoBehaviour
{
    // All wands protagonist have (including the stored one).
    // Also stored the configuration of each wand.
    public GameObject wands;
    
    // The wand protagonist is using.
    public Wand curWand;
    
    // Magic stones that protagonist have (including mounted stones) will stored as components here.
    public GameObject stoneStorage;
     
    public List<Stone> stones
    {
        get
        {
            var res = new List<Stone>();
            void TakeStone(GameObject x) => res.Add(x.GetComponent<Stone>());
            stoneStorage.ForeachChild(TakeStone);
            return res;
        }
    }
    
    public List<Stone> StonesOfType(StoneType type) => stones.Filter((x) => x.type == type);
    
    public List<Stone> unusedStones
    {
        get
        {
            if(curWand == null) return stones;
            var used = new HashSet<Stone>(curWand.stoneSlots.Map((x) => x.stone));
            return stones.Filter((x) => !used.Contains(x));
        }
    }
    
    public List<Stone> UnusedStoneOfType(StoneType type) => unusedStones.Filter((x) => x.type == type);
    
}
