using System.Collections;
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
                transfered = true;
                
                bool swapped = Transfer();
                float magicCost = 0f.Max(1.0f - config.efficiencyPerNatureStone * spec.Count(StoneType.Nature));
                if(swapped) protagonist.inventory.curWand.curSlot.ConsumeMagic(config.magicConsumePerSwap * magicCost);
                else protagonist.inventory.curWand.curSlot.ConsumeMagic(config.magicConsumePerUse * magicCost);
                 
                // Move the protagonist.
                this.transform.position = destination;
                
                // Clear protagonist's trails.
                foreach(var trail in protagonist.trails) trail.Clear();
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
        var target = Util.cursorWorldPosition;
        var cur = (Vector2)protagonist.transform.position;
        var move = target - cur;
        move = move.Len((config.maxDist * (1.0f + config.rangePerIndicatorStone * config.rangePerIndicatorStone)).Min(move.magnitude));
        
        // The move target is the place where protagonist is.
        // No need to compute more information.
        if(move.magnitude.LEZ()) return cur;
        
        // Test all possible colliders.
        var hitCols = Physics2D.RaycastAll(cur, move, move.magnitude, LayerMask.GetMask("Terrain"));
        Debug.DrawRay(cur, move, Color.cyan, 3f);
        
        // Get all actual collision points' distances in and out.
        // inout == 1 : go into the polygon.
        // inout == -1 : go out of the polygon.
        // inout == other numbers : undefined.
        var hits = new List<(float dist, Inout inout)>();
        
        // For logical consistance, add the beginning point.
        hits.Add((0, Inout.Out));
        
        foreach(var col in hitCols)
        {
            var c = col.collider;
            switch(c)
            {
                case PolygonCollider2D s:
                {
                    int cnt = 0;
                    for(int u=0; u<s.pathCount; u++)
                    {
                        var path = s.GetPath(u);
                        for(int i=0; i<path.Length; i++)
                        {
                            var polySeg = new Segment(
                                (Vector2)c.transform.position + path[i],
                                (Vector2)c.transform.position + path[(i + 1).ModSys(path.Length)]
                            );
                            var testSeg = new Segment(cur, cur + move);
                            
                            Debug.DrawLine(polySeg.from, polySeg.to, Color.blue, 3f);
                            
                            // Segments are parallel. Always ingore them in this case, even if they intersect.
                            if(!polySeg.asLine.Intersects(testSeg.asLine)) continue;
                            
                            var itsc = polySeg.asLine.Intersection(testSeg.asLine);
                            
                            // Get line intersection but not ray intersection.
                            if(cur.To(itsc).Dot(move.normalized).LEZ()) continue;
                            
                            // The intersection is too far to the original.
                            if(cur.To(itsc).magnitude.G(move.magnitude)) continue;
                            
                            // The test line covers one of the polygon's vertex. Ignore it.
                            if(!polySeg.Cover(itsc, true)) continue;
                            
                            // Note we don't have to check the intersection of testSeg. it is infinity long.
                            // if(!testSeg.Cover(itsc, true)) continue;
                            
                            cnt += 1;
                            hits.Add((cur.To(itsc).magnitude, 0));
                            Util.EditorDrawRect(itsc, Vector2.one * 0.3f, Color.green, 3.0f);
                        }
                        
                        // In this range, all are intersection to one path.
                        // A polygon path is closed, and the intersections are always in
                        //   "in - out - in - out ..." pattern when sorted by distance,
                        //   as long as the original point (protagonist's location) is off the polygon. 
                        hits.Sort(hits.Count - cnt, cnt, Comparer<(float, Inout)>.Default);
                        
                        Inout curInout = Inout.Out;
                        for(int i = hits.Count - cnt; i < hits.Count; i++)
                        {
                            hits[i] = (hits[i].dist, curInout = (Inout)(-(int)curInout));
                        }
                    }
                }
                break;
                default: throw new System.Exception("Not-supported collider.");
            }
        }
        
        // Get all in-out information.
        hits.Sort(Comparer<(float, Inout)>.Default); // simply sort asending.
        
        // There's no hits. Free to move directly...
        if(hits.Count <= 1) return cur + move;
        
        Vector2 res = Vector2.zero;
        int inCount = 0;
        for(int i=1; i<hits.Count; i++)
        {
            var pd = hits[i-1].dist;
            var cd = hits[i].dist;
            if(inCount == 0) // We're off any polygon now.
            {
                Segment cs = new Segment(cur + move.normalized * pd, cur + move.normalized * cd);
                Debug.DrawLine(cs.from, cs.to, Color.green, 3f);
                
                // The target must be in the valid segment first...
                // Cancel the check since move.magnitude < pd won't be true anymore...
                if(move.magnitude.L(pd)) continue;
                
                // And the target location can be... here.
                var safeDist = move.magnitude.Clamp(pd + config.collisionPreventingDist, cd - config.collisionPreventingDist);
                res = cur + safeDist * move.normalized;
            }
            else // We're in some polygon.
            {
                Segment cs = new Segment(cur + move.normalized * pd, cur + move.normalized * cd);
                Debug.DrawLine(cs.from, cs.to, Color.red, 3f);
            }
            
            switch(hits[i].inout)
            {
                case Inout.In : inCount++; break;
                case Inout.Out : inCount--; break;
                default: throw new System.Exception("Invalid inout parameter.");
            }
        }
        
        // A special case when specified destination is off all polygons.
        if(inCount == 0)
        {
            var finalDist = hits[hits.Count - 1].dist;
            var resDist = move.magnitude.Max(finalDist + config.collisionPreventingDist);
            return cur + move.Len(resDist);
        }
        
        return res;
    }
    
    // ============================================================================================
    // Util
    // ============================================================================================
    enum Inout : int
    {
        Out = -1,
        Unknown = 0,
        In = 1,
    }
}
