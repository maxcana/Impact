using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    public GameObject deathcircle;
    public float delay = 0;
    public float radius = 5;
    public float objects = 18;
    public float launchForce = 20;
    private void Start()
    {
        StartCoroutine(CreateCircleOfDoom());
    }
    public IEnumerator CreateCircleOfDoom()
    {
        if (delay >= 1)
        {
            yield return new WaitForSeconds(delay - 1);
            functions.SpawnCircle(transform.position, 1);
            yield return new WaitForSeconds(1);
        } else yield return new WaitForSeconds(delay);
        for (int i = 1; i <= objects; i++)
        {
            GameObject fireworkParticle = Instantiate(deathcircle, transform.position + new Vector3(Mathf.Cos((i * 360f / (float)objects) * Mathf.Deg2Rad) * radius, Mathf.Sin((i * 360f / (float)objects) * Mathf.Deg2Rad) * radius), Quaternion.identity);
            fireworkParticle.GetComponent<Rigidbody2D>().AddForce((fireworkParticle.transform.position - transform.position).normalized * launchForce, ForceMode2D.Impulse);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
