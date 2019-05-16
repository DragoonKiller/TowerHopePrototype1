using System;
using UnityEditor;
using UnityEngine;

/// This script is for displaying stones when configuraing your wand.
[ExecuteAlways]
public class StoneSlotDisplay : MonoBehaviour
{
    public StoneDisplayConfig config;
    StoneSlot slot => this.GetComponent<StoneSlot>();
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    void Update()
    {
        var type = slot.stoneType;
        rd.sprite = config.GetSprite(type);
        rd.color = config.GetColor(type);
    }
    
}
