using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    //! FIX BUG WHERE WHEN WALKING, DOESN'T TRIGGER TRIGGERS
    [SerializeField] float value = 1;
    [SerializeField] float moveSpeed;
    public float MaxHealth = 100;
    public float Health
    {
        get { return m_Health; }
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); }
    }
    float m_Health;
    private bool isCritical;
    float lastHurtTime;
    public float flipWaitTime = 1f;
    [SerializeField] private float rotationTime;
    bool isFlipping;
    Ball player;
    Rigidbody2D rb;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] float wallDetectionRange = 1;
    SpriteRenderer sprite;
    RaycastHit2D[] groundResults = new RaycastHit2D[10];

    //private Vector3 eyePosition;
    public Transform eyeTransform;
    public Transform damagePopupPosition;
    void Start()
    {
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (isBallVisible()) { Debug.DrawLine(eyeTransform.position, player.transform.position); }
        if (flipCheck()) { StartCoroutine(Flip()); }
    }

    void Die(float TimeToDie)
    {
        functions.SpawnCoins(transform.position, value, UnityEngine.Random.Range(Mathf.RoundToInt(value * 2), Mathf.CeilToInt(value * 6)));
        Destroy(gameObject, TimeToDie);
    }

    bool IsOnGround()
    {
        int colliderCount = groundCollider.Cast(Vector2.down, groundResults, 0.1f);
        return colliderCount > 0;
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //TODO if(Mathf.Round(rb.rotation) == 0 | Mathf.Round(rb.rotation) == 360){rb.rotation = 0;}
        if (Mathf.Abs(rb.rotation) % 360 < 5 && IsOnGround())
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

        float Damage = UnityEngine.Random.Range(-10, -20);
        Damage = Mathf.Clamp(Damage, 0 - (MaxHealth - Health), 0);

        DealDamage(Damage, Random.Range(0, 100) < 20, damagePopupPosition.position, 0);
        //? DEALDAMAGE HEALS THE ENEMY HERE ^
    }

    bool flipCheck()
    {
        return (Time.time > lastHurtTime + flipWaitTime && Mathf.Abs(transform.rotation.eulerAngles.z) > 30 && !isFlipping);
    }

    bool isBallVisible()
    {
        if (player != null)
        {
            RaycastHit2D hitObject = Physics2D.Linecast(eyeTransform.position, player.transform.position);
            return hitObject && hitObject.collider.gameObject == player.gameObject;
        }
        else { return false; }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D orb;
        orb = other.gameObject.GetComponent<Rigidbody2D>();
        if (orb != null)
        {
            float Damage;
            float popUpDamage;
            isCritical = Random.Range(0, 100) < 20;

            Damage = Mathf.Ceil(Mathf.Max(0.5f, orb.mass * orb.mass) * (isCritical ? 2.5f : 1) * (Mathf.Abs(orb.velocity.x) + Mathf.Abs(orb.velocity.y)));
            if (other.gameObject.tag == "Player")
            {
                Damage *= data.baseDamage / 10;
            }
            popUpDamage = Mathf.Clamp(Damage, 0, Health);

            if (Damage > 2)
            {
                DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                DealDamage(Damage, isCritical, popUpDamage, other.GetContact(0).point, other.rigidbody.mass * rb.mass);
            }

        }
    }

    public void DealDamage(float Damage, bool Critical, float popUpDamage, Vector2 position, float totalmass = 0)
    {
        Damage = Mathf.Floor(Damage);
        popUpDamage = Mathf.Floor(popUpDamage);
        if (popUpDamage > 0)
        {
            DamagePopup.Create(position, popUpDamage, isCritical, totalmass);
        }
        Health -= Damage;
        lastHurtTime = Time.time;
        if (Health <= 0) { Die(0f); }
    }
    public void DealDamage(float Damage, bool Critical, Vector2 position, float totalmass = 0)
    {
        DamagePopup.Create(position, Damage, isCritical, totalmass);
        Health -= Damage;
        lastHurtTime = Time.time;
        if (Health <= 0) { Die(0f); }
    }
    public void DealDamages(float Damage, bool Critical, float totalmass = 0)
    {
        DamagePopup.Create(damagePopupPosition.position, Damage, isCritical, totalmass);
        Health -= Damage;
        lastHurtTime = Time.time;
        if (Health <= 0) { Die(0f); }
    }
    public void DealDamageWithAutoPopupDamageAtDefaultPosition(float Damage)
    {
        float popUpDamage;
        isCritical = Random.Range(0, 100) < 20;
        popUpDamage = Mathf.Clamp(Damage, 0, Health);

        DealDamage(Damage, isCritical, popUpDamage, damagePopupPosition.position);
    }
}