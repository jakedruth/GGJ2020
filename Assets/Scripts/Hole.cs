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

        if (entity.tag == "Animal" || entity.tag == "Player")
            EmoteSystemManager.instance.CreateEmote(entity.transform, "faceSad");

        entity.MovingCoroutine = StartCoroutine(AnimateFalling(entity));
    }

    private IEnumerator AnimateFalling(EntityBase entity)
    {
        Transform t = entity.transform;

        const float fallTime = 1f;
        float timer = 0;

        const float anglesRotate = 720f;

        while (timer < fallTime)
        {
            timer += Time.deltaTime;
            float percent = Mathf.Clamp01(timer / fallTime);

            float angle = Mathf.Lerp(0, anglesRotate, percent);
            Vector3 scale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);

            t.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            t.localScale = scale;

            yield return null;
        }

        entity.MovingCoroutine = null;
        Destroy(entity.gameObject);
    }
}
