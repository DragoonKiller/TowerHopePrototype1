using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterVulkflyConfig", menuName = "ScriptableObjects/Monsters/Monster Vulkfly Config", order = 7)]
public class VulkflyConfig : ScriptableObject
{
    public GameObject bulletTemplate;
    public GameObject bulletExplodeTemplate;
    
    [Header("Skill Changing rules")]
    
    // The posibility deciding which skill to use.
    public float strikePosibility;
    // public float shotPosibility;
    
    // Skills will change in a random time between...
    public float minSkillChangeTime;
    public float maxSkillChangeTime;
    
    [Header("Ordinaary Move")]
    
    // The distance that make vulkfly approch protagonist.
    public float alertDistance;
    
    // If vulkfly chases protagonist too long, it will be forced go back.
    public float chasingTimeLimit;
    
    // Speed of ordinary vulkfly move.
    public float speed;
    
    public float angularSpeed;
    
    public Color idleColor;
    public Color ordinaryColor;
    
    [Header("Skill Strike")]
    
    // The distance that can perform strike.
    public float strikeRange;
    
    // Speed of vulkfly strike.
    public float strikeSpeed;
    
    // Chant time for strike.
    public float strikeChantSec;
    
    public Color strikeColor;
    
    [Header("Skill Shot")]
    
    // The distance that can perform shot.
    public float shotRange;
    
    // Speed of vulkfly bullet.
    public float bulletSpeed;
    
    public float bulletLifeTime;
    public float bulletAngularSpeed;
    
    // Chant time for shot.
    public float shotChantSec;
    public float shotRecoverSec;
    
    public int shotCount;
    public float shotDelay;
    public float bulletAngleRange;
    
    public Color shotColor;
    
}
