using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer sr;
    Vector2 o;
    bool open = false;
    SignalReciever reciever;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        o = transform.position;
        reciever = GetComponent<SignalReciever>();
    }
    private void OnEnable()
    {
        reciever.onRecieveSignal += OpenDoor;
    }

    private void Update()
    {
        if (open)
        {
            sr.color += new Color(0,0,0,functions.valueMoveTowards(sr.color.a, 0, 10));
            transform.Translate(new Vector2(0, functions.valueMoveTowards(transform.position.y, o.y + transform.localScale.y * 1.02f, 10)));
        }
        else
        {
            sr.color += new Color(0,0,0,functions.valueMoveTowards(sr.color.a, 1, 10));
            transform.Translate(new Vector2(0, functions.valueMoveTowards(transform.position.y, o.y, 10)));
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
