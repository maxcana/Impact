using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherGravity : MonoBehaviour
{
    CircleCollider2D occ;
    Rigidbody2D orb;
    Vector2 originRigidbody;
    PhysicsMaterial2D originMaterial;
    [SerializeField] PhysicsMaterial2D OtherGravityPhysics;
    [SerializeField] float GravityScale;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Rigidbody2D>() != false && other.GetComponent<CircleCollider2D>() != false)
        {
            orb = other.GetComponent<Rigidbody2D>();
            occ = other.GetComponent<CircleCollider2D>();

            originRigidbody.x = orb.drag;
            originRigidbody.y = orb.gravityScale;
            originMaterial = occ.sharedMaterial;

            orb.drag = 0;
            orb.gravityScale = GravityScale;
            occ.sharedMaterial = OtherGravityPhysics;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        orb.drag = originRigidbody.x;
        orb.gravityScale = originRigidbody.y;
        occ.sharedMaterial = originMaterial;
    }
}
