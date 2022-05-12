using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool x;
    SpriteRenderer sr;
    Vector2 o;
    bool open = false;
    SignalReciever reciever;
    private void Awake()
    {
        o = transform.position;
        reciever = GetComponent<SignalReciever>();
    }
    private void OnEnable()
    {
        reciever.onRecieveSignal += OpenDoor;
    }

    private void Update()
    {
        if (!x)
        {
            if (open)
            {
                transform.Translate(new Vector2(0, functions.valueMoveTowards(transform.position.y, o.y + transform.localScale.y * 1.02f, 20)));
            }
            else
            {
                transform.Translate(new Vector2(0, functions.valueMoveTowards(transform.position.y, o.y, 20)));
            }
        } else {
            if (open)
            {
                print("open");
                transform.Translate(new Vector2(functions.valueMoveTowards(transform.position.x, o.x + transform.localScale.x * 1.02f, 20), 0));
            }
            else
            {
                transform.Translate(new Vector2(functions.valueMoveTowards(transform.position.x, o.x, 20), 0));
            }
        }
    }
    private void OpenDoor(bool value)
    {
        open = value;
    }

    private void OnDisable()
    {
        reciever.onRecieveSignal -= OpenDoor;
    }
}
