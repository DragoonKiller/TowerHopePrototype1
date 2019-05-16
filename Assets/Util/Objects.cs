using System;

public static partial class Util
{
    public static T GetOrInit<T>(ref T x) where T : class, new()
    {
        if(x == null) x = new T();
        return x;
    }
    
    public static T GetOrInit<T>(ref T x, Func<T> creator) where T : class
    {
        if(x == null) x = creator();
        return x;
    }
    
}
