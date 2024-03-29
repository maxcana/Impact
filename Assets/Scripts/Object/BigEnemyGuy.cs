using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigEnemyGuy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    SpriteRenderer sr;
    public float MaxHealth = 100;
    public float Health
    {
        get { return m_Health; }
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); }
    }
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
    float timeSinceLastGroundDamage;
    Rigidbody2D rb;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] float wallDetectionRange = 1;
    SpriteRenderer sprite;
    RaycastHit2D[] groundResults = new RaycastHit2D[10];
    bool deadOnHit = false;
    bool didTurnOndamage;
    bool isDamagable;
    CapsuleCollider2D cc2d;
    bool dead;


    //private Vector3 eyePosition;
    public Transform eyeTransform;
    float TimeSincePart2;
    void Start()
    {
        dead = false;
        timeSinceLastGroundDamage = 0;
        didTurnOndamage = false;
        isDamagable = false;
        isDonePart2Intro = false;
        sr = GetComponent<SpriteRenderer>();
        cc2d = GetComponent<CapsuleCollider2D>();
        isPart2 = false;
        TimeSincePart2 = 0;
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {if(dead) return;
        timeSinceLastGroundDamage += Time.deltaTime;
        if (isPart2) { TimeSincePart2 += Time.deltaTime; }
        if (!isPart2)
        {
            if (Time.time > 2 && !didTurnOndamage)
            {
                isDamagable = true;
                didTurnOndamage = true;
            }
        }
        else
        {
            if (TimeSincePart2 > 2 && !didTurnOndamage)
            {
                isDamagable = true;
                didTurnOndamage = true;
            }
        }

        if (shoulIIncrementHowLongIHaveSpentRotatingTo0)
        {
            howLongIHaveSpentRotatingTo0 += Time.deltaTime;
        }

        if (rb.gravityScale != 0)
        {
            if (isBallVisible()) { Debug.DrawLine(eyeTransform.position, player.transform.position); }
            if (flipCheck()) { StartCoroutine(Flip()); }
            if (isPart2)
            {
                if (deadOnHit) { sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 1, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 0, 1), sr.color.b + functions.valueMoveTowards(sr.color.b, 0, 1f)); }
                else { sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 1f, 1f), sr.color.g + functions.valueMoveTowards(sr.color.g, 0.4f, 1f), sr.color.b + functions.valueMoveTowards(sr.color.b, 0.4f, 1f)); }
            }

        }
        else
        {

            //? red animation
            sr.color = new Color(sr.color.r + functions.valueMoveTowards(sr.color.r, 1, 0.7f), sr.color.g + functions.valueMoveTowards(sr.color.g, 0.4f, 0.7f), sr.color.b + functions.valueMoveTowards(sr.color.b, 0.4f, 0.7f));

            //? health change
            MaxHealth += functions.valueMoveTowards(MaxHealth, newMaxHealth, 4f);
            Health += functions.valueMoveTowards(Health, MaxHealth, 1.6f);
            if (10000 > MaxHealth) { MaxHealth++; }
            if (10000 > Health) { Health++; } else { Health = 10000; }
            Health = Mathf.Ceil(Health);

            //? size animation
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, 15 * Time.deltaTime), 6);
            transform.localScale = new Vector2(transform.localScale.x + functions.valueMoveTowards(transform.localScale.x, 3, 0.7f), transform.localScale.y + functions.valueMoveTowards(transform.localScale.y, 3, 0.7f));

            //? rotate (nobody needs to read this junk)
            float startAngle = rb.rotation;
            float angle = Mathf.LerpAngle(startAngle, 0, howLongIHaveSpentRotatingTo0 / to0RotationTimePart2);
            float xvel = Mathf.LerpAngle(startAngle, 0, howLongIHaveSpentRotatingTo0 / to0RotationTimePart2);
            rb.MoveRotation(angle);
            if (howLongIHaveSpentRotatingTo0 >= to0RotationTimePart2 / 3)
            {
                shoulIIncrementHowLongIHaveSpentRotatingTo0 = false;
                rb.MoveRotation(angle);
                rb.angularVelocity = 0;
                rb.MoveRotation(0);
                rb.mass = 10;
                howLongIHaveSpentRotatingTo0 = 0;
            }


            if (Health == 10000)
            {
                if (transform.localScale.x > 2.3f && transform.localScale.y > 2.3f)
                {
                    sr.color = new Color(1, 0.4f, 0.4f);
                    rb.gravityScale = 1;
                    rb.velocity = new Vector2(rb.velocity.x, -3);
                    didTurnOndamage = false;
                    TimeSincePart2 = 0;
                    isDonePart2Intro = true;
                    MaxHealth = newMaxHealth;
                    Health = MaxHealth;
                }
            }
        }


    }
    public IEnumerator Die(float delay)
    {
        dead = true;
        GetComponent<Shards>().Disperse(transform.position);
        yield return new WaitForSeconds(0.3f);
        functions.SpawnCoins(transform.position, 4, UnityEngine.Random.Range(18, 22), 8);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }

    bool IsOnGround()
    {
        int colliderCount = groundCollider.Cast(Vector2.down, groundResults, 0.1f);
        return colliderCount > 0;
    }
    void FixedUpdate()
    {
        if(dead) return;
        if (Mathf.Abs(rb.rotation) % 360 < 5 && IsOnGround() && rb.velocity.magnitude < 5)
        {
            rb.rotation = 0;
            float moveStep = moveSpeed * Time.fixedDeltaTime;
            Vector2 endPosition = rb.position + Vector2.right * moveStep;
            Vector2 rendPosition = endPosition;
            rendPosition.x += wallDetectionRange * Mathf.Sign(moveSpeed);
            RaycastHit2D hit = Physics2D.Linecast(rb.position, rendPosition);
            if (hit) { moveSpeed *= -1; } else { rb.MovePosition(endPosition); }
            Debug.DrawLine(rb.position, rendPosition, Color.red, Time.fixedDeltaTime);
        }
    }

    IEnumerator Flip()
    {
        isFlipping = true;
        rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
        float startAngle = rb.rotation;
        for (float e = 0; e < rotationTime; e += Time.deltaTime)
        {
            float angle = Mathf.LerpAngle(startAngle, 0, e / rotationTime);
            float xvel = Mathf.LerpAngle(startAngle, 0, e / rotationTime);
            rb.MoveRotation(angle);
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, 15 * Time.deltaTime), rb.velocity.y);
            yield return null;
        }
        rb.angularVelocity = 0;
        rb.MoveRotation(0);
        isFlipping = false;
        lastHurtTime = Time.time;

        float Damage = Mathf.Round(UnityEngine.Random.Range(-50, (MaxHealth - Health) / 40));
        float popUpDamage = Mathf.Clamp(Damage, 0 - (MaxHealth - Health), 0);

        DealDamage(Damage, false, false, transform.position, popUpDamage);
        //? DEALDAMAGE HEALS THE ENEMY HERE ^
    }

    bool flipCheck()
    {
        return (Time.time > lastHurtTime + flipWaitTime && Mathf.Abs(transform.rotation.eulerAngles.z) > 30 && !isFlipping);
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
        if (orb != null)
        {
            float Damage;
            float popUpDamage;

            bool isHyperCritical = Random.Range(0, 100) < 5;
            isCritical = Random.Range(0, 100) < 20;
            Damage = Mathf.Ceil(Mathf.Max(0.5f, orb.mass * orb.mass) * (isHyperCritical ? 6f : isCritical ? 2.5f : 1) * (Mathf.Abs(orb.velocity.x) + Mathf.Abs(orb.velocity.y)));
            if (other.gameObject.tag == "Player")
            {
                Damage *= data.baseDamage / 10;
            }
            popUpDamage = Mathf.Clamp(Damage, 0, Health);

            if (Damage > 2)
            {
                DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                DealDamage(Damage, isCritical, isHyperCritical, other.GetContact(0).point, popUpDamage, other.rigidbody.mass * rb.mass);
            }
        }
        else
        {
            if (other.gameObject.tag == "GroundsDamage")
            {
                float Damage;
                if (rb.velocity.y < 0)
                {
                    if (Mathf.Abs(transform.rotation.eulerAngles.z) > 70)
                    {
                        Damage = Mathf.Round(Mathf.Abs(rb.velocity.y) * 600);
                    }
                    else { Damage = Mathf.Round(Mathf.Abs(rb.velocity.y) * 125); }
                }
                else { Damage = 0; }

                if (Damage >= 1000 && isPart2 && timeSinceLastGroundDamage > 2)
                {
                    timeSinceLastGroundDamage = 0;
                    Damage = Mathf.Round(Damage / 500) * 500;
                    float popUpDamage = Mathf.Clamp(Damage, 0, Health);
                    DealDamage(Damage, false, false, other.GetContact(0).point, popUpDamage, 25 * rb.mass);
                }
            }
        }
    }

    public void DealDamage(float Damage, bool Critical, bool isHyperCritical, Vector2 position, float popUpDamage, float totalmass = 0)
    {
        Damage = Mathf.Floor(Damage);
        popUpDamage = Mathf.Floor(popUpDamage);
        if (isDamagable)
        {
            Health -= Damage;
            lastHurtTime = Time.time;
            DamagePopup.Create(position, popUpDamage, Critical, totalmass, isHyperCritical);

            if (Health <= 0 && isPart2) { StartCoroutine(Die(4f)); }
            else if (Health <= 0 && !isPart2)
            {
                newMaxHealth = MaxHealth * 10;
                Health = 1;
                isPart2 = true;
                isDamagable = false;
                howLongIHaveSpentRotatingTo0 = 0;
                rb.gravityScale = 0;
            }
        }
    }
    public void DealDamageWithAutoPopupDamageAtDefaultPosition(float Damage)
    {
        float popUpDamage;
        isCritical = Random.Range(0, 100) < 20;
        popUpDamage = Mathf.Clamp(Damage, 0, Health);

        DealDamage(Damage, isCritical, false, transform.position, popUpDamage);
    }
}