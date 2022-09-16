using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyHealthBar : MonoBehaviour
{
    float counter;
    float maxCounter;
    public float startMaxCounter;
    public Image HealthBarEffectImage;
    public Image HealthBarImage;
    float lastHealth;
    float HP;
    public EnemyBehavior Enemy;
    public GameObject healthCounter;
    TextMeshProUGUI healthCounterText;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        healthCounterText = healthCounter.GetComponent<TextMeshProUGUI>();
        maxCounter = startMaxCounter;
        counter = maxCounter;
    }

    private void Update()
    {
        HP = Enemy.Health;

        healthCounterText.text = HP + "/" + Enemy.MaxHealth;
        transform.rotation = Quaternion.identity;
        HealthBarImage.fillAmount += functions.valueMoveTowards(HealthBarImage.fillAmount, HP / Enemy.MaxHealth, 10);
        canvasGroup.alpha = HP < Enemy.MaxHealth ? 1 : 0;

        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            HealthBarEffectImage.fillAmount += functions.valueMoveTowards(HealthBarEffectImage.fillAmount, HP / Enemy.MaxHealth, 20);
        }
        if (Mathf.Abs((HealthBarEffectImage.fillAmount) - (HP / Enemy.MaxHealth)) < 0.004f)
        {
            HealthBarEffectImage.fillAmount = HP / Enemy.MaxHealth;
            maxCounter = startMaxCounter;
        }


        if (lastHealth > HP)
        {
            counter = maxCounter;
            maxCounter *= 0.95f;
        }
        lastHealth = Enemy.Health;
    }
}