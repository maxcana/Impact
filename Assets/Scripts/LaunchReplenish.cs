using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchReplenish : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            Ball.launches = 1;
        }
    }
}
