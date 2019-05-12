using System;
using System.Collections.Generic;

public static partial class Util
{
    public static T GetOrDefault<R, T>(this Dictionary<R, T> dict, R key, T defaultVal)
    {
        if(dict.TryGetValue(key, out T val)) return val;
        dict.Add(key, defaultVal);
        return default;
    }
    
    public static T GetOrDefault<R, T>(this Dictionary<R, T> dict, R key)
        where T : new()
    {
        if(dict.TryGetValue(key, out T val)) return val;
        var res = new T();
        dict.Add(key, res);
        return res;
    }
    
    public static void AddIntRange(this List<int> ls, int from, int to)
    {
        for(int i = from; i <= to; i++) ls.Add(i);
    }
    
    
    public static List<T> Map<T, F>(this List<F> s, Func<F, T> f)
    {
        var x = new List<T>();
        foreach(var i in s) x.Add(f(i));
        return x;
    }
    
    public static void Map<F, T>(this ICollection<F> s, ICollection<T> t, Func<F, T> f)
    {
        foreach(var i in s) t.Add(f(i));
    }
    
    public static T FindOrDefault<T>(this T[] s, Predicate<T> f, T def)
    {
        for(int i=0; i<s.Length; i++) if(f(s[i])) return s[i];
        return def;
    }
    
    public static T FindOrNew<T>(this T[] s, Predicate<T> f) where T : new()
    {
        for(int i=0; i<s.Length; i++) if(f(s[i])) return s[i];
        return new T();
    }
    
    public static int FindIndex<T>(this T[] s, Predicate<T> f)
    {
        for(int i=0; i<s.Length; i++) if(f(s[i])) return i;
        return -1;
    }
}
