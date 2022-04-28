using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float oy;
    SpriteRenderer sr;
    [SerializeField] float bobAmount = 0.2f;
    [SerializeField] bool isCollectable = true;
    [SerializeField] bool doesBob = true;
    [SerializeField] public int worth;
    public AudioClip collect;
    bool isCollecting = false;
    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        oy = transform.position.y;
    }
    private void Update() {
        if(doesBob) transform.position = new Vector3(transform.position.x, oy + functions.Sin(Time.time, 10, bobAmount), 1);
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
        if(isCollecting){
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + functions.valueMoveTowards(sr.color.a, 0, 30));
            transform.localScale += new Vector3(Time.deltaTime *4, Time.deltaTime *8, 0);
            if(sr.color.a < 0.05f){
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && isCollectable){
            data.coins += worth;
            PlayerPrefs.SetInt("Coins", data.coins);
            GameAssets.i.sound.PlayOneShot(collect);
            isCollectable = false;
            isCollecting = true;
        }
    }
}
