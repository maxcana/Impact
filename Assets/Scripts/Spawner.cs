using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    int counter = 0;
    [SerializeField] bool scaleWithEnemyLordHealth;
    public float timeToSpawn;
    float initialTimeToSpawn;
    [SerializeField] BigEnemyGuy beg;

    // Start is called before the first frame update
    void Start()
    {
        print("You BETTER not forget about me");
        print("You BETTER not forget about me");
        initialTimeToSpawn = timeToSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BigEnemyGuy.isDonePart2Intro){
            timeToSpawn = initialTimeToSpawn - (beg.MaxHealth - beg.Health) / 30;
        }

        counter += 1;
        if(counter >= timeToSpawn)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation); 
            counter = 0;
        }
    }
}
