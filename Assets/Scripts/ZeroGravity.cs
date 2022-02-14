using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    //? im worried that origin gravity gets set by other objects with different gravity scales, but i don't know how to fix that
    float originGravity;
    Rigidbody2D orb;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Rigidbody2D>() != false)
        {
            orb = other.GetComponent<Rigidbody2D>();
            originGravity = orb.gravityScale;
            orb.gravityScale = 0;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        orb.gravityScale = originGravity;
    }
}
