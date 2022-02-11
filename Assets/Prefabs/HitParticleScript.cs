using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleScript : MonoBehaviour
{
    public CameraShake shake;
    public ParticleSystem hitParticle;
    public float VelocityLimit;
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.relativeVelocity.magnitude > VelocityLimit && (other.rigidbody == false || other.rigidbody.mass > 0.9f))
        {
            var contact = other.GetContact(0);
            float degree = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
            ParticleSystem particleClone = GameObject.Instantiate(hitParticle, contact.point, Quaternion.Euler(0, 0, degree));
            var e = particleClone.emission;
            e.SetBurst(0, new ParticleSystem.Burst(0, 2* Mathf.Sqrt(other.relativeVelocity.magnitude)));
            particleClone.Play();

            shake.Shake(0.5f, other.relativeVelocity.sqrMagnitude);
        }
    }
}
