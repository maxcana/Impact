using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    //? im worried that origin gravity gets set by other objects with different gravity scales, but i don't know how to fix that
    float originGravity;
    [SerializeField] PhysicsMaterial2D mat;
    float originFriction;
    float originBounciness;
    float originLinearDrag;
    Rigidbody2D orb;
    CircleCollider2D occ;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Rigidbody2D>() != false && other.GetComponent<CircleCollider2D>() != false)
        {
            orb = other.GetComponent<Rigidbody2D>();
            occ = other.GetComponent<CircleCollider2D>();
            originLinearDrag = orb.drag;
            originFriction = mat.friction;
            originBounciness = mat.bounciness;
            mat.friction = 0;
            mat.bounciness = 1;
            originGravity = orb.gravityScale;
            orb.gravityScale = 0;
            orb.drag = 0;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        orb.gravityScale = originGravity;
        mat.friction = originFriction;
        mat.bounciness = originBounciness;
        orb.drag = originLinearDrag;
    }
}
