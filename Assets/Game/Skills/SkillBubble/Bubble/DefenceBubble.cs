using System;
using UnityEngine;

public class DefenceBubble : MonoBehaviour
{
    public SkillBubbleConfig config;
    public Protagonist player;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        this.transform.position = player.transform.position;
    }
    
    void FixedUpdate()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        var bullet = c.collider.GetComponent<MonsterBullet>();
        if(bullet == null) return;
        
        Vector2 avgContact = Vector2.zero;
        for(int i=0; i<c.contactCount; i++) avgContact += c.GetContact(i).point;
        avgContact /= c.contactCount;
        
        player.rd.velocity += config.collisionSpeed * -((Vector2)this.transform.position).To(avgContact);
    }
}
