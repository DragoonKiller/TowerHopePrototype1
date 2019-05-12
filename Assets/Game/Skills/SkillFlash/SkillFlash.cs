using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFlash : Skill
{
    public SkillFlashConfig config;
    
    public Vector2 destination;
    
    float physTimer;
    float displayTimer;
    bool transfered;
    
    [SerializeField] GameObject destinationAbsorb;
    SpriteRenderer absorbRenderer => destinationAbsorb.GetComponent<SpriteRenderer>();
    
    void Start()
    {
        destination = FindDestination();
        Util.DebugDrawCircle(destination, config.swapDist, 32, Color.red, 3f);
        
        physTimer = 0f;
        transfered = false;
        this.transform.localScale = Vector3.one;
        
        destinationAbsorb = Instantiate(config.destinationAbsorb);
        destinationAbsorb.transform.position = destination;
        destinationAbsorb.transform.localScale = Vector2.one * config.renderScales;
        
        var mat = destinationAbsorb.GetComponent<SpriteRenderer>().material;
        mat.SetFloat("radius", config.swapDist / config.renderScales * 2.0f);
        mat.SetFloat("width", config.swapDist / config.renderScales / 10.0f);
        absorbRenderer.color = absorbRenderer.color.A(0f);
    }
    
    void Update()
    {
        displayTimer += Time.deltaTime;
        if(displayTimer < config.hideTime)
        {
            var rate = 1.0f - physTimer / config.hideTime;
            this.gameObject.transform.localScale = Vector2.one * rate;
            
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
                this.gameObject.transform.localScale = Vector2.one * (curTime / config.appearTime);
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
                if(swapped) player.ConsumeMagic(config.magicConsumePerSwap);
                else player.ConsumeMagic(config.magicConsumePerUse);
                 
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
        this.transform.localScale = Vector2.one;
        if(transfered) rd.velocity = Vector2.zero;
        DestroyImmediate(destinationAbsorb);
    }
    
    /// Swap monster's bullets.
    /// Notice this function *must* performed *before* setting transform.position to destination.
    /// Notice this swap will not trigger collision between protagonist and bullets,
    ///   since this is performed without "time proceeded".
    bool Transfer()
    {
        var p = Physics2D.CircleCastAll(destination, config.swapDist, Vector2.zero, 0, LayerMask.GetMask("MonsterBullet"));
        bool swapped = false;
        foreach(var h in p)
        {
            var bullet = h.collider.gameObject.GetComponent<MonsterBullet>();
            if(bullet == null) continue;
            bullet.GetComponent<Rigidbody2D>().position += (Vector2)player.transform.position - destination;
            swapped = true;
        }
        return swapped;
    }
    
    Vector2 FindDestination()
    {
        Vector2 res = Vector2.zero;
        var target = Util.cursorWorldPosition;
        var cur = (Vector2)player.transform.position;
        var move = target - cur;
        move = move.Len(config.maxDist.Min(move.magnitude));
        
        // Using step cast method,
        //   because Physics2D.RaycastAll will *not* return multiple contacts for one collider.
        Debug.DrawRay(cur, move, Color.green, 5f);
        
        Vector2 curPos = cur;
        Vector2 restMove = move;
        bool insideGround = false;
        Vector2 prevOffGround = cur;    // Assume curent position is not in the ground.
        while(true)
        {
            var hit = Physics2D.Raycast(curPos, restMove, restMove.magnitude, LayerMask.GetMask("Terrain"));
            
            if(hit.collider == null)
            {
                Util.DebugDrawRect(curPos, Vector2.one * 0.1f, Color.red, 3f);
                if(insideGround) res = prevOffGround - move.normalized * config.collisionPreventingDist;
                else res = cur + move;
                break;
            }
            
            if(hit.normal.Dot(move.normalized).EZ())
            {
                Util.DebugDrawRect(curPos, Vector2.one * 0.1f, Color.yellow, 3f);
                curPos += move.normalized * config.collisionPreventingDist;
                restMove -= move.normalized * config.collisionPreventingDist;
                continue;
            }
            
            Util.DebugDrawRect(curPos, Vector2.one * 0.1f, Color.green, 3f);
            curPos = hit.point + move.normalized * config.collisionPreventingDist;
            restMove = cur + move - curPos;
            if(!insideGround) prevOffGround = hit.point - move.normalized * config.collisionPreventingDist;
            insideGround = !insideGround;
        }
        
        return res;
    }
}
