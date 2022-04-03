using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cursorScript : MonoBehaviour
{
    SpriteRenderer sr;
    float var;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var += Time.unscaledDeltaTime;
        transform.localScale = new Vector2(0.03f + Mathf.Sin(var * 15)/200, 0.03f + Mathf.Sin(var * 15)/200);
        if(sr.enabled == true){
            Cursor.visible = false;
        } else if (sr.enabled == false){
            Cursor.visible = true;
        }
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos;
    }
}
