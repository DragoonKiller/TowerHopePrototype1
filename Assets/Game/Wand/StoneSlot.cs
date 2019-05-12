using System;
using UnityEngine;

/// The descriptor for stone slots.
public class StoneSlot : MonoBehaviour
{
    public float magic
    {
        get => curMagic;
        set => curMagic = stone == null ? 0 : value.Clamp(0f, stone.maxMagic);
    }
    
    public float maxMagic => stone == null ? 0 : stone.maxMagic;
    
    public bool empty => stone == null;
    
    public StoneType stoneType => stone == null ? StoneType.None : stone.type;
    
    // The mounted magic stone.
    public Stone stone;
    
    // The real magic.
    [SerializeField] float curMagic;
    
    void FixedUpdate()
    {
        if(stone != null) magic += stone.magicRecoverRate * Time.fixedDeltaTime;
    }
}
