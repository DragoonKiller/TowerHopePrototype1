using System;
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
    public GameObject stones;
    
    
}
