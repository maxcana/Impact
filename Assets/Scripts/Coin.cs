using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float oy;
    SpriteRenderer sr;
    [SerializeField] bool isCollectable = true;
    [SerializeField] int worth;
    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        oy = transform.position.y;
    }
    private void Update() {
        transform.position = new Vector2(transform.position.x, oy + functions.Sin(Time.time, 10, 0.2f));
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && isCollectable){
            data.coins += worth;
            Destroy(gameObject);
        }
    }
}
