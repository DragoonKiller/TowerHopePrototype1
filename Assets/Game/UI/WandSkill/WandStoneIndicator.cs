using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class WandStoneIndicator : MonoBehaviour
{
    public StoneDisplayConfig config;
    public Inventory inventory;
    public GameObject[] displayers;
    
    Wand wand => inventory.carryingWand;
    
    void Update()
    {
        var curSkillSlot = wand.curSlot;
        int cc = 0;
        for(int i=0; i < SkillSpec.maxCount && i < wand.curSlot.slotCount && cc < displayers.Length; i++)
        {
            if(curSkillSlot[i].stoneType != StoneType.None)
            {
                var type = curSkillSlot[i].stoneType;
                var g = displayers[cc++];
                g.Activate();
                g.GetComponentInChildren<Image>().color = config.GetColor(type);
                g.GetComponentInChildren<Image>().sprite = config.GetSprite(type);
                g.GetComponentInChildren<Slider>().value = curSkillSlot[i].magic / curSkillSlot[i].maxMagic;
            }
        }
        
        for(int i = cc; i < displayers.Length; i++) displayers[i].Deactive();
        
        for(int i=0; i<displayers.Length; i++)
        {
            var tr = displayers[i].GetComponent<RectTransform>();
            Util.DebugDrawRect(tr.position, tr.rect.size, Color.green);
        }
    }
    
}
