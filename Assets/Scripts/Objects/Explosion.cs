using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    CircleCollider2D collider2d;
    [SerializeField] Explosive ourExplosiveFriend;
    [SerializeField] bool useExplosiveFriend = true;
    public float customForce;
    float timer = 0;
    private void Awake()
    {
        collider2d = GetComponent<CircleCollider2D>();
        if(useExplosiveFriend)
            gameObject.SetActive(false);
        if (useExplosiveFriend)
        {
            float force = ourExplosiveFriend.setForce ? ourExplosiveFriend.force : data.explosionForce;
            collider2d.radius = Mathf.Clamp(force / 14, 4, 100);
        }
        else
        {
            float force = customForce;
            collider2d.radius = Mathf.Clamp(force / 14, 4, 100);
            print("awake + radius = " + collider2d.radius);
        }
    }
    private void OnEnable()
    {
        
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.4f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("ontrigger2d");
        if (other.attachedRigidbody != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, other.transform.position);
            float force;
            Vector2 velocity;
            if (useExplosiveFriend)
            {
                force = ourExplosiveFriend.setForce ? (direction * ourExplosiveFriend.force * Mathf.Clamp01(1 - (distance / collider2d.radius))).magnitude : (direction * data.explosionForce * Mathf.Clamp01(1 - (distance / collider2d.radius))).magnitude;
                velocity = ourExplosiveFriend.setForce ? (direction * ourExplosiveFriend.force * Mathf.Clamp01(1 - (distance / collider2d.radius))) : (direction * data.explosionForce * Mathf.Clamp01(1 - (distance / collider2d.radius)));
            } else {
                force = (direction * customForce * Mathf.Clamp01(1 - (distance / collider2d.radius))).magnitude;
                velocity = (direction * customForce * Mathf.Clamp01(1 - (distance / collider2d.radius)));
            }
            other.attachedRigidbody.velocity = velocity;

            if (other.tag == "Enemy")
            {
                //! List Every Boss Here (to take explosion damage)!
                float damageToDeal = Mathf.Round(force * 10);
                print("enemy should have taken " + damageToDeal + " damage.");
                if (other.TryGetComponent<EnemyBehavior>(out EnemyBehavior eb)) eb.DealDamageWithAutoPopupDamageAtDefaultPosition(damageToDeal);
                if (other.TryGetComponent<BossScript>(out BossScript bs)) bs.DealDamageWithAutoPopupDamageAtDefaultPosition(damageToDeal);
                if (other.TryGetComponent<BigEnemyGuy>(out BigEnemyGuy beg)) beg.DealDamageWithAutoPopupDamageAtDefaultPosition(damageToDeal);
            }
        }
    }
    private void Start()
    {
        Debug.LogWarning("Don't forget about this when adding new bosses!");
    }
}
