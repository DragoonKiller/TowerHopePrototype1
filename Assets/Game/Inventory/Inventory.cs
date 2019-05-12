using System;
using UnityEngine;

/// The inventor (bag) manager for current player.
/// Each item will be a *Componenet* in a sub-type.
public class Inventory : MonoBehaviour
{
    // All wands player have (including the stored one).
    // Also stored the configuration of each wand.
    public GameObject wands;
    
    // The wand player is using.
    public Wand carryingWand;
    
    // Magic stones that player have (including mounted stones) will stored as components here.
    public GameObject stones;
    
    
}
