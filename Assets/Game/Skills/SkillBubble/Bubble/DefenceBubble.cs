using System;
using UnityEngine;

public class DefenceBubble : MonoBehaviour
{
    public SkillSpec spec;
    public SkillBubbleConfig config;
    public Protagonist protagonist;
    
    void Start()
    {
        this.transform.localScale = Vector3.one * (1.0f + config.rangePerIndicatorStone * spec.Count(StoneType.Indicator));
    }
    
    void Update()
    {
        this.transform.position = protagonist.transform.position;
    }
    
    void FixedUpdate()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        if(((1 << c.collider.gameObject.layer) & LayerMask.GetMask("MonsterBullet")) == 0) return;
        
        Vector2 avgContact = Vector2.zero;
        for(int i=0; i<c.contactCount; i++) avgContact += c.GetContact(i).point;
        avgContact /= c.contactCount;
        
        protagonist.rd.velocity += config.collisionSpeed * -((Vector2)this.transform.position).To(avgContact);
    }
}
