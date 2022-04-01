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
         if(currentBoss == enums.Bosses.Hex){
            MaxHealth = Hex.MaxHealth; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    MaxHealth = EnemyBoss.MaxHealth; }}

        if(currentBoss == enums.Bosses.Hex){
            Health = Hex.Health; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    Health = EnemyBoss.Health; }}

        counter -= Time.deltaTime;
        if(counter <= 0){
            HealthBarImage.fillAmount += functions.valueMoveTowards(HealthBarImage.fillAmount, Health / MaxHealth, 10);
        }
        //if(HealthBarImage.fillAmount)
        if(Mathf.Abs((HealthBarImage.fillAmount) - (Health / MaxHealth)) < 0.004f){
            HealthBarImage.fillAmount = Health / MaxHealth;
            maxCounter = startMaxCounter;
        }
        if(HealthBarImage.fillAmount > 0.997f){
            HealthBarImage.fillAmount = 1;
        }

        if(lastHealth > Health){
            counter = maxCounter;
            maxCounter *= 0.95f;
        }

        lastHealth = Health;
    }
}
