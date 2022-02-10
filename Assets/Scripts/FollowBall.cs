using System;
using UnityEngine;
using Cinemachine;
public class FollowBall : MonoBehaviour
{    
    CinemachineVirtualCamera cam;
    public Vector2 offset;
    private Rigidbody2D ballrb;
    //private Camera cam;
    public float sizeMin = 5;
    public float sizeMax = 8;
    public float velocitySizeMultiplier = 0.01f;
    public float zoomSpeed;
    void Awake()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        ballrb = Player.GetComponent<Rigidbody2D>();
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.Follow = Player.transform;
    }

    void LateUpdate()
    {
        float desiredSize = Mathf.Lerp(sizeMin, sizeMax, Math.Abs(ballrb.velocity.magnitude)*velocitySizeMultiplier);
        cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, desiredSize, Time.deltaTime * zoomSpeed);
    }
}
