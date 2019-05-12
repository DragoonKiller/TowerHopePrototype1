using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SkillTags : MonoBehaviour
{
    public PlayerState player;
    public Image[] images;
    public Color activeColor;
    public Color deactiveColor;
    void Update()
    {
        for(int i = 0; i < images.Length.Min(player.inventory.carryingWand.skillsSlots.Length); i++)
        {
            if(player.inventory.carryingWand.curSkillId == i) images[i].color = activeColor;
            else images[i].color = deactiveColor;
        }
    }
}
