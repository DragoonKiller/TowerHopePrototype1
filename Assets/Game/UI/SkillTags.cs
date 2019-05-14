using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SkillTags : MonoBehaviour
{
    public Inventory inventory;
    public SkillTable skills;
    
    Image image => this.GetComponent<Image>();
    
    void Update()
    {
        image.sprite = skills[inventory.carryingWand.curSkillSpec].sprite;
    }
}
