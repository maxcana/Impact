using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelTitle : MonoBehaviour
{
    TMP_Text tmp;
    float timer;
    void Start()
    {
        timer = 0;
        tmp = GetComponent<TMP_Text>();
        tmp.characterSpacing = 20;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(functions.IsBetweenf(0, 2, timer)){
            tmp.color += new Color(0, 0, 0, functions.valueMoveTowards(tmp.color.a, 1, 1.5f));
            tmp.characterSpacing += functions.valueMoveTowards(tmp.characterSpacing, 0, 1.5f);
        }
        if(functions.IsBetweenf(3, 5, timer)){
            tmp.color += new Color(0, 0, 0, functions.valueMoveTowards(tmp.color.a, 0, 6));
            tmp.characterSpacing += Time.deltaTime * 40;
        }
        if(timer > 5){
            Destroy(gameObject);
        }
    }
}
