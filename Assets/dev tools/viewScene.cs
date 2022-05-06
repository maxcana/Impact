using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class viewScene : MonoBehaviour
{
    Vector3 camPosition;
    GameObject ballCam;
    float size;
    private void Start() {
        size = 4;
        camPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10);
        ballCam = GameObject.FindWithTag("BallCamera");
    }
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Z)){
            ballCam.SetActive(false);
        }
        if(Input.GetKey(KeyCode.Z)){
            Time.timeScale = 0f;
            
            camPosition += new Vector3(Input.GetKey(KeyCode.RightArrow) ? 1 * Time.unscaledDeltaTime * 50 : Input.GetKey(KeyCode.LeftArrow) ? -1 * Time.unscaledDeltaTime * 50 : 0, Input.GetKey(KeyCode.UpArrow) ? 1 * Time.unscaledDeltaTime * 50 : Input.GetKey(KeyCode.DownArrow) ? -1 * Time.unscaledDeltaTime * 50 : 0, 0);
            size += Input.GetKey(KeyCode.X) ? 1 * Time.unscaledDeltaTime * 10 : Input.GetKey(KeyCode.C) ? -1 * Time.unscaledDeltaTime * 10 : 0;
            Camera.main.orthographicSize = size;
            Camera.main.transform.position = camPosition;
        }
        if(Input.GetKeyUp(KeyCode.Z)){
            ballCam.SetActive(true);
        }
    }
}
