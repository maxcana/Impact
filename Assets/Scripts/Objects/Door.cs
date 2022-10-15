using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool x;
    [SerializeField] bool reverse;
    Vector2 o;
    bool open = false;
    SignalReciever reciever;
    Transform graphicTransform;
    Collider2D doorCollider;
    private void Awake()
    {
        graphicTransform = transform.GetChild(0).GetComponent<Transform>();
        o = graphicTransform.position;
        reciever = GetComponent<SignalReciever>();
        doorCollider = GetComponent<Collider2D>();
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
                doorCollider.enabled = false;
                graphicTransform.Translate(new Vector2(0, functions.valueMoveTowards(graphicTransform.position.y, reverse ? o.y - transform.localScale.y : o.y + transform.localScale.y, 30)));
            }
            else
            {
                doorCollider.enabled = true;
                graphicTransform.Translate(new Vector2(0, functions.valueMoveTowards(graphicTransform.position.y, o.y, 30)));
            }
        } else {
            if (open)
            {
                doorCollider.enabled = false;
                graphicTransform.Translate(new Vector2(functions.valueMoveTowards(graphicTransform.position.x, reverse ? o.x - transform.localScale.x : o.x + transform.localScale.x, 30), 0));
            }
            else
            {
                doorCollider.enabled = true;
                graphicTransform.Translate(new Vector2(functions.valueMoveTowards(graphicTransform.position.x, o.x, 30), 0));
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
