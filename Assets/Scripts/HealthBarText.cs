using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private float defaultTextSize;
    private float maxTextSize;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        defaultTextSize = textMesh.fontSize;
        maxTextSize = defaultTextSize + 10;
        
    }

    void Update()
    {
        string preHealth;
        preHealth = textMesh.text;
        textMesh.SetText(BossScript.Health.ToString());
        if(preHealth != textMesh.text){
            textMesh.fontSize += 3;
            if(maxTextSize < textMesh.fontSize){
                textMesh.fontSize = maxTextSize;
            }
        }
    }

    private void FixedUpdate() {
        textMesh.fontSize += (defaultTextSize - textMesh.fontSize)/10;
    }
}
