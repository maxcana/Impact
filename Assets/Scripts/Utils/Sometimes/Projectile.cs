using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 direction = Vector2.zero;
    public float speed = 1;
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * 30 * speed);
    }
}
