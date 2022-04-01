using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] AnimationCurve fontSizeCurve;
    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCritical, float totalmass)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.DamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCritical, totalmass);

        return damagePopup;
    }

    private static int sortingOrder;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    float startSize;
    private float moveXspeed;
    private float moveYSpeed;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, bool isCritical, float totalmass)
    {
        if(damageAmount == 0){textMesh.enabled = false;}
        startSize = fontSizeCurve.Evaluate(damageAmount);
        moveXspeed = UnityEngine.Random.Range(-1f, 1f);
        textMesh.fontSize = startSize;

        if(damageAmount > 250 && totalmass > 20){
            GameAssets.i.Shake.Shake(Mathf.Clamp((damageAmount / 1000f) + totalmass/1000, 0.5f, 0.8f), Mathf.Clamp((damageAmount / 150f) + totalmass/300, 1f, 10));
        }

        if (damageAmount >= 0)
        {
            textColor = new Color(255, 0, 0);

            textMesh.SetText(damageAmount.ToString());
        }
        else
        {

            textColor = new Color(0, 255, 0);

            textMesh.SetText((damageAmount * -1f).ToString());
        }

        if(damageAmount >= 1000){textColor = new Color(256, 196, 0);}


        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        if(damageAmount >= 1000) {sortingOrder += 10000;}
        textMesh.sortingOrder = sortingOrder;
        moveYSpeed = 5f;
    }
    private void Update()
    {

        transform.position += new Vector3(moveXspeed, moveYSpeed) * Time.deltaTime;
        //? moves the text down at a fixed rate to simulate gravity
        moveYSpeed -= 8 * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }

        textMesh.fontSize += ((startSize * 1.6f - textMesh.fontSize) / 11) * 60 * Time.deltaTime;
    }

}
