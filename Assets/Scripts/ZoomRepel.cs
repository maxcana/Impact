using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomRepel : MonoBehaviour
{
    Vector2 size;
    float currentSize;
    float lerpSize;
    Vector2 orbForce;
    private void Start() {
        lerpSize = 0;
        currentSize = (transform.localScale.x + transform.localScale.y) / 2;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        lerpSize = 1;
    }
    private void Update() {
        lerpSize += Time.deltaTime * 100 * ((0 - lerpSize) / 5);
        size.x = Mathf.Lerp(currentSize, currentSize * 1.3f, lerpSize);
        size.y = size.x;
        transform.localScale = size;
    }
}
