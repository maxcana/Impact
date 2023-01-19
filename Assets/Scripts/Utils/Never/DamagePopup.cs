using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] AnimationCurve fontSizeCurve;
    public static DamagePopup Create(UnityEngine.Vector3 position, float damageAmount, bool isCritical, float totalmass, bool isHyperCritical, float lerpSpeed = 25f)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.DamagePopup, position, UnityEngine.Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCritical, totalmass, isHyperCritical, lerpSpeed);

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
    float lerpSpeed = 25;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, bool isCritical, float totalmass, bool isHyperCritical = false, float lerpSpeed = 25f)
    {
        this.lerpSpeed = lerpSpeed;
        this.damageAmount = damageAmount;
        this.isCritical = isCritical;
        this.isHyperCritical = isHyperCritical;
        if (damageAmount == 0) { Destroy(gameObject); }
        startSize = fontSizeCurve.Evaluate(damageAmount);
        moveXspeed = UnityEngine.Random.Range(-1f, 1f);
        textMesh.fontSize = startSize;

        if (damageAmount > 250 && totalmass > 20)
        {
            GameAssets.i.Shake.Shake(Mathf.Clamp((damageAmount / 3000f) + totalmass / 3000f, 0.1f, 0.5f), Mathf.Clamp((damageAmount / 300f) + totalmass / 600, 0.5f, 6));
        }
        BigInteger bigIntDamage = new BigInteger(damageAmount);

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
                    textMesh.text = isHyperCritical ? bigIntDamage.ToString() + "!!" : isCritical ? bigIntDamage.ToString() + "!" : bigIntDamage.ToString();
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

            textMesh.text = isHyperCritical ? (bigIntDamage * -1).ToString() + "!!" : isCritical ? (bigIntDamage * -1).ToString() + "!" : (bigIntDamage * -1).ToString();
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
            textLerp += functions.valueMoveTowards(textLerp, damageAmount, lerpSpeed);
            if (isHyperCritical)
            {
                textMesh.text = ((long)Mathf.Round(textLerp)) + "!!";
            }
            else if (isCritical)
            {
                textMesh.text = ((long)Mathf.Round(textLerp)) + "!";
            }
            else
            {
                textMesh.text = ((long)Mathf.Round(textLerp)).ToString();
            }
        }
        transform.position += new UnityEngine.Vector3(moveXspeed, moveYSpeed) * Time.deltaTime;
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
