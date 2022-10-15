using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oct : MonoBehaviour
{
    public enum State { damageable, spinningbar, lavafall, projectile, tracking }
    State state;
    bool isDamagable = true;
    Rigidbody2D rb;
    SpriteRenderer sr;
    public float MaxHealth = 100;
    float counter;
    public float Health
    {
        get { return m_Health; }
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); }
    }
    float m_Health;
    void Start()
    {
        counter = 0;
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        counter += Time.deltaTime;
        if(functions.IsBetweenf(0, 20, counter)){
            
        }
    }
     public IEnumerator Die(float delay)
    {
        GetComponent<Shards>().Disperse(transform.position, 8);
        yield return new WaitForSeconds(0.1f);
        functions.SpawnCoins(transform.position, 4, UnityEngine.Random.Range(18, 22), 170);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
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
            bool isCritical = Random.Range(0, 100) < 20;
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
    }

    public void DealDamage(float Damage, bool isCritical, bool isHyperCritical, Vector2 position, float popUpDamage, float totalmass = 0)
    {
        Damage = Mathf.Floor(Damage);
        popUpDamage = Mathf.Floor(popUpDamage);
        if (isDamagable)
        {
            Health -= Damage;
            DamagePopup.Create(position, popUpDamage, isCritical, totalmass, isHyperCritical);

            if (Health <= 0)
            {

            }
        }
    }
    public void DealDamageWithAutoPopupDamageAtDefaultPosition(float Damage)
    {
        float popUpDamage;
        bool isCritical = Random.Range(0, 100) < 20;
        popUpDamage = Mathf.Clamp(Damage, 0, Health);

        DealDamage(Damage, isCritical, false, transform.position, popUpDamage);
    }
}