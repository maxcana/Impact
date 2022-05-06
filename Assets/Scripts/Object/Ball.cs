using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    Volume postprocessing;
    Vector2 position;
    Rigidbody2D rb;
    CircleCollider2D cc2d;
    LineRenderer launchLine;
    public float slowMotionSpeed = 0.2f;
    public float slowDownTime = 0.33f;
    private float currentSlowDown = 0f;
    public bool inBossFight;
    private float leftMouseTime;
    private float rightMouseTime;
    public static int launches = 1;
    public static float maxLaunchDistance = 5f;
    public bool willLaunchOnHit;
    [SerializeField] PhysicsMaterial2D ballPrivaterb;
    private Camera cam;
    private void Awake()
        {
            cc2d = GetComponent<CircleCollider2D>();
            launches = 1;
            postprocessing = GetComponent<Volume>();
            cam = Camera.main;
            launchLine = GetComponent<LineRenderer>();
            rb = GetComponent<Rigidbody2D>();
            launchLine.enabled = false;
        }
        private void Start() {
            cc2d.sharedMaterial = ballPrivaterb;
            rb.gravityScale = 1;
            rb.drag = 0f;
        }

    void Update()
    {
        if(0.001f > Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y)) {if(rb.gravityScale != 0 && SceneManager.GetActiveScene().name != "Level13"){launches = 1;}}
        if(rb.gravityScale != 0){if(0.0012f > Mathf.Abs(rb.velocity.x) && SceneManager.GetActiveScene().name != "Level5" && SceneManager.GetActiveScene().name != "Level13") {{launches = 1;}}
        if((0.001f >= Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y)) && rb.gravityScale == 0){Destroy(gameObject);} else if((0.05f > Mathf.Abs(rb.velocity.x) || 0.05f > Mathf.Abs(rb.velocity.y)) && rb.gravityScale == 0){rb.AddForce(rb.velocity);}
        if(0.001f > Mathf.Abs(rb.velocity.y)) {if(rb.gravityScale != 0){launches = 1;}}
        //? A bunch of edge cases where you get stuck at 0 velocity


        if(!inBossFight){
            if(launches == 1)
            {
                currentSlowDown = Mathf.Clamp01(currentSlowDown + Time.deltaTime / slowDownTime);
            } else {  
                if(willLaunchOnHit){
                    currentSlowDown = Mathf.Clamp01(currentSlowDown - Time.deltaTime / slowDownTime);
                } else {
                    currentSlowDown = 0;
                } 
            }
        }

        if(Input.GetMouseButtonDown(0)){leftMouseTime = Time.time;}
        if(Input.GetMouseButtonDown(1)){rightMouseTime = Time.time;}

        if((Input.GetMouseButton(0) && leftMouseTime > rightMouseTime) || willLaunchOnHit)
        {

            if(inBossFight) {currentSlowDown = Mathf.Clamp01(currentSlowDown + Time.deltaTime / slowDownTime);}
            launchLine.SetPosition(0, transform.position);
            launchLine.SetPosition(1, GetLaunchPoint());

        if(launches > 0 || inBossFight)
        {
            launchLine.enabled = true;
        }

        } else {
            launchLine.enabled = false;
            if(inBossFight) {
                currentSlowDown = 0;
            }
        }
        

         if ((Input.GetMouseButtonUp(0) && leftMouseTime > rightMouseTime) || willLaunchOnHit) 
        {   
            if(launches > 0 || inBossFight)
            {
                launches --;
                Vector2 finalLaunchPoint = GetLaunchPoint();
                Vector2 position = transform.position;
                rb.velocity = new Vector2((finalLaunchPoint.x - position.x) * 6f, ((finalLaunchPoint.y - position.y)*6f) + 0.1f);
                if(willLaunchOnHit){
                    currentSlowDown = 1;
                }
            }
        }
        

        Time.timeScale = Mathf.Lerp(1, slowMotionSpeed, currentSlowDown);
        postprocessing.weight = currentSlowDown;

    }
    Vector2 GetLaunchPoint()
    {
        Vector2 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        return Vector2.MoveTowards(transform.position, mouseWorldPosition, maxLaunchDistance);
    }
}
}
