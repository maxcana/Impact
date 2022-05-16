using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishableObject : MonoBehaviour
{
    [SerializeField] const float respawnTime = 2.5f;
    float timer = 0f;
    bool isRunning;
    bool isTouchingButton;
    Collider2D anyCollider;
    Vector2 o;
    Vector3 ov3;
    Rigidbody2D rb;
    SpriteRenderer sr;
    private void Awake()
    {
        anyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        isTouchingButton = false;
        o = transform.position;
        ov3 = new Vector3(o.x, o.y);
        isRunning = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Button")
        {
            isTouchingButton = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Button")
        {
            isTouchingButton = false;
        }
    }
    private void Update()
    {
        if (IsNotMoving() && Vector2.Distance(o, transform.position) > 1 && isTouchingButton == false)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        if (timer > respawnTime && !isRunning)
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
        anyCollider.enabled = false;
        //Move Toward Starting Position
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        while (Vector2.Distance(transform.position, o) > 0.1f)
        {
            float speed = 10f;
            sr.color += new Color(0, 0, 0, functions.valueMoveTowards(sr.color.a, 1, speed));
            transform.position += new Vector3(functions.positionMoveTowards(transform.position, o, speed).x, functions.positionMoveTowards(transform.position, o, speed).y);
            //! doesnt work
            //transform.rotation = (Quaternion.Euler(0, 0, transform.rotation.z % 360 + functions.valueMoveTowards(transform.rotation.eulerAngles.z, 0, speed)));
            yield return null;
        }
        transform.position = o;
        transform.rotation = (Quaternion.Euler(0,0,0));
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        //Move Toward Starting Position End
        anyCollider.enabled = true;
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
        timer = 0;
        isRunning = false;
    }
}
