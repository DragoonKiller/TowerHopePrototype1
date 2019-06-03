using System;
using UnityEngine;

[ExecuteAlways]
public class InventoryAssistant : MonoBehaviour
{
    public Inventory inventory;
    public GameObject stoneTemplate;
    
    [Header("Add stones")]
    public bool doAdd;
    public StoneType stoneType;
    public float stoneMaxMagic;
    public float stoneRecMagic;
    
    [Header("Modify all stone's pproperty")]
    public bool doModifyProperty;
    public float magicAdd;
    public float magicMult;
    public float recoverAdd;
    public float recoverMult;
    
    void Update()
    {
        if(doAdd)
        {
            doAdd = false;
            AddStone();
        }
        
        if(doModifyProperty)
        {
            doModifyProperty = false;
            ModifyStoneProperty();
        }
    }
    
    void AddStone()
    {
        var g = Instantiate(stoneTemplate, inventory.stoneStorage.transform);
        g.name = "Stone";
        
        stoneType = StoneType.None;
        stoneMaxMagic = 0f;
        stoneRecMagic = 0f;
    }
    
    void ModifyStoneProperty()
    {
        foreach(var s in Component.FindObjectsOfType<Stone>())
        {
            s.maxMagic = s.maxMagic * magicMult + magicAdd;
            s.magicRecoverRate = s.magicRecoverRate * recoverMult + recoverAdd;
        }
        
        foreach(var s in Component.FindObjectsOfType<NonCollectedStone>())
        {
            s.maxMagic = s.maxMagic * magicMult + magicAdd;
            s.recMagic = s.recMagic * recoverMult + recoverAdd;
        }
        
        magicAdd = recoverAdd = 0f;
        magicMult = recoverMult = 1f;
    }
}
