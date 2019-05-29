using System;
using UnityEngine;

public class RemoveDialogTrigger : MonoBehaviour
{
    public DialogTrigger removeTarget;
    
    bool shouldDestroy;
    
    void Update()
    {
        if(shouldDestroy) DestroyImmediate(this.gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.layer != LayerMask.NameToLayer("PRotagonist")) return;
        
        removeTarget.shouldDestroy = true;
    }
}
