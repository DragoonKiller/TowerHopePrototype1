using System;
using UnityEngine;

public static partial class Util
{
    public static Color R(this Color v, float r) => new Color(r, v.g, v.b, v.a);
    public static Color G(this Color v, float g) => new Color(v.r, g, v.b, v.a);
    public static Color B(this Color v, float b) => new Color(v.r, v.g, b, v.a);
    public static Color A(this Color v, float a) => new Color(v.r, v.g, v.b, a);
    
    
    public static Color R(this Color v, Color h) => new Color(h.r, v.g, v.b, v.a);
    public static Color G(this Color v, Color h) => new Color(v.r, h.g, v.b, v.a);
    public static Color B(this Color v, Color h) => new Color(v.r, v.g, h.b, v.a);
    public static Color A(this Color v, Color h) => new Color(v.r, v.g, v.b, h.a);
    
}
