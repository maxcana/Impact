using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossScript : MonoBehaviour
{
    [SerializeField] public float MaxHealthIsSetTo = 100;
    [SerializeField] Gradient dangerGradient;
    public float MaxHealth = 100;
    public float Health { 
        get { return m_Health; } 
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); } }
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
    SpriteRenderer sr;
    PolygonCollider2D pc2d;

    private void Start()
    {
        pc2d = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        willKillOnHit = false;
        if(AutoSetHealth){MaxHealth = bossLevel * 500;} else {MaxHealth = MaxHealthIsSetTo;}
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
        if (Time.time > timeSinceLastDamageTaken + Mathf.Clamp((8f - (bossLevel / 20)), 0, 8) && Health != 0){
            if (UnityEngine.Random.Range(1, (int)Mathf.Clamp(26 - Mathf.Ceil(bossLevel / 10), 1,26)) == 1){
                HealDamage(Mathf.Round(UnityEngine.Random.Range(-10, (0 - 20 - (bossLevel / 3)))), Random.Range(0, 100) < Mathf.Clamp((20 - bossLevel / 10), 0,100)); } }
        
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
        if(Health == 0){
            rb.gravityScale = 1;
            transform.localScale = new Vector3(transform.localScale.x + functions.valueMoveTowards(transform.localScale.x, 0, 0.6f), transform.localScale.y + functions.valueMoveTowards(transform.localScale.y, 0, 0.6f), transform.localScale.z);
            rb.mass += functions.valueMoveTowards(rb.mass, 0, 2f);
        }
        if(transform.localScale.x + transform.localScale.y <= 0.02f){
                Destroy(gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
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

            if (dangerTime == 1) { if(Health > 0){Destroy(other.gameObject);} }
            else
            {
                float Damage;
                float popUpDamage;
                isCritical = Random.Range(0, 100) < 20;

                Damage = Mathf.Ceil(Mathf.Max(0.5f, orb.mass * orb.mass) * (isCritical ? 2.5f : 1) * (Mathf.Abs(orb.velocity.x) + Mathf.Abs(orb.velocity.y)));
                if(other.gameObject.tag == "Player"){
                    Damage *= data.baseDamage / 10;
                }
                popUpDamage = Mathf.Clamp(Damage, 0, Health);

                if(Damage > 2){
                Health -= Damage;
                if(popUpDamage > 0){
                    DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                    DamagePopup.Create(other.GetContact(0).point, popUpDamage, isCritical, other.rigidbody.mass * rb.mass);
                }
                timeSinceLastDamageTaken = Time.time;}
            }
        }
    }

    private void HealDamage(float Damage, bool Critical)
    {
        Damage = -1 * (Mathf.Clamp(Health - Damage, 0f, MaxHealth) - Health);
        if(Damage == 0){return;}
        Health -= Damage;
        DamagePopup.Create(transform.position, Damage, isCritical, 0);
        //W h y
    }
}