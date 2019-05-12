using System;
using UnityEngine;

[ExecuteAlways]
public class StoneSlotDisplay : MonoBehaviour
{
    public WandDisplayConfig config;
    StoneSlot slot => this.GetComponent<StoneSlot>();
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    void Update()
    {
        var type = slot.stoneType;
        rd.sprite = config.GetSprite(type);
        rd.color = config.GetColor(type);
    }
}
