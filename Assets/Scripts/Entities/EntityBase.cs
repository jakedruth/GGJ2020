using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(BoxCollider2D))]
public class EntityBase : MonoBehaviour
{
    private BoxCollider2D _bounds;

    // Variables
    public float moveSpeed = 15;
    public Coroutine MovingCoroutine { get; private set; }

    public UnityAction<Vector3, Vector3> OnMove;

    private void Awake()
    {
        _bounds = GetComponent<BoxCollider2D>();
    }

    public bool MoveTo(Vector3 targetPoint, bool ignoreRaycast = false)
    {
        // Check if moving
        if (MovingCoroutine != null)
            return false;

        if(ignoreRaycast)
        {
            if (OnMove != null)
                OnMove.Invoke(transform.position, targetPoint);

            MovingCoroutine = StartCoroutine(MoveToPosition(targetPoint));
            return true;
        }

        // Check to see if there is an open spot
        Vector3 dir = targetPoint - transform.position;
        bool canMove = false;
        bool bounce = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 1);
        if (hits.Length == 1 && hits[0].transform == transform)
        {
            canMove = true;
            bounce = false;
        }
        else
        {
            // Hit something(s)!
            if(hits.Length > 2)
            {
                Debug.LogError("This is to many things");
            }

            foreach (RaycastHit2D hit in hits)
            {
                // skip the this collider
                if (hit.transform == transform)
                    continue;

                EntityBase other = hit.transform.GetComponent<EntityBase>();

                if (other != null && other.MovingCoroutine != null)
                {
                    canMove = true;
                    bounce = false;
                    continue;
                }

                // Handle what hit?
                if (hit.transform.tag == "Wall")
                {
                    canMove = false;
                    bounce = true;
                }

                if (hit.transform.tag == "Animal")
                {
                    AnimalController animalController = hit.transform.GetComponent<AnimalController>();
                    if (animalController != null)
                    {
                        if (animalController.isMovable)
                        {
                            if (animalController.MoveInDirection(dir))
                            {
                                canMove = true;
                                bounce = false;
                            }
                            else
                            {
                                canMove = false;
                                bounce = false;
                            }
                        }
                        else
                        {
                            canMove = false;
                            bounce = true;
                        }
                    }
                }
            }
        }

        if (canMove)
        {
            if (OnMove != null)
                OnMove.Invoke(transform.position, targetPoint);

            MovingCoroutine = StartCoroutine(MoveToPosition(targetPoint));            
        }
        else if (bounce)
        {
            MovingCoroutine = StartCoroutine(BounceOffPosition(targetPoint, 0.5f));
        }

        return canMove;
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

        MovingCoroutine = null;
    }

    private IEnumerator BounceOffPosition(Vector3 target, float distancePercentage)
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

        MovingCoroutine = null;
    }
}
