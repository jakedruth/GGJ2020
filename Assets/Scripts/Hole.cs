using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

    private void FixedUpdate()
    {
        Collider2D otherCollider = Physics2D.OverlapPoint(transform.position);
        if (otherCollider == null)
            return;
        
        EntityBase entity = otherCollider.GetComponent<EntityBase>();
        if (entity == null)
            return;

        if (entity.MovingCoroutine != null)
            return;

        entity.MovingCoroutine = StartCoroutine(AnimateFalling(entity.transform));
    }

    private IEnumerator AnimateFalling(Transform t)
    {
        const float fallTime = 1f;
        float timer = 0;

        float anglesRotate = 720f;
        float angle = 0;




        yield return null;
    }
}
