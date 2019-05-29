using System;
using UnityEngine;

[ExecuteAlways]
public class NonCollectedStone : MonoBehaviour
{
    public Inventory inventory;
    public StoneType type;
    public StoneDisplayConfig config;
    public float maxMagic;
    public float recMagic;
    public GameObject stoneTemplate;
    
    SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
    bool shouldDestroy;
    
    void Update()
    {
        if(shouldDestroy)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        
        rd.sprite = config.GetSprite(type); 
        rd.color = config.GetColor(type);
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.layer != LayerMask.NameToLayer("Protagonist")) return;
        
        var g = Instantiate(stoneTemplate, inventory.stoneStorage.transform);
        
        var s = g.GetComponent<Stone>();
        s.maxMagic = maxMagic;
        s.magicRecoverRate = recMagic;
        s.type = type;
        
        shouldDestroy = true;
    }
}
