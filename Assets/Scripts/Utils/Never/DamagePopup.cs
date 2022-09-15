using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] AnimationCurve fontSizeCurve;
    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCritical, float totalmass, bool isHyperCritical)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.DamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCritical, totalmass, isHyperCritical);

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
    private float damageAmount;
    private float textLerp;
    bool isHyperCritical;
    bool isCritical;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, bool isCritical, float totalmass, bool isHyperCritical = false)
    {
        this.damageAmount = damageAmount;
        this.isCritical = isCritical;
        this.isHyperCritical = isHyperCritical;
        if (damageAmount == 0) { Destroy(gameObject); }
        startSize = fontSizeCurve.Evaluate(damageAmount);
        moveXspeed = UnityEngine.Random.Range(-1f, 1f);
        textMesh.fontSize = startSize;

        if (damageAmount > 250 && totalmass > 20)
        {
            GameAssets.i.Shake.Shake(Mathf.Clamp((damageAmount / 3000f) + totalmass / 3000, 0.1f, 0.5f), Mathf.Clamp((damageAmount / 300f) + totalmass / 600, 0.5f, 6));
        }

        if (damageAmount >= 0)
        {
            if (damageAmount >= 1000)
            {
                textColor = new Color(1, 1, 0);
            }
            else
            {
                if (damageAmount < 100)
                {
                    textColor = new Color(1, 0.005f, 0.005f);
                    textMesh.text = isHyperCritical ? damageAmount.ToString() + "!!" : isCritical ? damageAmount.ToString() + "!" : damageAmount.ToString();
                }
                else
                {
                    textColor = new Color(1, 0.005f, 0.005f);
                }
            }
        }
        else
        {

            textColor = new Color(0.005f, 1, 0.005f);

            textMesh.text = isHyperCritical ? (damageAmount * -1f).ToString() + "!!" : isCritical ? (damageAmount * -1f).ToString() + "!" : (damageAmount * -1f).ToString();
        }


        textLerp = 0;
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        if (damageAmount >= 1000) { sortingOrder *= 100; }
        textMesh.sortingOrder = sortingOrder;
        moveYSpeed = 5f;
    }
    private void Update()
    {
        if (damageAmount >= 100)
        {
            textLerp += functions.valueMoveTowards(textLerp, damageAmount, 25);
            if (isHyperCritical)
            {
                textMesh.text = Mathf.Round(textLerp) + "!!";
            }
            else if (isCritical)
            {
                textMesh.text = Mathf.Round(textLerp) + "!";
            }
            else
            {
                textMesh.text = Mathf.Round(textLerp).ToString();
            }
        }
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
