using System;
using UnityEngine;

/// Control skills' indicator, including range, etc.
[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SkillRangeIndicator : MonoBehaviour
{
    public Protagonist protagonist;
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
        float radius = protagonist.inventory.curWand.curSkillConfig.range;
        
        if(radius.LEZ())
        {
            rd.enabled = false;
            return;
        }
        
        rd.enabled = true;
        displayScale = radius * 3;
        Vector2 delta = Util.cursorWorldPosition - (Vector2)protagonist.transform.position;
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
