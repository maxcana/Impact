using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenerThingThatIsDefinitelyNotAButton : MonoBehaviour
{
    SpriteRenderer sr;
    Vector2 o;
    SignalSender sender;
    bool pressed = false;
    List<Collider2D> thingsOnTheButton = new List<Collider2D>();
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        o = transform.position;
        sender = GetComponent<SignalSender>();
    }
    private void Update()
    {
        if (pressed)
        {
            transform.Translate(new Vector2(0f, functions.valueMoveTowards(transform.position.y, o.y - transform.localScale.y * 0.6f, 10)));
            float toChange = functions.valueMoveTowards(sr.color.r, 191f / 255f, 10);
            sr.color += new Color(toChange, toChange, toChange);
        }
        else
        {
            transform.Translate(new Vector2(0f, functions.valueMoveTowards(transform.position.y, o.y, 10)));
            float toChange = functions.valueMoveTowards(sr.color.r, 227f / 255f, 10);
            sr.color += new Color(toChange, toChange, toChange);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            if (thingsOnTheButton.Count == 0)
            {
                sender.SetSignal(true);
                pressed = true;
            }
            if (!thingsOnTheButton.Contains(other))
                thingsOnTheButton.Add(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            thingsOnTheButton.Remove(other);
            if (thingsOnTheButton.Count == 0)
            {
                sender.SetSignal(false);
                pressed = false;
            }
        }
    }
}
