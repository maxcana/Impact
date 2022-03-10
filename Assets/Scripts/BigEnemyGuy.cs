using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyGuy : MonoBehaviour
{
    //TODO MAKE BOSS TAKE 1000 DAMAGE AND SHAKE SCREEN AND JUMP BACK UP IF FLIPPED OVER
    [SerializeField] float moveSpeed;
    SpriteRenderer sr;
    public float MaxHealth = 100;
    public float Health { 
        get { return m_Health; } 
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); } }
    float m_Health;
    [SerializeField] GameObject Enemy;
    private bool isCritical;
    float lastHurtTime;
    public float flipWaitTime = 1f;
    [SerializeField] private float rotationTime;
    bool isFlipping;
    bool isPart2;
    Ball player;
    Rigidbody2D rb;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] float wallDetectionRange = 1;
    SpriteRenderer sprite;
    RaycastHit2D[] groundResults = new RaycastHit2D[10];
    bool deadOnHit;

    //private Vector3 eyePosition;
    public Transform eyeTransform;
    void Start()
    {   
        sr = GetComponent<SpriteRenderer>();
        isPart2 = false;
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(rb.gravityScale != 0){
        if(isBallVisible()){Debug.DrawLine(eyeTransform.position, player.transform.position);}
        if(flipCheck()){StartCoroutine(Flip());}
        if(!deadOnHit){sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 155, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 255, 1), sr.color.b + functions.valueMoveTowards(sr.color.b, 255, 1f));}
        else {sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 255, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 255, 1f), sr.color.b + functions.valueMoveTowards(sr.color.b, 255, 1f));}
        } else {
            sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 155, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 0, 1f), sr.color.b + functions.valueMoveTowards(sr.color.b, 0, 1f));
            rb.velocity = new Vector2(0, 3);
            transform.localScale = new Vector2(transform.localScale.x + functions.valueMoveTowards(transform.localScale.x, 3, 1f), transform.localScale.y + functions.valueMoveTowards(transform.localScale.y, 3, 1f));

            if(Mathf.Round(sr.color.r) == 155 && Mathf.Round(sr.color.g) == 0 && Mathf.Round(sr.color.b) == 0){
                if(transform.localScale.x > 2.95 && transform.localScale.y > 2.95){
                    rb.gravityScale = 1;
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
        Health -= Damage;
        lastHurtTime = Time.time;
        DamagePopup.Create(position, Damage, Critical);
        if(Health <= 0 && isPart2){Die(0f);} else if(Health <= 0 && !isPart2){
            MaxHealth = MaxHealth * 10;
            Health = MaxHealth;
            isPart2 = true;
            rb.gravityScale = 0;
        }
    }
}
