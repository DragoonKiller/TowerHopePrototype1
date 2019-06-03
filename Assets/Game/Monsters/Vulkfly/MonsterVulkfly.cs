using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The AI of vulkfly.
public class MonsterVulkfly : Monster
{
    public VulkflyConfig config;
    public Protagonist protagonist;
    
    [Header("Internal Information")]
    [SerializeField] Vector2 origin;
    [SerializeField] bool idle;
    [SerializeField] Vector2 targetFacing;
    
    SpriteRenderer sp => this.GetComponent<SpriteRenderer>();
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    TrailRenderer[] trails => this.GetComponentsInChildren<TrailRenderer>();
    
    bool alert => (protagonist.transform.position - this.transform.position).magnitude <= config.alertDistance;
    bool stayNearOrigin => ((Vector2)this.transform.position - origin).magnitude < config.speed * Time.deltaTime;
    
    void Start()
    {
        origin = this.transform.position;
        sp.color = config.idleColor;
    }
    
    void Update()
    {
        Util.EditorDrawCircle(this.transform.position, config.alertDistance, 32, Color.red.A(0.6f));
        
        if(idle && alert)
        {
            idle = false;
            StartCoroutine("BehaviourApproch");
        }
        
        RotateToTargetFacing();
    }
    
    void RotateToTargetFacing()
    {
        var a = Vector2.SignedAngle(this.transform.rotation * Vector2.down, targetFacing);
        a = a.Sgn() * a.Abs().Clamp(0f, config.angularSpeed * Time.deltaTime);
        this.transform.rotation *= Quaternion.Euler(0, 0, a);
    }
    
    IEnumerator BehaviourApproch()
    {
        // Debug.Log("Approch...");
        // Setup a timer for chasing times limit AND changing skills...
        float t = 0;
        float nxtSkillTime = 0;
        int curSkill = 0; // 0 => strike. 1 => shot.
        while(t < config.chasingTimeLimit)
        {
            t += Time.deltaTime;
            
            // Protagonist out of range, return.
            if(!alert)
            {
                StartCoroutine("BehaviourReturn");
                yield break;
            }
            
            if(t >= nxtSkillTime)
            {
                if(UnityEngine.Random.Range(0f, 1f) <= config.strikePosibility) curSkill = 0;
                else curSkill = 1;
                
                nxtSkillTime += UnityEngine.Random.Range(config.minSkillChangeTime, config.maxSkillChangeTime);
            }
            
            Vector2 diff = protagonist.transform.position - this.transform.position;
            rd.velocity = diff.normalized * config.speed;
            targetFacing = diff;
            
            if(curSkill == 0 && diff.magnitude <= config.strikeRange)
            {
                StartCoroutine("BehaviourStrike");
                yield break;
            }
            
            if(curSkill == 1 && diff.magnitude <= config.shotRange)
            {
                StartCoroutine("BehaviourShot");
                yield break;
            }
            
            yield return null;
        }
        
        // Time's up, go back directly.
        StartCoroutine("BehaviourReturn");
        yield break;
    }
    
    IEnumerator BehaviourStrike()
    {
        // Debug.Log("Strke begin");
        
        sp.color = config.strikeColor;
        
        // Record protagonist's positon.
        Vector2 diff = protagonist.transform.position - this.transform.position;
        
        // And wait for some time...
        for(float t = 0; t < config.strikeChantSec; t += Time.deltaTime)
        {
            RotateToTargetFacing();
            yield return null;
        }
        
        // Setup speed. The speed will not be updated by this script further more,
        //   allow the vulkfly interact with terrains.
        rd.velocity = diff.normalized * config.strikeSpeed;
        targetFacing = diff;
        
        float expectedTime = diff.magnitude / config.strikeSpeed;
        yield return new WaitForSeconds(expectedTime);
        
        // Debug.Log("Strke end");
        SetIdle();
        yield break;
    }
    
    IEnumerator BehaviourShot()
    {
        // Debug.Log("Shot begin");
        sp.color = config.shotColor;
        
        // Wait for some time...
        for(float t = 0; t < config.strikeChantSec; t += Time.deltaTime)
        {
            Vector2 diff = protagonist.transform.position - this.transform.position;
            RotateToTargetFacing();
            yield return null;
        }
        
        
        for(int i=0; i<config.shotCount; i++)
        {
            var g = Instantiate(config.bulletTemplate);
            g.transform.position = this.transform.position;
            var angleBias = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-config.bulletAngleRange, config.bulletAngleRange));
            g.transform.rotation = this.transform.rotation * angleBias;
            var c = g.GetComponent<VulkflyBullet>();
            c.config = config;
            c.protagonist = protagonist;
            yield return new WaitForSeconds(config.shotDelay);
        }
        
        yield return new WaitForSeconds(config.shotRecoverSec);
        
        SetIdle();
        // Debug.Log("Shot end");
        yield break;
    }
    
    IEnumerator BehaviourReturn()
    {
        // Debug.Log("Return begin");
        sp.color = config.ordinaryColor;
        
        while(!stayNearOrigin && !alert)
        {
            Vector2 diff = origin - (Vector2)this.transform.position;
            rd.velocity = diff.normalized * config.speed;
            targetFacing = diff;
            yield return null;
        }
        
        // Debug.Log("Return end");
        
        if(alert)
        {
            StartCoroutine("BehaviourApproch");
            yield break;
        }
        
        // Face to the down side always.
        targetFacing = Vector2.down;
        rd.velocity = Vector2.zero;
        SetIdle();
        yield break;
    }
    
    void SetIdle()
    {
        idle = true;
        sp.color = config.idleColor;
    }
}
