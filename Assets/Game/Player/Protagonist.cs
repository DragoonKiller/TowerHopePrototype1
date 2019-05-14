using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls the behaviour of a player.
public class Protagonist : MonoBehaviour
{
    public Inventory inventory;
    
    public GameObject reviveBeacon;
    
    public Skill curSkillState;
    
    // Is there any script that required the overall control of protagonist?
    public Util.UniqueRegister<MonoBehaviour> requireControl = new Util.UniqueRegister<MonoBehaviour>();
    
    const int contactLimit = 20;
    ContactPoint2D[] recentContactsCache; 
    [SerializeField] int contactCount = 0;
    public IReadOnlyList<ContactPoint2D> recentContacts => new ContactList(){ player = this };
    
    public Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    public PolygonCollider2D col => this.GetComponentInChildren<PolygonCollider2D>();
    public SpriteRenderer sprite => this.GetComponentInChildren<SpriteRenderer>();
    public TrailRenderer trail => this.GetComponentInChildren<TrailRenderer>();
    
    
    public bool TryConsumeMagic(float x) => inventory.carryingWand.curSlot.TryConsumeMagic(x);
    public void ConsumeMagic(float x) => inventory.carryingWand.curSlot.ConsumeMagic(x);
    public void AddMagic(float x) => inventory.carryingWand.curSlot.AddMagic(x);
    
    void Start()
    {
        reviveBeacon.transform.position = this.transform.position;
    }
    
    void Update()
    {
        foreach(var i in recentContacts) Debug.DrawRay(i.point, i.normal, Color.red);
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        // Only Monsters and MonsterBullets can hurt protagonist.
        if(((1 << c.collider.gameObject.layer) & LayerMask.GetMask("Monster", "MonsterBullet")) == 0) return;
        
        DestroyPlayer();
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
    
    /// Player is dead and should be revived.
    /// The player object will not be removed but behaviours are limited.
    void DestroyPlayer()
    {
        if(curSkillState != null) Destroy(curSkillState);
        var rev = this.gameObject.AddComponent<PlayerReviving>();
        rev.reviveBeacon = reviveBeacon;
    }
    
    // ============================================================================================
    // Internal structures and classes
    // ============================================================================================
    
    struct ContactList : IReadOnlyList<ContactPoint2D>
    {
        public Protagonist player;
        
        public int Count => player.contactCount;
        
        public IEnumerator<ContactPoint2D> GetEnumerator() => new Enumerator(){ index = -1, player = player };
        
        public ContactPoint2D this[int k]
        {
            get
            {
                if(k < 0 || k >= player.contactCount) throw new IndexOutOfRangeException();
                return player.recentContactsCache[k];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
        
        struct Enumerator : IEnumerator<ContactPoint2D>
        {
            public Protagonist player;
            public int index;
            public ContactPoint2D Current => player.recentContactsCache[index];
            public void Reset() => index = -1;
            public bool MoveNext()
            {
                index += 1;
                if(index >= player.contactCount) return false;
                return true;
            }
            public void Dispose() { }
            object IEnumerator.Current => throw new NotSupportedException();
        }
    }
}
