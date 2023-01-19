using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oct : MonoBehaviour
{
    [SerializeField] GameObject spinDeathCircleGroup;
    [SerializeField] GameObject comet;
    [SerializeField] Vector2 cometMinMaxX;
    [SerializeField] Vector2 cometMinMaxY;
    [SerializeField] GameObject edgeDeathCircleGroup;
    [SerializeField] GameObject fireworksGroupGroup;
    [SerializeField] GameObject bouncyBallGroup;
    public enum State { damageable, comets, spin, spinPowerCrystal, edges, fireworks, bouncy }
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
    State pastState;
    float cometTimer = 0;
    private void Update()
    {
        cometTimer += Time.deltaTime;
        counter += Time.deltaTime;

        pastState = state;
        if (functions.IsBetweenf(0, 10, counter)) state = State.spin;
        if (functions.IsBetweenf(10, 30, counter)) state = State.comets;
        if (functions.IsBetweenf(30, 45, counter)) state = State.fireworks;
        if (functions.IsBetweenf(53, 60, counter)) state = State.bouncy;
        if (functions.IsBetweenf(60, 70, counter)) state = State.edges;
        if (functions.IsBetweenf(70, 80, counter)) state = State.spinPowerCrystal;
        if (functions.IsBetweenf(80, 10000, counter)) state = State.damageable;
        if (state != pastState)
        {
            OnEnterState(state);
            OnExitState(pastState);
        }

        if (state == State.comets)
        {
            if (cometTimer > 0.3f)
            {
                GameObject comet = Instantiate(this.comet);
                comet.transform.position = new Vector2(Random.Range(cometMinMaxX.x, cometMinMaxX.y), Random.Range(cometMinMaxY.x, cometMinMaxY.y));
                // functions.SpawnCircle(comet.transform.position, 0.5f);
                cometTimer = 0;
                comet.SetActive(true);
            }
        }
    }
    public void OnExitState(State theState)
    {
        if (theState == State.spin)
        {
            if (spinDeathCircles != null)
                Destroy(spinDeathCircles);
        }
        if (theState == State.edges)
        {
            if (edgeDeathCircles != null)
                Destroy(edgeDeathCircles);
        }
        if (theState == State.fireworks)
        {
            if (fireworksGroups != null)
                Destroy(fireworksGroups);
        }
        if (theState == State.bouncy)
        {
            if (bouncyBalls != null)
                Destroy(bouncyBalls);
        }
    }
    GameObject spinDeathCircles;
    GameObject edgeDeathCircles;
    GameObject fireworksGroups;
    GameObject bouncyBalls;
    public void OnEnterState(State theState)
    {
        if (theState == State.spin)
        {
            spinDeathCircles = Instantiate(spinDeathCircleGroup);
            spinDeathCircles.transform.position = transform.position;
        }
        if (theState == State.edges)
        {
            edgeDeathCircles = Instantiate(edgeDeathCircleGroup);
            edgeDeathCircles.transform.position = transform.position;
        }
        if (theState == State.fireworks)
        {
            fireworksGroups = Instantiate(fireworksGroupGroup);
            fireworksGroups.transform.position = new Vector2(11.53f, 15.15f);
        }
        if (theState == State.bouncy)
        {
            bouncyBalls = Instantiate(bouncyBallGroup);
            bouncyBalls.transform.position = transform.position;
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
        if (state == State.damageable)
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

                if (Damage >= 1000000)
                {
                    DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                    DealDamage(Damage, isCritical, isHyperCritical, other.GetContact(0).point, popUpDamage, other.rigidbody.mass * rb.mass);
                    counter = 0;
                }
                else
                {
                    DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                    DealDamage(Mathf.Ceil(Damage / 100f), isCritical, isHyperCritical, other.GetContact(0).point, Mathf.Ceil(popUpDamage / 100f), other.rigidbody.mass * rb.mass);
                    counter = 0;
                }
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