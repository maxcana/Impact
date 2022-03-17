using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyGuy : MonoBehaviour
{
    //! FIX BUG WHERE WHEN WALKING, DOESN'T TRIGGER TRIGGERS, THIS ALLOWS BOSS TO WALK THROUGH BOTH ONE WAY DOORS ON THE SIDES
    //! FIX BUG WHERE HE DOESNT CHANGE COLOUR WHEN HE GOES INTO PHASE 2
    //! HE ALSO RISES UP FOR WAY TOO LONG
    //TODO MAKE BOSS TAKE DAMAGE BASED ON VELOCITY AND SHAKE SCREEN AND JUMP BACK UP IF FLIPPED OVER
    [SerializeField] float moveSpeed;
    SpriteRenderer sr;
    public float MaxHealth = 100;
    public float Health { 
        get { return m_Health; } 
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); } }
    float m_Health;
    float newMaxHealth;
    [SerializeField] GameObject Enemy;
    private bool isCritical;
    float lastHurtTime;
    public float flipWaitTime = 1f;
    public static bool isDonePart2Intro;
    [SerializeField] private float rotationTime;
    [SerializeField] float to0RotationTimePart2;
    float howLongIHaveSpentRotatingTo0;
    bool shoulIIncrementHowLongIHaveSpentRotatingTo0 = true;
    bool isFlipping;
    bool isPart2;
    Ball player;
    Rigidbody2D rb;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] float wallDetectionRange = 1;
    SpriteRenderer sprite;
    RaycastHit2D[] groundResults = new RaycastHit2D[10];
    bool deadOnHit = false;
    bool isDamagable;

    //private Vector3 eyePosition;
    public Transform eyeTransform;
    void Start()
    {   
        isDonePart2Intro = false;
        sr = GetComponent<SpriteRenderer>();
        isPart2 = false;
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>();
        sprite = GetComponent<SpriteRenderer>();
        isDamagable = true;
    }
    void Update()
    {
        if(shoulIIncrementHowLongIHaveSpentRotatingTo0){
        howLongIHaveSpentRotatingTo0 += Time.deltaTime;
        }

        if(rb.gravityScale != 0){
        if(isBallVisible()){Debug.DrawLine(eyeTransform.position, player.transform.position);}
        if(flipCheck()){StartCoroutine(Flip());}
        if(isPart2){
            if(deadOnHit){sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 255, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 0, 1), sr.color.b + functions.valueMoveTowards(sr.color.b, 0, 1f));}
            else {sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 255, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 114, 1f), sr.color.b + functions.valueMoveTowards(sr.color.b, 114, 1f));}
        }
        
        } else {
            
            //? red animation
            sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 255, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 114, 1f), sr.color.b + functions.valueMoveTowards(sr.color.b, 114, 1f));
            
            //? health change
            MaxHealth += functions.valueMoveTowards(MaxHealth, newMaxHealth, 2f);
            Health += functions.valueMoveTowards(Health, MaxHealth, 0.8f);
            if(10000 > MaxHealth){MaxHealth++;}
            if(10000 > Health){Health++;} else {Health = 10000;}
            Health = Mathf.Ceil(Health);

            //? size animation
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, 15 * Time.deltaTime), 2);
            transform.localScale = new Vector2(transform.localScale.x + functions.valueMoveTowards(transform.localScale.x, 3, 0.2f), transform.localScale.y + functions.valueMoveTowards(transform.localScale.y, 3, 0.2f));

            //? rotate (nobody needs to read this junk)
            float startAngle = rb.rotation;
            float angle = Mathf.LerpAngle(startAngle, 0, howLongIHaveSpentRotatingTo0/to0RotationTimePart2);
            float xvel = Mathf.LerpAngle(startAngle, 0, howLongIHaveSpentRotatingTo0/to0RotationTimePart2);
            rb.MoveRotation(angle);
            if(howLongIHaveSpentRotatingTo0 >= to0RotationTimePart2 / 3){
                shoulIIncrementHowLongIHaveSpentRotatingTo0 = false;
                rb.MoveRotation(angle);
                rb.angularVelocity = 0;
                rb.MoveRotation(0);
                rb.mass = 10;
                howLongIHaveSpentRotatingTo0 = 0;
            }
            

            if(Health == 10000){
                if(transform.localScale.x > 2.95 && transform.localScale.y > 2.95){
                    sr.color = new Color(255, 114, 114);
                    rb.gravityScale = 1;
                    rb.velocity = new Vector2(rb.velocity.x, -3);
                    isDamagable = true;
                    isDonePart2Intro = true;
                    MaxHealth = newMaxHealth;
                    Health = MaxHealth;
                }
            }
        }


    }

    void Die(float TimeToDie){Destroy(gameObject, TimeToDie);}

    bool IsOnGround(){
        int colliderCount = groundCollider.Cast(Vector2.down, groundResults, 0.1f);
        return colliderCount > 0;
    }
    void FixedUpdate() {
        if(Mathf.Abs(rb.rotation) % 360 < 5 && IsOnGround()){
            rb.rotation = 0;
            float moveStep = moveSpeed * Time.fixedDeltaTime;
            Vector2 endPosition = rb.position + Vector2.right * moveStep;
            Vector2 rendPosition = endPosition;
            rendPosition.x += wallDetectionRange *Mathf.Sign(moveSpeed);
            RaycastHit2D hit = Physics2D.Linecast(rb.position, rendPosition);
            if(hit){moveSpeed *= -1;} else {rb.MovePosition(endPosition);}
            Debug.DrawLine(rb.position, rendPosition, Color.red, Time.fixedDeltaTime);
        }
    }

    IEnumerator Flip(){
        isFlipping = true;
        rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
        float startAngle = rb.rotation;
        for(float e = 0; e < rotationTime; e+= Time.deltaTime){
            float angle = Mathf.LerpAngle(startAngle, 0, e/rotationTime);
            float xvel = Mathf.LerpAngle(startAngle, 0, e/rotationTime);
            rb.MoveRotation(angle);
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, 15 * Time.deltaTime), rb.velocity.y);
            yield return null;
        }
        rb.angularVelocity = 0;
        rb.MoveRotation(0);
        isFlipping = false;
        lastHurtTime = Time.time;

        DealDamage(UnityEngine.Random.Range(-10,-20), Random.Range(0, 100) < 20, transform.position);
        //? DEALDAMAGE HEALS THE ENEMY HERE ^
    }

    bool flipCheck()
    {
        return(Time.time > lastHurtTime + flipWaitTime && Mathf.Abs(transform.rotation.eulerAngles.z) > 30 && !isFlipping);
    }

    bool isBallVisible()
    {
        RaycastHit2D hitObject = Physics2D.Linecast(eyeTransform.position, player.transform.position);
        return hitObject && hitObject.collider.gameObject == player.gameObject;
    }

     private void OnCollisionEnter2D(Collision2D other) 
    {
        Rigidbody2D orb;
        orb = other.gameObject.GetComponent<Rigidbody2D>();
        if(orb != null)
        {

            float Damage;
            isCritical = Random.Range(0, 100) < 20;

        
                Damage = Mathf.Ceil(Mathf.Max(0.5f, orb.mass * orb.mass) * (isCritical?2.5f:1) * (Mathf.Abs(orb.velocity.x) + Mathf.Abs(orb.velocity.y)));

            DealDamage(Damage, isCritical, other.GetContact(0).point);
        }
    }

    public void DealDamage(float Damage, bool Critical, Vector2 position)
    {
        if(isDamagable){
        Health -= Damage;
        lastHurtTime = Time.time;
        DamagePopup.Create(position, Damage, Critical);

        if(Health <= 0 && isPart2){Die(0f);} else if(Health <= 0 && !isPart2){
            newMaxHealth = MaxHealth * 10;
            Health = 1;
            isPart2 = true;
            isDamagable = false;
            howLongIHaveSpentRotatingTo0 = 0;
            rb.gravityScale = 0;
        }}
    }
}