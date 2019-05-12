using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBarnacleConfig", menuName = "ScriptableObjects/Monsters/Monster Barnacle Config", order = 6)]
public class BarnacleConfig : ScriptableObject
{
    public GameObject barnacleBulletExplodeTemplate;
    public GameObject barnacleBulletTemplate;
    public float bulletLifeTime;
    public float targetSpeed;
    public float accRatePerPhysFrame;
    public float cooldown;
}
