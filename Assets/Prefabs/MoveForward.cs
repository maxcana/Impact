using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private void Update() {
        transform.position += transform.forward;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Circle" & other.tag != "Player") {Destroy(gameObject);}
    }
}
