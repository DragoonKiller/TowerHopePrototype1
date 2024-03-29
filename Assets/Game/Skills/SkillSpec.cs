using System;
using UnityEngine;

[Serializable]
public struct SkillSpec : IEquatable<SkillSpec>
{
    public const int maxCount = 5;
    
    public StoneType a;
    public StoneType b;
    public StoneType c;
    public StoneType d;
    public StoneType e;
    
    public StoneType this[int k]
    {
        get
        {
            switch(k)
            {
                case 0 : return a;
                case 1 : return b;
                case 2 : return c;
                case 3 : return d;
                case 4 : return e;
                default : break;
            }
            return StoneType.None;
        }
        
        set
        {
            switch(k)
            {
                case 0 : a = value; return;
                case 1 : b = value; return;
                case 2 : c = value; return;
                case 3 : d = value; return;
                case 4 : e = value; return;
                default : break;
            }
        }
    }
    
    public int count =>
        + (a == StoneType.None ? 0 : 1)
        + (b == StoneType.None ? 0 : 1)
        + (c == StoneType.None ? 0 : 1)
        + (d == StoneType.None ? 0 : 1)
        + (e == StoneType.None ? 0 : 1);
    
    public bool valid => count != 0;
    
    public int Count(StoneType type)
    {
        int res = 0;
        for(int i=0; i<maxCount; i++) if(this[i] == type) res++;
        return res;
    }
    
    public bool Equals(SkillSpec spec)
    {
        unsafe
        {
            var cur = stackalloc StoneType[maxCount] { a, b, c, d, e };
            var oth = stackalloc StoneType[maxCount] { spec.a, spec.b, spec.c, spec.d, spec.e };
            var used = stackalloc bool[maxCount] { false, false, false, false, false };
            for(int i=0; i<maxCount; i++) if(cur[i] != StoneType.None)
            {
                bool found = false;
                for(int j=0; j<maxCount; j++) if(oth[j] != StoneType.None && !used[j] && cur[i] == oth[j])
                {
                    used[j] = true;
                    found = true;
                    break;
                }
                
                if(!found) return false;
            }
            for(int i=0; i<maxCount; i++) if(oth[i] != StoneType.None && !used[i]) return false;
            return true;
        }
    }
    
    public override bool Equals(object o) => o is SkillSpec ? Equals(o) : false;
    
    public override int GetHashCode()
    {
        int ah = a.GetHashCode();
        int bh = b.GetHashCode();
        int ch = c.GetHashCode();
        int dh = d.GetHashCode();
        int eh = e.GetHashCode();
        return (ah + bh + ch + dh + eh) - (ah ^ bh ^ ch ^ dh ^ eh);
    }
    
    public static bool operator==(SkillSpec x, SkillSpec y) => x.Equals(y);
    public static bool operator!=(SkillSpec x, SkillSpec y) => !x.Equals(y);
    
    public bool SameSkill(SkillSpec x)
    {
        int a = 0, b = 0;
        for(int i=0; i<maxCount; i++) if(this[i] == StoneType.Spirit) a++;
        for(int i=0; i<maxCount; i++) if(this[i] == StoneType.Power) b++;
        for(int i=0; i<maxCount; i++) if(x[i] == StoneType.Spirit) a--;
        for(int i=0; i<maxCount; i++) if(x[i] == StoneType.Power) b--;
        return a == 0 && b == 0;
    }
    
    public override string ToString()
        => "[" + a.ToString() + ","
        + b.ToString() + ","
        + c.ToString() + ","
        + d.ToString() + ","
        + e.ToString() + "]";
         
}
