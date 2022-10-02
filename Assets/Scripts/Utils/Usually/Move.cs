using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class Move : MonoBehaviour
{
    public Vector3 direction;
    public Vector3 increaseOverTime = new Vector3();
    public float maxIncrease = 0;
    Vector3 moveAmount;
    private void Awake() {
        moveAmount = direction;
    }
    void LateUpdate()
    {
        moveAmount.x += increaseOverTime.x * Time.deltaTime;
        moveAmount.y += increaseOverTime.y * Time.deltaTime;
        moveAmount.z += increaseOverTime.z * Time.deltaTime;
        Mathf.Clamp(moveAmount.x, float.MinValue, direction.x + maxIncrease);
        Mathf.Clamp(moveAmount.y, float.MinValue, direction.y + maxIncrease);
        Mathf.Clamp(moveAmount.z, float.MinValue, direction.z + maxIncrease);
        transform.Translate(direction * Time.deltaTime);
    }
}
