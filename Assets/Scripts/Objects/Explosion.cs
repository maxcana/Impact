using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Explosive ourExplosiveFriend;
    float timer = 0;
    private void Awake()
    {
        ourExplosiveFriend = transform.parent.GetChild(0).GetComponent<Explosive>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.15f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, other.transform.position);


            Vector2 velocity = ourExplosiveFriend.setForce ? (direction * ourExplosiveFriend.force * Mathf.Clamp01(1 - (distance / GetComponent<CircleCollider2D>().radius))) : (direction *  data.explosionForce * Mathf.Clamp01(1 - (distance / GetComponent<CircleCollider2D>().radius)));
            other.attachedRigidbody.velocity = velocity;

            if (other.tag == "Enemy")
            {
                //! List Every Boss Here!
                EnemyBehavior eb;
                BossScript bs;
                BigEnemyGuy beg;
                float damageToDeal = ourExplosiveFriend.setForce ? (10 + Mathf.Ceil(100 * (ourExplosiveFriend.force * Mathf.Clamp01(1 - (distance / GetComponent<CircleCollider2D>().radius))))) : (10 + Mathf.Ceil(100 * (data.explosionForce * Mathf.Clamp01(1 - (distance / GetComponent<CircleCollider2D>().radius)))));
                if (TryGetComponent(out eb)) eb.DealDamageWithAutoPopupDamageAtDefaultPosition(damageToDeal);
                if (TryGetComponent(out bs)) bs.DealDamageWithAutoPopupDamageAtDefaultPosition(damageToDeal);
                if (TryGetComponent(out beg)) beg.DealDamageWithAutoPopupDamageAtDefaultPosition(damageToDeal);
            }
        }
    }
    private void Start()
    {
        Debug.LogWarning("Don't forget about this when adding new bosses!");
        //goto Line 33
    }
}
