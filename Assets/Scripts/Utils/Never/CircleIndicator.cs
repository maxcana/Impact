using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicator : MonoBehaviour
{
    SpriteRenderer sr;
    float timer;
    public float life;
    private void Start()
    {
        life = life == 0 ? 2 : life;
        sr = GetComponent<SpriteRenderer>();

        timer = 0;
        transform.localScale = new Vector2(0, 0);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
    }
    private void Update()
    {
        timer += Time.deltaTime;

        sr.color += new Color(0, 0, 0, functions.valueMoveTowards(sr.color.a, timer > life ? 0 : 0.3f, 10));
        transform.localScale += new Vector3(functions.valueMoveTowards(transform.localScale.x, timer > 1f ? 0 : 4, 10), functions.valueMoveTowards(transform.localScale.y, timer > 1f ? 0 : 4, 10));

        if(timer > life && transform.localScale.x < 0.05f)
            Destroy(gameObject);
    }
}
