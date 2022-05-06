using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKeepTransformAligned : MonoBehaviour
{
    public Collider2D referenceCollider;
    public Vector2 offset;

    private void LateUpdate() {
        float topy = referenceCollider.ClosestPoint(new Vector2(referenceCollider.transform.position.x, referenceCollider.transform.position.y + 1000)).y;
        transform.position = new Vector2(referenceCollider.transform.position.x, topy) + offset;
        //transform.localPosition = new Vector2(0, transform.localPosition.y);
    }
}
