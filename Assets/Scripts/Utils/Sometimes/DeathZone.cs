using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeathZone : MonoBehaviour
{
    public bool explodeOnHit;
    [SerializeField] GameObject explosionObject;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] ParticleSystem explosion;
    public ParticleSystem onDeath;
    public float TimeUntilDeath;
    public string[] killTags;
    public string[] ignoreTags;
    public bool dieOnHitWithNonIgnoreTagsObject = true;
    float timer;
    private void Start() {
        timer = 0;
    }
    private void Update() {
        timer += Time.deltaTime;
    }
    public void Explode()
    {
        print("boom");
        
        ParticleSystem particleClone = GameObject.Instantiate(explosion, transform.position, Quaternion.Euler(90, 0, 0));
        GameAssets.i.Shake.Shake(0.15f, 15);
        GameAssets.i.sound.PlayOneShot(explosionSound, 0.3f);

        short strength = (short)Mathf.Clamp((explosionObject.GetComponent<Explosion>().customForce) / 100f, 1, 20);
        var main = particleClone.main;
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, (Int16)(24 * strength), (Int16)(36 * strength), 11, 0.01f);
        particleClone.emission.SetBurst(0, burst);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0f, 8f * strength);
        main.startSize = 1 + (strength / 4);
        particleClone.Play();

        explosionObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (killTags != null)
        {
            if (killTags.Length > 0)
            {
                if (!ignoreTags.Contains(other.tag) || ignoreTags == null)
                {
                    if (!killTags.Contains(other.tag))
                    {
                        if (dieOnHitWithNonIgnoreTagsObject)
                        {
                            if(timer < 0.1f){
                                return;
                            }
                            if(onDeath != null){
                            ParticleSystem particles = Instantiate(onDeath, transform.position, Quaternion.Euler(90, 0, 0));
                            particles.Play();}
                            if(explodeOnHit){Explode();}

                            GetComponent<Collider2D>().enabled = false;
                            TryGetComponent<SpriteRenderer>(out SpriteRenderer sr);
                            if(sr != null) sr.enabled = false;

                            Destroy(gameObject, 1f);
                            return;
                        }
                    }
                }
                else { return; }
            }
        }

        if (other.tag == "Explosive" || other.tag == "ExplosiveCollider")
        {
            if (other.tag == "ExplosiveCollider")
            {
                StartCoroutine(other.GetComponent<Explosive>().Explode());
            }
            else
            {
                StartCoroutine(other.transform.GetChild(0).GetComponent<Explosive>().Explode());
            }
        }
        else
        {
            if (other.tag != "Undestroyable")
            {
                if (other.tag == "Enemy")
                {
                    EnemyBehavior e = other.GetComponent<EnemyBehavior>();
                    e.DealDamages(e.Health, false, false, 0);
                }
                else
                {
                    Destroy(other.gameObject, TimeUntilDeath);
                }

            }
        }
    }
}
