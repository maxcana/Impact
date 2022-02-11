using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithSine : MonoBehaviour
{
    private Vector2 startPosition;
    public bool moveX;
    public bool moveY;
    public float moveAmount;
    public float moveSpeed;
    public float startingSineX;
    public float startingSineY;
    private float timeCount = 0;

    private void Start() 
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        timeCount += Time.deltaTime;
        if(moveX)
        {
            transform.position = new Vector2(startPosition.x + Mathf.Cos(startingSineX + timeCount * moveSpeed) * moveAmount, transform.position.y);
        }

        if(moveY)
        {
            transform.position = new Vector2(transform.position.x, startPosition.y + Mathf.Sin(startingSineY + timeCount * moveSpeed) * moveAmount);
        }
    }
}