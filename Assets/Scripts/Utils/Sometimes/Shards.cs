using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shards : MonoBehaviour
{
    [SerializeField] PolygonCollider2D[] shards;
    public void Disperse(Vector2 position, float power = 4)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        float i = 0;
        foreach (var shard in shards)
        {
            shard.transform.parent = transform.parent;
            shard.gameObject.SetActive(true);
            shard.attachedRigidbody.velocity = power * ((Vector2)shard.bounds.center - position);
            Fade(1f + ((++i) / 12), shard);
        }
    }
    public async void Fade(float delay, PolygonCollider2D collider)
    {
        await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(delay * 1000));
        var sr = collider.gameObject.GetComponent<SpriteRenderer>();
        while (sr.color.a > 0.02f)
        {
            sr.color += new Color(0,0,0,functions.valueMoveTowards(sr.color.a, 0, 20));
            await System.Threading.Tasks.Task.Delay(1);
        }
        Destroy(collider.gameObject);
    }
}
