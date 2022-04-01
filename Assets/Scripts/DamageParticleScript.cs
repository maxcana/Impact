using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticleScript : MonoBehaviour
{
    float damageAmount;
    bool isAKill;
    public static DamageParticleScript Create(Vector3 position, float Damage, bool isKill)
    {
        Transform damageParticleTransform = Instantiate(GameAssets.i.DamageParticle, position, Quaternion.identity);

        DamageParticleScript damageParticle = damageParticleTransform.GetComponent<DamageParticleScript>();
        damageParticle.Setup(position, Damage, isKill);

        return damageParticle;

    }
    SpriteRenderer sr;
    public void Setup(Vector3 position, float Damage, bool isKill){
        sr.color = new Color(0, 0, 0, 0.7f);
        damageAmount = Damage;
        isAKill = isKill;
    }
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update() {
        float sizeToGetTo;
        
        sizeToGetTo= Mathf.Clamp(damageAmount/100, 0.5f, 1000);
        if(isAKill){sizeToGetTo += 5;}

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + (-0.2f - sr.color.a) *6 * Time.deltaTime);
        transform.localScale = new Vector2(transform.localScale.x + (sizeToGetTo - transform.localScale.x) * 6 * Time.deltaTime, transform.localScale.y + (sizeToGetTo - transform.localScale.y) * 6 * Time.deltaTime);
        if(sr.color.a <= 0){
            Destroy(gameObject);
        }
    }
}
