using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchReplenish : MonoBehaviour
{
    Ball ball;
    private void Start() {
        ball = GameObject.FindWithTag("Player").GetComponent<Ball>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player" && !ball.dontRegainLaunches)
        {
            ball.launches = 1;
        }
    }
}
