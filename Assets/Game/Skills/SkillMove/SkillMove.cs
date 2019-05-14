using System;
using UnityEngine;

// Supress "unused private variable" since they an be used by the inspector.
#pragma warning disable 0414


public class SkillMove : MonoBehaviour
{
    public float moveSpeed;
    
    public float jumpSpeed;
    
    // In which contact angle it is considered stably standing on the ground.
    public float stableAngle;
    
    // In whitch contact angle it is considered as "ground".
    public float groundAngle;
    
    public float accPerPhysicsFrame;
    
    // Horizontal velocity that is less than this paramter will be directly set to stopping when stoping. 
    public float maxStopVelocity;
    
    float gravityScale;
    
    // Use default gravity.
    Vector2 gravity => Physics2D.gravity * gravityScale;
    
    // The default gravity direction is down.
    // Unity uses left-hand coordinates.
    // Reverse the angle here.
    Vector2 downDir => Vector2.down.Rot(-this.transform.eulerAngles.z);
    
    CoordSys localCoord => new CoordSys(downDir.RotHalfPi(), -downDir);
    
    /// Used for post-command jumping.
    /// When player pressed JumpKey and the protagonist does not touching the ground,
    /// There's a count down that will make protagonist jump after reaching the ground.
    public float jumpCooldown;
    [SerializeField] float delayedJumpTimer;
    
    public float offGroundJumpTime;
    float offGroundTimer;
    
    public float repeatJumpCooldown;
    float repeatJumpTimer;
    
    [SerializeField] string curStateDisplay;
    [SerializeField] string velocityDispaly;
    
    /// The protagonist is on the fly.
    bool standingStable
    {
        // Criterion: there *exists* a collision with angle between normal and gravity is less than groundAngle.
        get
        {
            foreach(var c in player.recentContacts)
            {
                var pt = c.normal;
                if(Vector2.Angle(pt, -gravity).LE(stableAngle)) return true; 
            }
            return false;
        }
    }
    
    /// The protagonist is standing on a surface, stable or not.
    /// This case should completely cover standingStable case.
    bool standing
    {
        // Criterion: there *exists* a collision with angles between normal and gravity is less than or equal to 90 degrees.
        get
        {
            foreach(var c in player.recentContacts)
            {
                var pt = c.normal;
                if(Vector2.Angle(pt, -gravity).LE(groundAngle)) return true; 
            }
            return false;
        }
    }
    
    bool flying => !standing;
    
    Protagonist player => this.GetComponent<Protagonist>();
    Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
    
    void Start()
    {
        gravityScale = 1.0f;
    }
    
    void Update()
    {
        if(standingStable) curStateDisplay = "stable";
        else if(standing) curStateDisplay = "unstable";
        else curStateDisplay = "flying";
        
        velocityDispaly = rd.velocity.ToString();
        
        
    }
    
    void FixedUpdate()
    {
        // Whatever player's behaviour is, the gravity applies.
        // The Rigidbody2D will handle collision, constraints and so on.
        // (if the speed is not too large...)
        rd.velocity += gravity * Time.fixedDeltaTime;
        
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) ActionStop();
        else if(Input.GetKey(KeyCode.A)) ActionLeft();
        else if(Input.GetKey(KeyCode.D)) ActionRight();
        else ActionStop();
        
        offGroundTimer += Time.fixedDeltaTime;
        if(standingStable) offGroundTimer = 0.0f;
        
        repeatJumpTimer = 0f.Max(repeatJumpTimer - Time.fixedDeltaTime);
        
        delayedJumpTimer = 0f.Max(delayedJumpTimer - Time.fixedDeltaTime);
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) delayedJumpTimer = jumpCooldown;
        ActionJump();
    }
    
    void ActionLeft() => ActionMove(new CoordSys(-localCoord.x, localCoord.y));
    
    void ActionRight() => ActionMove(localCoord);
    
    // coord: default is right: +x axis, up : +y axis.
    //        if you want to use left: +x axis simply reverse it in CoordSys constructor.
    void ActionMove(CoordSys movingCoord)
    {
        Vector2 DirectionalReduceVelocity(Vector2 vs)
        {
            foreach(var c in player.recentContacts) if(c.collider != null)
            {
                if(movingCoord.WorldToLocal(c.normal).Dot(Vector2.right).GEZ()) continue;
                var groundCoord = new CoordSys(c.normal, c.normal.RotHalfPi());
                vs = groundCoord.LocalToWorld(groundCoord.WorldToLocal(vs).Y(0));
            }
            return vs;
        }
        
        var v = movingCoord.WorldToLocal(rd.velocity);
        if(flying)
        {
            v.x = moveSpeed.Min(0f.Max(v.x) + accPerPhysicsFrame);
            DirectionalReduceVelocity(v);
        }
        else if(standingStable)
        {
            v.x = moveSpeed.Min(0f.Max(v.x) + accPerPhysicsFrame);
            v.y = 0f.Max(v.y);
            DirectionalReduceVelocity(v);
        }
        else
        {
            v = Vector2.right * moveSpeed * 0.5f;
            DirectionalReduceVelocity(v);
        }
        
        rd.velocity = movingCoord.LocalToWorld(v);
    }
    
    void ActionStop()
    {
        if(flying)
        {
            var v = localCoord.WorldToLocal(rd.velocity);
            v.x *= accPerPhysicsFrame * Time.fixedDeltaTime;
            if(v.x.Abs() < maxStopVelocity) v.x = 0;
            rd.velocity = localCoord.LocalToWorld(v);
        }
        else if(standingStable)
        {
            /// Notice:
            /// The player will experience some slide-off-fly issue when simply moving on uneven ground.
            /// This may affect the "jump" operation.
            /// The temporary solution is offgroundTimer.
            rd.velocity = rd.velocity.X(0).Y(rd.velocity.y.Max(0f));
        }
        else
        {
            // do nothing...
        }
    }
    
    void ActionJump()
    {
        if(offGroundTimer < offGroundJumpTime
            && delayedJumpTimer != 0.0f
            && repeatJumpTimer == 0.0f
        )
        {
            var localVelocity = localCoord.WorldToLocal(rd.velocity);
            localVelocity.y = jumpSpeed;
            rd.velocity = localCoord.LocalToWorld(localVelocity);
            
            // Preventing jump for another time.
            offGroundTimer = offGroundJumpTime;
            
            // Preventing jump in several frames.
            repeatJumpTimer = repeatJumpCooldown;
        }
    }
}
