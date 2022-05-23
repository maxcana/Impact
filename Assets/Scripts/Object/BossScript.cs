using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossScript : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] public float MaxHealthIsSetTo = 100;
    [SerializeField] Gradient dangerGradient;
    public float MaxHealth = 100;
    public float Health
    {
        get { return m_Health; }
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); }
    }
    float m_Health;
    private bool isCritical;
    private int counter;
    [SerializeField] bool AutoSetHealth;
    private bool willKillOnHit;
    float timeSinceLastDamageTaken;
    [SerializeField] float fadeSpeed;
    [SerializeField] float bossLevel;
    float dangerTime;
    Rigidbody2D rb;
    bool dead;
    SpriteRenderer sr;
    PolygonCollider2D pc2d;

    private void Start()
    {
        dead = false;
        pc2d = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        willKillOnHit = false;
        if (AutoSetHealth) { MaxHealth = bossLevel * 500; } else { MaxHealth = MaxHealthIsSetTo; }
        Health = MaxHealth;
        counter = 0;
        timeSinceLastDamageTaken = Time.time;
        rb.gravityScale = 0;
        transform.localScale = new Vector3(6.5579f, 6.5579f, 6.5579f);
        pc2d.enabled = true;
        rb.mass = 200;
    }
    private void FixedUpdate()
    {
        if(dead)
            return;
        //?heals over time
        if (Time.time > timeSinceLastDamageTaken + Mathf.Clamp((8f - (bossLevel / 20)), 0, 8) && Health != 0)
        {
            if (UnityEngine.Random.Range(1, (int)Mathf.Clamp(26 - Mathf.Ceil(bossLevel / 10), 1, 26)) == 1)
            {
                HealDamage(Mathf.Round(UnityEngine.Random.Range(-10, (0 - 20 - (bossLevel / 3)))), Random.Range(0, 100) < Mathf.Clamp((20 - bossLevel / 10), 0, 100));
            }
        }

        //? if it spins too fast or it is attacking then it will kill on hit 
        if (Mathf.Abs(rb.angularVelocity) >= 1350 || IsBetween(450, 1100, counter)) { willKillOnHit = true; } else { willKillOnHit = false; }



        counter++;
        if (IsBetween(0, 200, counter))
        {
            rb.angularVelocity += 10;
        }

        if (IsBetween(230, 250, counter)) { rb.angularVelocity /= 1.3f; }

        if (IsBetween(250, 450, counter))
        {
            rb.angularVelocity -= 10;
        }

        if (counter == 500 || counter == 550 || counter == 600 || counter == 650)
        {
            StartCoroutine(playShootPattern(transform.localEulerAngles.z, 6, 360 / 6, 0));
        }

        if (counter == 700)
        {
            StartCoroutine(playShootPattern(transform.localEulerAngles.z, 144, 360 / 6, 0.05f));
            StartCoroutine(playShootPattern(transform.localEulerAngles.z + 360 / 12, 72, 360 / 6, 0.1f));
        }

        if (counter == 1200)
        {
            counter = 0;
        }

    }
    public IEnumerator playShootPattern(float startingDegree, int amount, float degree, float delay)
    {
        var wait = new WaitForSeconds(delay);
        for (int i = 0; i < amount; i++)
        {
            Projectile p = Instantiate(projectile);
            p.transform.position = transform.position;
            p.direction = functions.degreeToVector2(degree * i + startingDegree);
            yield return wait;
        }
    }
    private void Update()
    {
        float stepTime = Time.deltaTime * (willKillOnHit ? fadeSpeed : -fadeSpeed);
        dangerTime = Mathf.Clamp01(dangerTime + stepTime);
        sr.color = dangerGradient.Evaluate(dangerTime);
        if (Health == 0 && !dead)
        {
            dead = true;
            StartCoroutine(Die(9f));
        }
    }
    public IEnumerator Die(float delay)
    {
        GetComponent<Shards>().Disperse(transform.position);
        yield return new WaitForSeconds(0.1f);
        functions.SpawnCoins(transform.position, 4, UnityEngine.Random.Range(18, 22), 170);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }
    private bool IsBetween(int a, int b, int number)
    {
        return number >= a & number < b;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D orb;
        orb = other.gameObject.GetComponent<Rigidbody2D>();
        if (orb != null)
        {

            if (dangerTime == 1) { if (Health > 0) { Destroy(other.gameObject); } }
            else
            {
                float Damage;
                float popUpDamage;
                isCritical = Random.Range(0, 100) < 20;
                bool isHyperCritical = Random.Range(0, 100) < 5;

                Damage = Mathf.Ceil(Mathf.Max(0.5f, orb.mass * orb.mass) * (isHyperCritical ? 6f : isCritical ? 2.5f : 1) * (Mathf.Abs(orb.velocity.x) + Mathf.Abs(orb.velocity.y)));
                if (other.gameObject.tag == "Player")
                {
                    Damage *= data.baseDamage / 10;
                }

                popUpDamage = Mathf.Clamp(Damage, 0, Health);
                Damage = Mathf.Floor(Damage);
                popUpDamage = Mathf.Floor(popUpDamage);

                if (Damage > 2)
                {
                    Health -= Damage;
                    if (popUpDamage > 0)
                    {
                        DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                        DamagePopup.Create(other.GetContact(0).point, popUpDamage, isCritical, other.rigidbody.mass * rb.mass, isHyperCritical);
                    }
                    timeSinceLastDamageTaken = Time.time;
                }
            }
        }
    }

    private void HealDamage(float Damage, bool Critical)
    {
        Damage = Mathf.Floor(Damage);
        Damage = -1 * (Mathf.Clamp(Health - Damage, 0f, MaxHealth) - Health);
        if (Damage == 0) { return; }
        Health -= Damage;
        DamagePopup.Create(transform.position, Damage, false, 0, false);
        //W h y
    }
    public void DealDamageWithAutoPopupDamageAtDefaultPosition(float Damage)
    {
        float popUpDamage;
        isCritical = Random.Range(0, 100) < 20;
        popUpDamage = Mathf.Clamp(Damage, 0, Health);

        Health -= Damage;
        if (popUpDamage > 0)
        {
            DamagePopup.Create(transform.position, popUpDamage, isCritical, 0, false);
        }
        timeSinceLastDamageTaken = Time.time;
    }
}