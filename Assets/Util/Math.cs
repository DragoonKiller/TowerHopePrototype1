using System;
using System.Runtime.CompilerServices;

using UnityEngine;

public static partial class Util
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ModSys(this int i, int mod)
    {
        if(i < 0) return i % mod == 0 ? 0 : i % mod + mod;
        if(i >= mod) return i % mod;
        return i;
    }
    
    
    public static float eps => 1e-6f;
    
    /// Float equal in linear space.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Feq(this float a, float b) => Mathf.Approximately(a, b);
    
    
    
    /// Less than zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LZ(this float x) => x <= -eps;
    
    /// Greater than zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GZ(this float x) => x >= eps;
    
    /// Less or equal to zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LEZ(this float x) => x <= eps;
    
    /// Greater or equal to zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GEZ(this float x) => x >= -eps;
    
    /// Equal to zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EZ(this float x) => x.LEZ() && x.GEZ();
    
    /// Not equal to zero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NZ(this float x) => x.LZ() || x.GZ();
    
    /// Less or equal to.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LE(this float x, float y) => x <= y + eps;
    
    /// Greater or equal to.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GE(this float x, float y) => x >= y - eps;
    
    /// Less.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool L(this float x, float y) => x <= y - eps;
    
    /// Greater.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool G(this float x, float y) => x >= y + eps;
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(this int a, int l, int r) => a < l ? l : a > r ? r : a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(this float a, float l, float r) => a < l ? l : a > r ? r : a;
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Xmap(this float x, float l, float r, float a, float b) => (x - l) / (r - l) * (b - a) + b;
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Abs(this float x) => Mathf.Abs(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(this int x) => Mathf.Abs(x);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdMax(this ref float x, float y) => x = Mathf.Max(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdMin(this ref float x, float y) => x = Mathf.Min(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(this float x, float y) => Mathf.Max(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(this float x, float y) => Mathf.Min(x, y);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdMax(this ref int x, int y) => x = Mathf.Max(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdMin(this ref int x, int y) => x = Mathf.Min(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(this int x, int y) => Mathf.Max(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(this int x, int y) => Mathf.Min(x, y);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sin(this float x) => Mathf.Sin(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cos(this float x) => Mathf.Cos(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Tan(this float x) => Mathf.Tan(x);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ceil(this float x) => Mathf.Ceil(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(this float x) => Mathf.Round(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Floor(this float x) => Mathf.Floor(x);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CeilToInt(this float x) => Mathf.CeilToInt(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RoundToInt(this float x) => Mathf.RoundToInt(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FloorToInt(this float x) => Mathf.FloorToInt(x);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow(this float x, float y) => Mathf.Pow(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow(this int x, float y) => Mathf.Pow(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow(this float x, int y) => Mathf.Pow(x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Pow(this int x, int y) => (int)Math.Pow(x, y);
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sqrt(this float x) => Mathf.Sqrt(x);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sqrt(this int x) => Mathf.Sqrt(x);
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sqr(this float x) => x * x;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sqr(this int x) => x * x;
    
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sgn(this float x) => x.LZ() ? -1 : x.GZ() ? 1 : 0;
    
    
}
