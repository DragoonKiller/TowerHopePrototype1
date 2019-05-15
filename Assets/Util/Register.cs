using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Util
{
    public class Register<T> where T : class
    {
        readonly List<T> list = new List<T>();
        
        public void Add(T x)
        {
            if(list.Contains(x)) throw new InvalidOperationException("Try to register one object multiple times into a Register.");
            list.Add(x);
        }
        
        public void Remove(T x)
        {
            if(!list.Remove(x)) throw new InvalidOperationException("Try to remove non-exist object from Register.");
        }
        
        public bool Registered(T x)
        {
            foreach(var i in list) if(x == i) return true;
            return false;
        }
        
        public int count => list.Count;
        public bool empty => list.Count == 0;
    }
    
    public class UniqueRegister<T> where T : class
    {
        T value;
        
        public void Register(T x)
        {
            if(value != null) throw new InvalidOperationException("Try to register multiple object into UniqueRegister.");
            value = x;
        }
        
        public void Remove(T x) 
        {
            if(value != x) throw new InvalidOperationException("Try to remove non-exist object from UniqueRegister.");
            value = null;
        }
        
        public bool Registered(T x) => value == x;
        
        public bool registered => value != null;
    }
}
