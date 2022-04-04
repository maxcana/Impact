using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float oy;
    SpriteRenderer sr;
    [SerializeField] int worth;
    bool isCollecting = false;
    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        oy = transform.position.y;
    }
    private void Update() {
        if(!isCollecting){
            transform.position = new Vector2(transform.position.x, oy + functions.Sin(Time.time, 10, 0.2f));
        } else {
            transform.position = new Vector2(transform.position.x, transform.position.y + Time.deltaTime);
            transform.localScale += Vector3.one * Time.deltaTime;
            if(transform.localScale.y > 1f){
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            isCollecting = true;
            data.coins += worth;
        }
    }
}
