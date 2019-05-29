﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFlash : Skill
{
    public new SkillFlashConfig config => base.config as SkillFlashConfig;
    
    public Vector2 destination;
    
    float physTimer;
    float displayTimer;
    bool transfered;
    
    [SerializeField] GameObject destinationAbsorb;
    SpriteRenderer absorbRenderer => destinationAbsorb.GetComponent<SpriteRenderer>();
    
    void Start()
    {
        destination = FindDestination();
        
        physTimer = 0f;
        transfered = false;
        
        destinationAbsorb = Instantiate(config.destinationAbsorb);
        destinationAbsorb.transform.position = destination;
        destinationAbsorb.transform.localScale = Vector2.one * config.renderScales;
        
        var mat = destinationAbsorb.GetComponent<SpriteRenderer>().material;
        mat.SetFloat("radius", config.swapDist / config.renderScales);
        mat.SetFloat("width", config.swapDist / config.renderScales / 10.0f);
        absorbRenderer.color = absorbRenderer.color.A(0f);
    }
    
    void Update()
    {
        displayTimer += Time.deltaTime;
        if(displayTimer < config.hideTime)
        {
            var rate = 1.0f - physTimer / config.hideTime;
            
            absorbRenderer.color = absorbRenderer.color.A(displayTimer / config.hideTime);
            
            // Player canstop the skill manually, if the transfer is not performed.
            if(!Input.GetKey(KeyCode.Mouse0))
            {
                DestroyImmediate(this);
                return;
            }
        }
        else
        {
            var curTime = displayTimer - config.hideTime;
            
            if(curTime.LE(config.appearTime))
            {
                absorbRenderer.color = absorbRenderer.color.A(1.0f - curTime / config.appearTime);
            }
            else
            {
                DestroyImmediate(this);
                return;
            }
        }
    }
    
    void FixedUpdate()
    {
        physTimer += Time.fixedDeltaTime;
        if(physTimer > config.hideTime)
        {
            if(!transfered)
            {
                bool swapped = Transfer();
                float magicCost = 0f.Max(1.0f - config.efficiencyPerNatureStone * spec.Count(StoneType.Nature));
                if(swapped) protagonist.inventory.curWand.curSlot.ConsumeMagic(config.magicConsumePerSwap * magicCost);
                else protagonist.inventory.curWand.curSlot.ConsumeMagic(config.magicConsumePerUse * magicCost);
                 
                this.transform.position = destination;
                transfered = true;
            }
            
            var curTime = physTimer - config.hideTime;
            if(curTime.G(config.appearTime))
            {
                DestroyImmediate(this);
                return;
            }
        }
    }
    
    void OnDestroy()
    {
        if(transfered) rd.velocity = Vector2.zero;
        DestroyImmediate(destinationAbsorb);
    }
    
    /// Swap monster's bullets.
    /// Notice this function *must* performed *before* setting transform.position to destination.
    /// Notice this swap will not trigger collision between protagonist and bullets,
    ///   since this is performed without "time proceeded".
    bool Transfer()
    {
        Util.EditorDrawCircle(destination, config.swapDist, 32, Color.red, 3f);
        
        var originSetting = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = true;
        var p = Physics2D.CircleCastAll(destination, config.swapDist, Vector2.up, Util.eps, LayerMask.GetMask("MonsterBullet"));
        Physics2D.queriesStartInColliders = originSetting;
        
        bool swapped = false;
        foreach(var h in p)
        {
            var bullet = h.collider.gameObject.GetComponent<MonsterBullet>();
            if(bullet == null) continue;
            bullet.GetComponent<Rigidbody2D>().position += (Vector2)protagonist.transform.position - destination;
            swapped = true;
        }
        return swapped;
    }
    
    Vector2 FindDestination()
    {
        Vector2 res = Vector2.zero;
        var target = Util.cursorWorldPosition;
        var cur = (Vector2)protagonist.transform.position;
        var move = target - cur;
        move = move.Len((config.maxDist * (1.0f + config.rangePerIndicatorStone * config.rangePerIndicatorStone)).Min(move.magnitude));
        
        // Using step cast method,
        //   because Physics2D.RaycastAll will *not* return multiple contacts for one collider.
        Debug.DrawRay(cur, move * 1e3f, Color.green, 5f);
        
        Vector2 curPos = cur;
        Vector2 restMove = move;
        bool insideGround = false;
        Vector2 prevOffGround = cur;    // Assume curent position is not in the ground.
        for(int i=0; i<20; i++)
        {
            var hit = Physics2D.Raycast(curPos, restMove, restMove.magnitude, LayerMask.GetMask("Terrain"));
            
            var hits = Physics2D.RaycastAll(curPos, restMove, restMove.magnitude, LayerMask.GetMask("Terrain"));
            foreach(var r in hits) Debug.DrawRay(r.point, r.normal, Color.white, 3f);
            
            // Did not hit anything...
            if(hit.collider == null)
            {
                Util.EditorDrawRect(curPos, Vector2.one * 0.1f, Color.red, 3f);
                if(insideGround) res = prevOffGround - move.normalized * config.collisionPreventingDist;
                else res = cur + move;
                break;
            }
            
            // Hit an surface that is parallel to the direction...
            if(hit.normal.Dot(move.normalized).EZ())
            {
                Util.EditorDrawRect(curPos, Vector2.one * 0.1f, Color.yellow, 3f);
                curPos += move.normalized * config.collisionPreventingDist;
                restMove -= move.normalized * config.collisionPreventingDist;
                continue;
            }
            
            // Hit something...
            Util.EditorDrawRect(curPos, Vector2.one * 0.1f, Color.green, 3f);
            curPos = hit.point + move.normalized * config.collisionPreventingDist;
            restMove = cur + move - curPos;
            if(!insideGround) prevOffGround = hit.point - move.normalized * config.collisionPreventingDist;
            insideGround = !insideGround;
            
            Debug.DrawRay(hit.point, hit.normal, Color.cyan, 3f);
        }
        
        return res;
    }
}
