using System;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogData[] data;
    
    
    public bool shouldDestroy;
    void Update()
    {
        if(shouldDestroy) DestroyImmediate(this.gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        // Collided one is not protagonist.
        if(c.gameObject.layer != LayerMask.NameToLayer("Protagonist")) return;
        
        foreach(var d in data) Dialog.inst.AddDialog(d.text, d.duration);
        
        shouldDestroy = true;
    }
    
    
    
}
