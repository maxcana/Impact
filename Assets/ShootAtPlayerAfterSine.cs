using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerAfterSine : MonoBehaviour
{
    GameObject player;
    MoveWithSine spin;
    Vector2 dir;
    public float force = 50f;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        spin = GetComponent<MoveWithSine>();
        StartCoroutine(shoot(1 + (spin.startingSineX * 1.5f) + Random.Range(-1f, 1f)));
    }
    private void Update()
    {
    }
    private IEnumerator shoot(float delay)
    {
        WaitForSeconds waitASec = new WaitForSeconds(0.2f);
        if (delay > 0)
            yield return new WaitForSeconds(delay);
        for (int i = 0; i < 1000; i++)
        {
            if (Physics2D.Raycast(transform.position, player.transform.position - transform.position).collider.gameObject.tag == "Player")
            {
                Destroy(spin);
                dir = (player.transform.position - transform.position).normalized * force;
                GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
            }
            yield return waitASec;
        }
    }
}
