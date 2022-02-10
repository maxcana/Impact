using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleScript : MonoBehaviour
{

    public ParticleSystem hitParticle;
    public float VelocityLimit;
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.relativeVelocity.magnitude > VelocityLimit && (other.rigidbody == false || other.rigidbody.mass > 0.9f))
        {
             GameObject.Instantiate(hitParticle, other.GetContact(0).point, Quaternion.identity);
        }
    }
}
