using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIfTouchingBall : MonoBehaviour
{
    SpriteRenderer sr;
    bool to0;
    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        to0 = false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
                to0 = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            to0 = false;
        }
    }
    private void Update() {
        if(to0){
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + functions.valueMoveTowards(sr.color.a, 0.2f, 6f));
        } else {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + functions.valueMoveTowards(sr.color.a, 1f, 12f));
            }
    }
}
