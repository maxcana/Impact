using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHitParticle : MonoBehaviour
{
    public CameraShake shake;
    public ParticleSystem hitParticle;
    [SerializeField] float VelocityLimit;
    [SerializeField] float rbMassLimit;
    private Rigidbody2D rb;
    float velocityBeforePhysicsUpdate;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
     void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rb.velocity.magnitude;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //! BUG: It says the velocity on collision is very low, which means that once it collides, it gets the new velocity, but I want the velocity before it collides...
        //! The only way I got this was by checking every frame which is kinda bad...

        //Debug.Log("Box says: my velocity before is " + velocityBeforePhysicsUpdate);
        if(Mathf.Abs(velocityBeforePhysicsUpdate) > VelocityLimit && (other.rigidbody == false || other.rigidbody.mass > rbMassLimit))
        {
            var contact = other.GetContact(0);
            //! Problem: It always spawns on a corner because of the square/rectangle collision points, if the angle is small enough, it should spawn in the middle instead.

            float degree = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
            ParticleSystem particleClone = GameObject.Instantiate(hitParticle, contact.point, Quaternion.Euler(0, 0, degree));
            var e = particleClone.emission;
            e.SetBurst(0, new ParticleSystem.Burst(0, UnityEngine.Random.Range(5,8)));
            particleClone.Play();
            //! NOT WORKING (SHAKE)
            shake.Shake(0.1f, 2);
            Debug.Log("Tried to spawn particles");

            //shake.Shake(0.5f, other.relativeVelocity.sqrMagnitude);
        }
    }
}
