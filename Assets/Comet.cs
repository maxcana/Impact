using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    SpriteRenderer sr;
    Vector2 op;
    DeathZone dz;
    [SerializeField] GameObject crashParticles;
    private void OnEnable()
    {
        dz = GetComponent<DeathZone>();
        op = transform.position;
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector2(10, 10);

        transform.LeanScale(new Vector3(4.5f, 4.5f, 1), 1).setEaseInQuint();
    }
    bool oldDz = false;
    private void Update()
    {  
        if (transform.localScale.x > 4.7f)
        {
            sr.color = new Color(1, 19f/255f, 19f/255f, sr.color.a + (1f - sr.color.a) * Time.deltaTime * 0.3f);
            transform.position = op + new Vector2(Random.Range(-0.1f * transform.localScale.x, 0.1f * transform.localScale.x), Random.Range(-0.1f * transform.localScale.y, 0.1f * transform.localScale.y));
        }
        else
        {
            sr.color = new Color(1, 19f/255f, 19f/255f, 1);
            dz.enabled = true;
            if (dz.enabled != oldDz)
            {
                ParticleSystem particles = Instantiate(crashParticles, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<ParticleSystem>();
                particles.Play();
            }
        }
        oldDz = dz.enabled;
    }
}
