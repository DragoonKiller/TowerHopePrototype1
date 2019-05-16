using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Util;

/// Control the display of the whole wand, including configuring scene's display info.
[ExecuteAlways]
public class WandDisplay : MonoBehaviour
{
    // Is this wand allowed to be displayed?
    // Only used when configure the wand.
    public bool _active;
    public bool active
    {
        get => _active;
        set
        {
            _active = value;
            RefreshChildsLayer();
        }
    }
    
    SkillSlotDisplay[] skills => this.GetComponentsInChildren<SkillSlotDisplay>();
    StoneSlotDisplay[] stones => this.GetComponentsInChildren<StoneSlotDisplay>();
    
    void Update()
    {
        RefreshChildsLayer();
    }
    
    void RefreshChildsLayer()
    {
        if(active) this.gameObject.SetAllLayer(LayerMask.NameToLayer("Default"));
        else this.gameObject.SetAllLayer(LayerMask.NameToLayer("EditorOnly"));
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white.A(0.5f);
        for(int i = -1; i <= 1; i++)
        {
            Gizmos.DrawRay(this.transform.position + (Vector3)Vector2.left.RotHalfPi() * 0.1f * i, Vector2.left * 10f);
            Gizmos.DrawRay(this.transform.position + (Vector3)Vector2.right.RotHalfPi() * 0.1f * i, Vector2.right * 10f);
            Gizmos.DrawRay(this.transform.position + (Vector3)Vector2.up.RotHalfPi() * 0.1f * i, Vector2.up * 10f);
            Gizmos.DrawRay(this.transform.position + (Vector3)Vector2.down.RotHalfPi() * 0.1f * i, Vector2.down * 10f);
        }
    }
    
    
}
