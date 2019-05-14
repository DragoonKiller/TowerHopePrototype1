using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDetector : MonoBehaviour
{
    Protagonist protagonist => this.GetComponent<Protagonist>();
    const int contactLimit = 20;
    ContactPoint2D[] recentContactsCache; 
    [SerializeField] int contactCount = 0;
    public IReadOnlyList<ContactPoint2D> recentContacts => new ContactList() { maintainer = this };
    
    
    
    void Update()
    {
        foreach(var i in recentContacts) Debug.DrawRay(i.point, i.normal, Color.red);
    }
    
    void FixedUpdate()
    {
        UpdateContacts();
    }
    
    void UpdateContacts()
    {
        if(recentContactsCache == null)
        {
            recentContactsCache = new ContactPoint2D[contactLimit];
            for(int i=0; i<recentContactsCache.Length; i++) recentContactsCache[i] = new ContactPoint2D();
        }
        
        contactCount = this.GetComponent<Rigidbody2D>().GetContacts(recentContactsCache);
        
        for(int i=0; i<contactCount; i++)
        {
            for(int j=i+1; j<contactCount; j++)
            {
                if(Vector2.Angle(recentContactsCache[i].normal, recentContactsCache[j].normal).LEZ())
                {
                    (recentContactsCache[i], recentContactsCache[contactCount - 1]) =
                        (recentContactsCache[contactCount - 1], recentContactsCache[i]);
                    contactCount--;
                    j--;
                }
            }
        }
    }
    
    
    struct ContactList : IReadOnlyList<ContactPoint2D>
    {
        public ContactDetector maintainer;
        
        public int Count => maintainer.contactCount;
        
        public IEnumerator<ContactPoint2D> GetEnumerator() => new Enumerator(){ index = -1, maintainer = maintainer };
        
        public ContactPoint2D this[int k]
        {
            get
            {
                if(k < 0 || k >= maintainer.contactCount) throw new IndexOutOfRangeException();
                return maintainer.recentContactsCache[k];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
        
        struct Enumerator : IEnumerator<ContactPoint2D>
        {
            public ContactDetector maintainer;
            public int index;
            public ContactPoint2D Current => maintainer.recentContactsCache[index];
            public void Reset() => index = -1;
            public bool MoveNext()
            {
                index += 1;
                if(index >= maintainer.contactCount) return false;
                return true;
            }
            public void Dispose() { }
            object IEnumerator.Current => throw new NotSupportedException();
        }
    }
}
