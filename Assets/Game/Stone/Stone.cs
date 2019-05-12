using System;
using UnityEngine;

public enum StoneType : uint
{
    None        = 0,
    Spirit      = 1,            // white
    Power       = 2,            // purple
    Nature      = 4,            // green
    Indicator   = 8,            // yellow
}

/// Notice: this class is only for data storage.
/// If you want to display stones, in inventory or in world,
///   write custom renderer on InventoryDisplay or world display...
public class Stone : MonoBehaviour
{
    public StoneType type;
    
    public float maxMagic;
    
    // Magic recover, unit per physics frame.
    public float magicRecoverRate;
    
}
