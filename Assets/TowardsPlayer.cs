using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    Transform playerTransform;
    [SerializeField] float slowness;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        rb.AddForce(new Vector2((playerTransform.position.x - transform.position.x)/(10 * slowness), (playerTransform.position.y - transform.position.y)/(10 * slowness)), ForceMode2D.Impulse);
    }
}
