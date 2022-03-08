using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    Rigidbody2D rb;
    public float xvZoom = 1f;
    public float yvZoom = 1f;
    [SerializeField] bool OneWay;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        rb = other.GetComponent<Rigidbody2D>();

            if(OneWay){
                if(xvZoom > 0 && yvZoom == 0){
                    if(rb.velocity.x < 0){
                        rb.velocity = new Vector2(xvZoom, rb.velocity.y);
                    }
                }
                if(xvZoom < 0 && yvZoom == 0){
                    if(rb.velocity.x > 0){
                        rb.velocity = new Vector2(xvZoom, rb.velocity.y);
                    }
                }
                if(yvZoom > 0 && xvZoom == 0){
                    if(rb.velocity.y < 0){
                        rb.velocity = new Vector2(rb.velocity.x, yvZoom);
                    }
                }
                if(yvZoom > 0 && xvZoom == 0){
                    if(rb.velocity.y < 0){
                        rb.velocity = new Vector2(rb.velocity.x, yvZoom);
                    }
                }
            } 
            else {
            if(yvZoom == 0) {rb.velocity = new Vector2(xvZoom, rb.velocity.y);}
            if (xvZoom == 0) {rb.velocity = new Vector2(rb.velocity.x, yvZoom);}
            if(xvZoom != 0 & yvZoom != 0) {rb.velocity = new Vector2(xvZoom, yvZoom);}
            if(xvZoom == 0 & yvZoom == 0){return;}
            }
        //? If the zoom change is 0, keeps the original speed
        //? If both zoom changes are 0, does nothing (keeps the original speed)
    }
}
