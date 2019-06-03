using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VulkflyBulletExplode : MonoBehaviour
{
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    
    public float fadeTime;
    public float timer;
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > fadeTime)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        
        rd.color = rd.color.A(1f - timer / fadeTime);
    }
}
