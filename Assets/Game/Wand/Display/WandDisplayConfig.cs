using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotDisplayConfig", menuName = "ScriptableObjects/Slot Display Configs", order = 19)]
public class WandDisplayConfig : ScriptableObject
{
    [Serializable]
    public struct StoneSprite
    {
        public StoneType type;
        public Sprite sprite;
        public Color color;
    }
    
    public StoneSprite[] sprites;
    
    public Sprite GetSprite(StoneType type)
    {
        foreach(var s in sprites) if(s.type == type) return s.sprite;
        return null;
    }
    
    public Color GetColor(StoneType type)
    {
        foreach(var s in sprites) if(s.type == type) return s.color;
        return Color.black.A(0);
    }
}
