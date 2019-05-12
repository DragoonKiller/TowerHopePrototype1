using System;
using UnityEngine;

/// Control skills' indicator, including range, etc.
[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SkillIndicator : MonoBehaviour
{
    public PlayerState player;
    public SkillTable skills;
    
    public float displayBegin;
    
    public float displayScale;
    
    public int minPartCount;
    public int basicPartCount;
    public float width;
    public float fadeRate;
    
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    void Update()
    {
        var mat = rd.sharedMaterial;
        float radius = 0;
        switch(player.inventory.carryingWand.curSkillId)
        {
            case 0 : radius = skills["rush"].config.range; break;
            case 1 : radius = skills["bubble"].config.range; break;
            case 2 : radius = skills["flash"].config.range; break;
            default : break;
        }
        
        if(radius.LEZ() || player.curSkillState != null)
        {
            rd.enabled = false;
            return;
        }
        
        rd.enabled = true;
        displayScale = radius * 3;
        Vector2 delta = Util.cursorWorldPosition - (Vector2)player.transform.position;
        rd.transform.localScale = Vector2.one * displayScale;
        mat.SetInt("partCount", (int)(basicPartCount * radius).Max(minPartCount));
        mat.SetFloat("radius", radius / displayScale);
        mat.SetFloat("width", width / displayScale);
        mat.SetFloat("fadeRate", fadeRate / displayScale);
        mat.SetVector("currentDir", delta.normalized);
        if(delta.magnitude / radius < displayBegin) rd.color = rd.color.A(0.0f);
        else if(delta.magnitude < radius) rd.color = rd.color.A((delta.magnitude / radius - displayBegin) / (1.0f - displayBegin));
        else rd.color = rd.color.A(1.0f);
    }
    
    
}
