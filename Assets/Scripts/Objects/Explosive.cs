using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Explosive : MonoBehaviour
{
    GameObject explosionObject;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] ParticleSystem explosion;
    public bool setForce;
    public float force = float.NaN;
    private void Start()
    {
        explosionObject = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.attachedRigidbody == null || other.tag == "ExplosiveCollider" || other.tag == "Explosive"))
        {
            if (Mathf.Abs(other.attachedRigidbody.velocity.magnitude) > 0.2f || Mathf.Abs(other.attachedRigidbody.angularVelocity) > 0.2f)
            {
                if(other.tag == "Explosion")
                    StartCoroutine(Explode(0.3f)); 
                else 
                    StartCoroutine(Explode());
            }
        }
    }
    public IEnumerator Explode(float delay = 0)
    {
        print("boom");
        if(delay > 0)
            yield return new WaitForSeconds(delay);
        
        ParticleSystem particleClone = GameObject.Instantiate(explosion, transform.position, Quaternion.Euler(90, 0, 0));
        GameAssets.i.Shake.Shake(0.15f, 15);
        GameAssets.i.sound.PlayOneShot(explosionSound, 0.3f);

        short strength = (short)Mathf.Clamp((setForce ? force : data.explosionForce) / 100f, 1, 20);
        var main = particleClone.main;
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, (Int16)(24 * strength), (Int16)(36 * strength), 11, 0.01f);
        particleClone.emission.SetBurst(0, burst);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0f, 8f * strength);
        main.startSize = 1 + (strength / 4);
        particleClone.Play();

        explosionObject.SetActive(true);
        Destroy(gameObject);
        Destroy(transform.parent.gameObject, 0.2f);
        yield break;
    }
}
