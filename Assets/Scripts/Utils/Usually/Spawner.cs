using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    int counter = 0;
    [SerializeField] Vector2 velocity;
    [SerializeField] bool forceVelocity;
    [SerializeField] bool scaleWithEnemyLordHealth;
    public int timeToSpawn;
    public bool randomizeTimeToSpawn = false;
    public int maxTimeToSpawn;
    int initialTimeToSpawn;
    [SerializeField] BigEnemyGuy beg;

    // Start is called before the first frame update
    void Start()
    {
        if(randomizeTimeToSpawn)
            timeToSpawn = UnityEngine.Random.Range(timeToSpawn, maxTimeToSpawn + 1);
        initialTimeToSpawn = timeToSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(scaleWithEnemyLordHealth){
            if (BigEnemyGuy.isDonePart2Intro){
                timeToSpawn = Mathf.CeilToInt(initialTimeToSpawn - (beg.MaxHealth - beg.Health) / 30);
            }
        }

        counter += 1;
        if(counter >= timeToSpawn)
        {
            GameObject myObject = Instantiate(objectToSpawn, transform.position, transform.rotation); 
            if(forceVelocity){
                myObject.GetComponent<Rigidbody2D>().velocity = velocity;
            }
            counter = 0;
        }
    }
}
