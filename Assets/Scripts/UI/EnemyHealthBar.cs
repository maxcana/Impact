using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    public Image HealthBarImage;
    float CurrentHealth; 
    float MaxHealth;
    public EnemyBehavior Enemy;
    CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        MaxHealth = Enemy.MaxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
        CurrentHealth = Enemy.Health;
        HealthBarImage.fillAmount = CurrentHealth / MaxHealth;
        canvasGroup.alpha = CurrentHealth<MaxHealth?1:0;
    }
}