using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarScript : MonoBehaviour
{
    Image HealthBarImage;
    [SerializeField] enums.Bosses currentBoss;
    private float MaxHealth;
    float Health;
    BossScript Boss;
    [SerializeField] BigEnemyGuy EnemyBoss;
    [SerializeField] BossScript Hex;
    [SerializeField] Tri tri;
    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
    }
    private void Update()
    {
        if (currentBoss == enums.Bosses.Hex)
        {
            MaxHealth = Hex.MaxHealth;
            Health = Hex.Health;
        }
        else
        {
            if (currentBoss == enums.Bosses.BigEnemy)
            {
                MaxHealth = EnemyBoss.MaxHealth;
                Health = EnemyBoss.Health;
            }
            else
            {
                if (currentBoss == enums.Bosses.Tri)
                {
                    MaxHealth = tri.MaxHealth;
                    Health = tri.Health;
                }
            }
        }

        HealthBarImage.fillAmount += functions.valueMoveTowards(HealthBarImage.fillAmount, Health / MaxHealth, 10);

        if (Mathf.Abs((HealthBarImage.fillAmount) - (Health / MaxHealth)) < 0.004f)
        {
            HealthBarImage.fillAmount = Health / MaxHealth;
        }
    }
}
