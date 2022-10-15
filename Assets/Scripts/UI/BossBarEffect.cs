using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarEffect : MonoBehaviour
{
    //! 0000000000000
    //! 000 0000 0000
    //! 0000000000000
    //! 0000000000000
    //! 0000000000000
    Image HealthBarImage;
    [SerializeField] enums.Bosses currentBoss;
    [SerializeField] BigEnemyGuy EnemyBoss;
    [SerializeField] BossScript Hex;
    [SerializeField] Tri tri;
    [SerializeField] Oct oct;
    private float MaxHealth;
    float lastHealth;
    float Health;
    float counter;
    [SerializeField] float startMaxCounter;
    float maxCounter;
    private void Start()
    {
        maxCounter = startMaxCounter;
        counter = maxCounter;
        HealthBarImage = GetComponent<Image>();
    }
    void Update()
    {
        Health = currentBoss == enums.Bosses.Hex ? Hex.Health : currentBoss == enums.Bosses.BigEnemy ? EnemyBoss.Health : currentBoss == enums.Bosses.Tri ? tri.Health : currentBoss == enums.Bosses.Oct ? oct.Health : 6969696969696969;
        MaxHealth = currentBoss == enums.Bosses.Hex ? Hex.MaxHealth : currentBoss == enums.Bosses.BigEnemy ? EnemyBoss.MaxHealth : currentBoss == enums.Bosses.Tri ? tri.MaxHealth : currentBoss == enums.Bosses.Oct ? oct.MaxHealth : 6969696969696969;

        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            HealthBarImage.fillAmount += functions.valueMoveTowards(HealthBarImage.fillAmount, Health / MaxHealth, 10);
        }
        if (Mathf.Abs((HealthBarImage.fillAmount) - (Health / MaxHealth)) < 0.004f)
        {
            HealthBarImage.fillAmount = Health / MaxHealth;
            maxCounter = startMaxCounter;
        }

        if (lastHealth > Health)
        {
            counter = maxCounter;
            maxCounter *= 0.95f;
        }

        lastHealth = Health;
    }
}
