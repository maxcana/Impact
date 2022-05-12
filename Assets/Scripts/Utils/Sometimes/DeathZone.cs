using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeathZone : MonoBehaviour
{
    public float TimeUntilDeath;
    public string[] killTags;
    public string[] ignoreTags;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (killTags != null)
        {
            if (killTags.Length > 0)
            {
                if (!ignoreTags.Contains(other.tag) || ignoreTags == null)
                {
                    if (!killTags.Contains(other.tag))
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
                else { return; }
            }
        }

        if (other.tag == "Explosive" || other.tag == "ExplosiveCollider")
        {
            if(other.tag == "ExplosiveCollider")
            {
                StartCoroutine(other.GetComponent<Explosive>().Explode());
            } else {
                StartCoroutine(other.transform.GetChild(0).GetComponent<Explosive>().Explode());
            }
        }
        else
        {
            if (other.tag != "Undestroyable")
            {
                if (other.tag == "Enemy")
                {
                    EnemyBehavior e = other.GetComponent<EnemyBehavior>();
                    e.DealDamages(e.Health, false, false, 0);
                }
                else
                {
                    Destroy(other.gameObject, TimeUntilDeath);
                }

            }
        }
    }
}
