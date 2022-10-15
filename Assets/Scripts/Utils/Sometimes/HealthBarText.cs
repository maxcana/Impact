using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] enums.Bosses currentBoss;
    private float defaultTextSize;
    private float maxTextSize;
    [SerializeField] BigEnemyGuy enemyguy;
    [SerializeField] BossScript Hex;
    [SerializeField] Tri tri;
    [SerializeField] Oct oct;

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

        if(currentBoss == enums.Bosses.Hex){ textMesh.SetText(((long)Hex.Health).ToString()); }
        if(currentBoss == enums.Bosses.BigEnemy){ textMesh.SetText(((long)enemyguy.Health).ToString());}
        if(currentBoss == enums.Bosses.Tri){textMesh.SetText(((long)(tri.Health)).ToString());}
        if(currentBoss == enums.Bosses.Oct){textMesh.SetText(((long)(oct.Health)).ToString());}
        
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
