using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tri : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] public float MaxHealthIsSetTo = 100;
    public float MaxHealth = 100;
    public float Health
    {
        get { return m_Health; }
        set { m_Health = Mathf.Clamp(value, 0, MaxHealth); }
    }
    float m_Health;
    private bool isCritical;
    [SerializeField] bool AutoSetHealth;
    private bool willKillOnHit;
    float timeSinceLastDamageTaken;
    [SerializeField] float fadeSpeed;
    [SerializeField] float bossLevel;
    float dangerTime;
    Rigidbody2D rb;
    bool dead;
    bool ballIsClose;
    SpriteRenderer sr;
    PolygonCollider2D pc2d;
    bool isTeleporting;

    private void Start()
    {
        isTeleporting = false;
        dead = false;
        pc2d = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        willKillOnHit = false;
        if (AutoSetHealth) { MaxHealth = bossLevel * 500; } else { MaxHealth = MaxHealthIsSetTo; }
        Health = MaxHealth;
        timeSinceLastDamageTaken = Time.time;
        rb.gravityScale = 0;
        pc2d.enabled = true;
    }
    private void Update()
    {
        float stepTime = Time.deltaTime * (willKillOnHit ? fadeSpeed : -fadeSpeed);
        dangerTime = Mathf.Clamp01(dangerTime + stepTime);
        if (Health == 0 && !dead)
        {
            dead = true;
            StartCoroutine(Die(4f));
        }
        if (functions.TakeChance(Time.deltaTime) && !isTeleporting)
        {
            Vector2 pos = GetUnoccupiedPosition();
            functions.SpawnCircle(pos, 0.2f);
            StartCoroutine(Teleport(pos, 0.2f));
        }

        var player = GameObject.FindWithTag("Player");
        if (Vector2.Distance(player.transform.position, transform.position) < 5)
        {
            if (!ballIsClose)
            {
                if(!isTeleporting){
                Vector2 pos = GetUnoccupiedPosition();
                StartCoroutine(Teleport(pos, 0.2f));}
            }
            ballIsClose = true;
        }
        else
        {
            ballIsClose = false;
        }

        if (dead)
            return;
        //?heals over time
        if (Time.time > timeSinceLastDamageTaken + Mathf.Clamp((8f - (bossLevel / 20)), 0, 8) && Health != 0)
        {
            if (UnityEngine.Random.Range(1, (int)Mathf.Clamp(26 - Mathf.Ceil(bossLevel / 10), 1, 26)) == 1)
            {
                HealDamage(Mathf.Round(UnityEngine.Random.Range(-10, (0 - 20 - (bossLevel / 3)))), Random.Range(0, 100) < Mathf.Clamp((20 - bossLevel / 10), 0, 100));
            }
        }
    }
    public Vector2 GetUnoccupiedPosition()
    {
        var player = GameObject.FindWithTag("Player");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("TeleportPosition");
        Vector2[] positions = new Vector2[gameObjects.Length];
        int i = 0;
        foreach (var gameObject in gameObjects)
        {
            positions[i] = gameObject.transform.position;
            i++;
        }

        i = 0;
        bool found = false;
        List<Vector2> possiblePositions = new List<Vector2>();
        RaycastHit2D[] results = new RaycastHit2D[1];
        while (i < positions.Length)
        {
            if (!Physics2D.OverlapCircle(positions[i], transform.localScale.magnitude / 2))
            {
                float distance = Vector2.Distance(transform.position, positions[i]);
                if (8 < Vector2.Distance((Vector2)player.transform.position + player.GetComponent<Rigidbody2D>().velocity * 3, positions[i]))
                {
                    possiblePositions.Add(positions[i]);
                    found = true;
                }
            }
            i++;
        }
        if (found)
        {
            if (!isTeleporting)
            {
                Vector2 pos = possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
                return pos;
            }
        }
        return transform.position;
    }
    public IEnumerator Teleport(Vector2 position, float delay)
    {
        isTeleporting = true;
        Collider2D collider2d = GetComponent<Collider2D>();
        collider2d.enabled = false;
        while (transform.localScale.x > 0.02f)
        {
            sr.color += new Color(0, 0, 0, functions.valueMoveTowards(sr.color.a, 0, 30));
            transform.localScale += (Vector3)functions.positionMoveTowards(transform.localScale, new Vector2(0, 0), 30);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        transform.localScale = new Vector2(0.0001f, 0.0001f);
        yield return new WaitForSeconds(delay);

        transform.position = position;
        transform.rotation = Quaternion.Euler(0,0,0);
        rb.angularVelocity = functions.Random(-50, 50);
        while (transform.localScale.x < 0.1791f)
        {
            sr.color += new Color(0, 0, 0, functions.valueMoveTowards(sr.color.a, 1, 30));
            transform.localScale += (Vector3)functions.positionMoveTowards(transform.localScale, new Vector2(0.19f, 0.19f), 30);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        collider2d.enabled = true;
        transform.localScale = new Vector2(0.1791f, 0.1791f);
        isTeleporting = false;
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
    public IEnumerator Die(float delay)
    {
        GetComponent<Shards>().Disperse(transform.position);
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
