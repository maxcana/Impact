using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishableObject : MonoBehaviour
{
    float timeSpentNotMoving = 0f;
    bool isRunning;
    Collider2D collider2D;
    Vector2 o;
    Vector3 ov3;
    Rigidbody2D rb;
    SpriteRenderer sr;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        o = transform.position;
        ov3 = new Vector3(o.x, o.y);
        isRunning = false;
    }
    private void Update()
    {
        if (IsNotMoving() && Vector2.Distance(o, transform.position) > 1)
        {
            timeSpentNotMoving += Time.deltaTime;
        }
        else
        {
            timeSpentNotMoving = 0;
        }
        if (timeSpentNotMoving > 5 && !isRunning)
        {
            isRunning = true;
            StartCoroutine(flash(35));
        }
    }
    public IEnumerator flash(int amountOfFlashCycles)
    {
        for (int i = 0; i < amountOfFlashCycles; i++)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
            if (!IsNotMoving())
            {
                ExitCoroutine();
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            if (!IsNotMoving())
            {
                ExitCoroutine();
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        
        rb.simulated = false;
        collider2D.enabled = false;
        //Move Toward Starting Position
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        while(Vector2.Distance(transform.position, o) > 0.1f){
            print("frame");
            //! This is not fps independent, yet it doesn't lerp right, instead of gliding, it teleports...

            float speed = 10f;
            sr.color += new Color(0,0,0,functions.valueMoveTowards(sr.color.a, 1, speed));
            transform.position += new Vector3(functions.positionMoveTowards(transform.position, o, speed).x, functions.positionMoveTowards(transform.position, o, speed).y);
        }
        transform.position = o;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        //Move Toward Starting Position End
        collider2D.enabled = true;
        rb.simulated = true;
        ExitCoroutine();
        yield break;
    }
    bool IsNotMoving()
    {
        return rb.velocity.magnitude < 0.04f;
    }
    void ExitCoroutine()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        print("Replenishable object started moving again or spawned at original position");
        timeSpentNotMoving = 0;
        isRunning = false;
    }
}
