using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    GameObject ball;
    private void Awake()
    {
        ball = GameObject.FindWithTag("Player");
    }
       private void Update() {
        
        if(ball == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
