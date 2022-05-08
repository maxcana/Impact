using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cursorScript : MonoBehaviour
{
    RectTransform t;
    Image image;
    float var;
    void Start()
    {
        t = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var += Time.unscaledDeltaTime;
        t.localScale = new Vector2(1f + Mathf.Sin(var * 8)/5, 1f + Mathf.Sin(var * 8)/5);
        if(image.enabled == true){
            Cursor.visible = false;
        } else if (image.enabled == false){
            Cursor.visible = true;
        }
        Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        t.position = pos;
    }
}
