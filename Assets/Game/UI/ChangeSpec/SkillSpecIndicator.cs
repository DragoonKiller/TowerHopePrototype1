using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SkillSpecIndicator : MonoBehaviour
{
    public PlayerController playerController;
    
    public Color invalidColor;
    public Color validColor;
    public Color changingColor;
    
    Image image => this.GetComponent<Image>();
    
    void Update()
    {
        if(playerController.changingSpec)
        {
            image.color = changingColor;
            return;
        }
        
        if(playerController.ableToChangeSpec)
        {
            image.color = validColor;
            return;
        }
        
        image.color = invalidColor;
    }
    
}
