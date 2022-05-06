using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    Transform playerTransform;
    [SerializeField] float slowness;
    Vector2 velocityfirst;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocityfirst.x * 5, velocityfirst.y * 5);
        velocityfirst.x += (playerTransform.position.x - transform.position.x)/(slowness);
        velocityfirst.y += (playerTransform.position.y - transform.position.y)/(slowness);
        velocityfirst.x += (functions.valueMoveTowards(velocityfirst.x, 0, 2));
        velocityfirst.y += (functions.valueMoveTowards(velocityfirst.y, 0, 2));
    }
}
