using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oct : MonoBehaviour
{
    [SerializeField] GameObject powerCrystalChallengeGroup;
    [SerializeField] GameObject powerCrystalChallengeGroupTwo;
    [SerializeField] GameObject powerCrystalChallengeGroupThree;
    [SerializeField] GameObject easySpinDeathCircleGroup;
    [SerializeField] GameObject spinDeathCircleGroup;
    [SerializeField] GameObject comet;
    [SerializeField] Vector2 cometMinMaxX;
    [SerializeField] Vector2 cometMinMaxY;
    [SerializeField] GameObject edgeDeathCircleGroup;
    [SerializeField] GameObject fireworksGroupGroup;
    [SerializeField] GameObject hardFireworksGroupGroup;
    [SerializeField] GameObject bouncyBallGroup;
    public enum State { nothing, comets, hardcomets, spin, easyspin, firstspinPowerCrystal, secondspinPowerCrystal, thirdspinPowerCrystal, edges, fireworks, bouncy, hardfireworks }
    State state;
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
        sr = GetComponent<SpriteRenderer>();
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
        if (functions.IsBetweenf(0, 10, counter)) state = State.easyspin; //done
        if (functions.IsBetweenf(10, 20, counter)) state = State.comets; //done
        if (functions.IsBetweenf(29, 40, counter)) state = State.fireworks; //done

        if (functions.IsBetweenf(40, 55, counter)) state = State.firstspinPowerCrystal; //done
        if (functions.IsBetweenf(55, 57, counter)) state = State.nothing; //done

        if (functions.IsBetweenf(57, 72, counter)) state = State.easyspin; //done
        if (functions.IsBetweenf(72, 90, counter)) state = State.bouncy; //done
        if (functions.IsBetweenf(90, 95, counter)) state = State.edges;

        if (functions.IsBetweenf(95, 123, counter)) state = State.secondspinPowerCrystal; //done
        if (functions.IsBetweenf(123, 125, counter)) state = State.nothing; //done

        if (functions.IsBetweenf(125, 140, counter)) state = State.spin; //done
        if (functions.IsBetweenf(140, 160, counter)) state = State.hardcomets; //done
        if (functions.IsBetweenf(165, 180, counter)) state = State.hardfireworks; //done

        if (functions.IsBetweenf(180, 9999, counter)) state = State.thirdspinPowerCrystal; //done

        if (state != pastState)
        {
            OnEnterState(state);
            OnExitState(pastState);
        }

        if (state == State.firstspinPowerCrystal || state == State.secondspinPowerCrystal || state == State.thirdspinPowerCrystal)
        {
            sr.color += functions.colorMoveTowards(sr.color, new Vector4(0, 250f / 255f, 1, 1f), 0.6f, false);
        }
        else
        {
            sr.color += functions.colorMoveTowards(sr.color, new Vector4(1, 1, 1, 1f), 2f, false);
        }

        if (state == State.comets)
        {
            if (cometTimer > 0.3f)
            {
                GameObject comet = Instantiate(this.comet);
                comet.transform.position = new Vector2(Random.Range(cometMinMaxX.x, cometMinMaxX.y), Random.Range(cometMinMaxY.x, cometMinMaxY.y));
                cometTimer = 0;
                comet.SetActive(true);
            }
        }
        if (state == State.hardcomets)
        {
            if (cometTimer > 0.075f)
            {
                //4x more comets
                GameObject comet = Instantiate(this.comet);
                comet.transform.position = new Vector2(Random.Range(cometMinMaxX.x, cometMinMaxX.y), Random.Range(cometMinMaxY.x, cometMinMaxY.y));
                cometTimer = 0;
                comet.SetActive(true);
            }
        }

    }
    public void OnExitState(State theState)
    {
        if (theState == State.firstspinPowerCrystal)
        {
            if (powerCrystalGroup != null)
                Destroy(powerCrystalGroup);
        }
        if (theState == State.secondspinPowerCrystal)
        {
            if (powerCrystalGrouptwo != null)
                Destroy(powerCrystalGrouptwo);
        }
        if (theState == State.thirdspinPowerCrystal)
        {
            if (powerCrystalGroupthree != null)
                Destroy(powerCrystalGroupthree);
        }
        if (theState == State.easyspin)
        {
            if (easySpinDeathCircles != null)
                Destroy(easySpinDeathCircles);
        }
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
        if (theState == State.hardfireworks)
        {
            if (hardFireworksGroups != null)
                Destroy(hardFireworksGroups);
        }

        if (theState == State.bouncy)
        {
            if (bouncyBalls != null)
                Destroy(bouncyBalls);
        }
    }
    GameObject easySpinDeathCircles;
    GameObject spinDeathCircles;
    GameObject edgeDeathCircles;
    GameObject fireworksGroups;
    GameObject hardFireworksGroups;
    GameObject bouncyBalls;
    GameObject powerCrystalGroup;
    GameObject powerCrystalGrouptwo;
    GameObject powerCrystalGroupthree;
    public void OnEnterState(State theState)
    {
        if (theState == State.firstspinPowerCrystal)
        {
            powerCrystalGroup = Instantiate(powerCrystalChallengeGroup);
            powerCrystalGroup.transform.position = new Vector2(Random.Range(cometMinMaxX.x, cometMinMaxX.y), Random.Range(cometMinMaxY.x, cometMinMaxY.y));;
        }
        if (theState == State.secondspinPowerCrystal)
        {
            powerCrystalGrouptwo = Instantiate(powerCrystalChallengeGroupTwo);
            powerCrystalGrouptwo.transform.position = new Vector2(Random.Range(cometMinMaxX.x, cometMinMaxX.y), Random.Range(cometMinMaxY.x, cometMinMaxY.y));;
        }
        if (theState == State.thirdspinPowerCrystal)
        {
            powerCrystalGroupthree = Instantiate(powerCrystalChallengeGroupThree);
            powerCrystalGroupthree.transform.position = new Vector2(Random.Range(cometMinMaxX.x, cometMinMaxX.y), Random.Range(cometMinMaxY.x, cometMinMaxY.y));;
        }
        if (theState == State.easyspin)
        {
            easySpinDeathCircles = Instantiate(easySpinDeathCircleGroup);
            easySpinDeathCircles.transform.position = transform.position;
        }
        if (theState == State.spin)
        {
            spinDeathCircles = Instantiate(spinDeathCircleGroup);
            spinDeathCircles.transform.position = transform.position;
        }
        if (theState == State.edges)
        {
            edgeDeathCircles = Instantiate(edgeDeathCircleGroup);
        }
        if (theState == State.fireworks)
        {
            fireworksGroups = Instantiate(fireworksGroupGroup);
            fireworksGroups.transform.position = new Vector2(11.53f, 15.15f);
        }
        if (theState == State.hardfireworks)
        {
            hardFireworksGroups = Instantiate(hardFireworksGroupGroup);
            hardFireworksGroups.transform.position = new Vector2(11.53f, 15.15f);
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
        if (state == State.firstspinPowerCrystal || state == State.secondspinPowerCrystal || state == State.thirdspinPowerCrystal)
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
                    if (PowerCrystal.playerHasCrystal)
                    {
                        Damage = 1000000;
                        isHyperCritical = true;
                        PowerCrystal.playerHasCrystal = false;
                    }
                }
                popUpDamage = Mathf.Clamp(Damage, 0, Health);

                if (Damage >= 1000000)
                {
                    DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                    DealDamage(Damage, isCritical, isHyperCritical, other.GetContact(0).point, popUpDamage, 1);
                    GameAssets.i.Shake.Shake(0.7f, 10f);
                    // counter = 0;
                }
                else
                {
                    DamageParticleScript.Create(other.GetContact(0).point, popUpDamage, Damage >= Health);
                    DealDamage(Mathf.Ceil(Damage / 100f), isCritical, isHyperCritical, other.GetContact(0).point, Mathf.Ceil(popUpDamage / 100f), other.rigidbody.mass * rb.mass);
                }
            }
        }
    }

    public void DealDamage(float Damage, bool isCritical, bool isHyperCritical, Vector2 position, float popUpDamage, float totalmass = 0)
    {
        Damage = Mathf.Floor(Damage);
        popUpDamage = Mathf.Floor(popUpDamage);
        Health -= Damage;
        DamagePopup.Create(position, popUpDamage, isCritical, totalmass, isHyperCritical, 125);

        if (Health <= 0)
        {

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