using System;
using UnityEngine;

public static partial class Util
{
    public static void Activate(this GameObject x) { if(!x.activeSelf) x.SetActive(true); }
    public static void Deactive(this GameObject x) { if(x.activeSelf) x.SetActive(false); }
    
    
    public static void SetAllLayer(this Transform x, int layer) => x.gameObject.SetAllLayer(layer);
    public static void SetAllLayer(this GameObject x, int layer)
    {
        void Bind(GameObject y) => y.SetAllLayer(layer);
        x.layer = layer;
        x.gameObject.ForeachChild(Bind);
    }
}
