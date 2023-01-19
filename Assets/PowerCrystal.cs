using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCrystal : MonoBehaviour
{
    public GameObject collectParticles;
    public GameObject[] otherParticles;
    public static bool playerHasCrystal = false;
    private void Start()
    {
        playerHasCrystal = false;
    }
    private void Update()
    {
        if (playerHasCrystal)
        {
            collectParticles.transform.localRotation = Quaternion.Euler(new Vector3(0, collectParticles.transform.eulerAngles.y + 150 * Time.deltaTime, 0));
        }
        else transform.Rotate(new Vector3(50 * Time.deltaTime, 0, 0));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject[] deadlies = GameObject.FindGameObjectsWithTag("Deadly");
            foreach(GameObject deadly in deadlies){
                Destroy(deadly);
            }
            playerHasCrystal = true;
            collectParticles.SetActive(true);
            collectParticles.transform.parent = null;

            foreach (GameObject particles in otherParticles)
            {
                particles.SetActive(false);
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 3f);
        }
    }
}
