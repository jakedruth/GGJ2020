using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EntityBase : MonoBehaviour
{
    private BoxCollider2D _bounds;

    // Variables
    public float moveSpeed = 15;
    private Coroutine moveingCoroutine;

    private void Awake()
    {
        _bounds = GetComponent<BoxCollider2D>();
    }

    public bool MoveTo(Vector3 targetPoint)
    {
        // Check if moving
        if (moveingCoroutine == null)
        {
            // Check to see if there is an open spot
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, targetPoint - transform.position, 1);
            if (hits.Length == 1 && hits[0].transform == transform)
            {
                moveingCoroutine = StartCoroutine(MoveToPosition(targetPoint));
                return true;
            } 
            else
            {
                // Hit something(s)!
                foreach(RaycastHit2D hit in hits)
                {
                    // skip the player collider
                    if (hit.transform == transform)
                        continue;

                    // Handle what hit?
                    if (hit.transform.tag == "Wall")
                    {
                        moveingCoroutine = StartCoroutine(BounceOfPosition(targetPoint, 0.5f));
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        Vector3 pos = transform.position;

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, moveSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }

        moveingCoroutine = null;
    }

    private IEnumerator BounceOfPosition(Vector3 target, float distancePercentage)
    {
        Vector3 start = transform.position;
        Vector3 pos = start;

        target = Vector3.Lerp(start, target, distancePercentage);

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, moveSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }

        while (pos != start)
        {
            pos = Vector3.MoveTowards(pos, start, moveSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }

        moveingCoroutine = null;
    }
}
