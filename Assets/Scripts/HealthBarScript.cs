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
    private void Start()
    {
        HealthBarImage = GetComponent<Image>(); 
    }
    private void Update()
    {
        if(currentBoss == enums.Bosses.Hex){
            MaxHealth = Hex.MaxHealth; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    MaxHealth = EnemyBoss.MaxHealth; }}

        if(currentBoss == enums.Bosses.Hex){
            Health = Hex.Health; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    Health = EnemyBoss.Health; }}

        HealthBarImage.fillAmount += functions.valueMoveTowards(HealthBarImage.fillAmount, Health / MaxHealth, 10);
        
    }
}
    