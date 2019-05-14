using System;
using UnityEngine;

public static partial class Util
{
    public static void Activate(this GameObject x) { if(!x.activeSelf) x.SetActive(true); }
    public static void Deactive(this GameObject x) { if(x.activeSelf) x.SetActive(false); }
}
