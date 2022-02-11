using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossScript : MonoBehaviour
{
    [SerializeField] public float MaxHealthIsSetTo = 100;
    [SerializeField] Gradient dangerGradient;
    public static float MaxHealth = 100;
    public static float Health { 
        get { return m_Health; } 
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); } }
    static float m_Health;
    private bool isCritical;
    private int counter;
    [SerializeField] bool AutoSetHealth;
    private bool willKillOnHit;
    float timeSinceLastDamageTaken;
    [SerializeField] float fadeSpeed;
    [SerializeField] float bossLevel;
    float dangerTime;
    Rigidbody2D rb;
    SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        willKillOnHit = false;
        if(AutoSetHealth){MaxHealth = bossLevel * 500;} else {MaxHealth = MaxHealthIsSetTo;}
        Health = MaxHealth;
        counter = 0;
        timeSinceLastDamageTaken = Time.time;
    }

    private void FixedUpdate()
    {
        counter++;
        if (IsBetween(0, 200, counter))
        {
            rb.angularVelocity += 10;
        }

        if (IsBetween(230, 250, counter)) { rb.angularVelocity /= 1.3f;}

        if (IsBetween(250, 450, counter))
        {
            rb.angularVelocity -= 10;
        }

        //?heals over time
        if (Time.time > timeSinceLastDamageTaken + Mathf.Clamp((8f - (bossLevel / 20)), 0, 8)){
            if (UnityEngine.Random.Range(1, (int)Mathf.Clamp(26 - Mathf.Ceil(bossLevel / 10), 1,26)) == 1)

            {DealDamage(Mathf.Round(UnityEngine.Random.Range(-10, (0 - 20 - (bossLevel / 3)))), Random.Range(0, 100) < Mathf.Clamp((20 - bossLevel / 10), 0,100)); } }
        
        //? if it spins too fast then it will kill on hit
        if (Mathf.Abs(rb.angularVelocity) >= 1350) { willKillOnHit = true;} else {willKillOnHit = false;}

        if (counter == 800)
        {
            counter = 0;
            //Instantiate(GameAssets.i.DeathCircle, transform.position, Quaternion.Slerp(a,b,0.1f));
        }
        
    }

    private void Update() {
        float stepTime = Time.deltaTime * (willKillOnHit? fadeSpeed:-fadeSpeed);
        dangerTime = Mathf.Clamp01(dangerTime + stepTime);
        sr.color = dangerGradient.Evaluate(dangerTime);
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

            if (dangerTime == 1) { Destroy(other.gameObject); }
            else
            {
                float Damage;
                isCritical = Random.Range(0, 100) < 20;

                Damage = Mathf.Ceil(Mathf.Max(0.5f, orb.mass * orb.mass) * (isCritical ? 2.5f : 1) * (Mathf.Abs(orb.velocity.x) + Mathf.Abs(orb.velocity.y)));

                Health -= Damage;
                DamagePopup.Create(other.GetContact(0).point, Damage, isCritical);
                timeSinceLastDamageTaken = Time.time;
            }
        }
    }

    private void DealDamage(float Damage, bool Critical)
    {
        Damage = -1 * (Mathf.Clamp(Health - Damage, 0f, MaxHealth) - Health);
        if(Damage == 0){return;}
        Health -= Damage;
        DamagePopup.Create(transform.position, Damage, isCritical);
    }
}