using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    GameObject explosionObject;
    [SerializeField] ParticleSystem explosion;
    private void Awake()
    {
        explosionObject = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            if (Mathf.Abs(other.attachedRigidbody.velocity.magnitude) > 0.2f || Mathf.Abs(other.attachedRigidbody.angularVelocity) > 0.2f)
            {
                Explode();
            }
        }
    }
    public void Explode()
    {
        ParticleSystem particleClone = GameObject.Instantiate(explosion, transform.position, Quaternion.Euler(90, 0, 0));
        GameAssets.i.Shake.Shake(0.1f, 15);
        particleClone.Play();

        explosionObject.SetActive(true);
        Destroy(gameObject);
        Destroy(transform.parent.gameObject, 0.2f);
    }
}
