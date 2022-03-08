using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarScript : MonoBehaviour
{
    private Image HealthBarImage;
    public float CurrentHealth; 
    private float MaxHealth = BossScript.MaxHealth;
    BossScript Boss;
    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
        Boss = FindObjectOfType<BossScript>();
    }
    private void Update()
    {
        MaxHealth = BossScript.MaxHealth;
        CurrentHealth = BossScript.Health; 
        HealthBarImage.fillAmount += Time.deltaTime * 10 *((CurrentHealth / MaxHealth) - HealthBarImage.fillAmount);
    }
}