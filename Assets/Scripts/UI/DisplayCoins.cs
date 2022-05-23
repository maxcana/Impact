using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayCoins : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    RectTransform rt;
    public bool countLaunches = false;
    public bool isCoinUICanvas;
    Ball ball;
    void Start()
    {
        if(countLaunches)
            ball = GameObject.FindWithTag("Player").GetComponent<Ball>();
        rt = GetComponent<RectTransform>();
        if (isCoinUICanvas)
        {
            textMesh = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = countLaunches ? ball.launches.ToString() : data.coins.ToString();
        if (isCoinUICanvas)
            rt.anchoredPosition = new Vector2((rt.sizeDelta.x * 0.5f) + 30, (rt.sizeDelta.y * -0.5f) - 30);
    }
}
