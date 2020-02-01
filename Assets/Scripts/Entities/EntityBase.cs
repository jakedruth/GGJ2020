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
    public bool isPushable;
    public bool isPullable;

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

        Collider2D otherCollider = Physics2D.OverlapPoint(transform.position + dir);

        if (otherCollider == null)
        {
            canMove = true;
            bounce = false;
        }
        else if (otherCollider.transform != transform)
        {
            switch (otherCollider.transform.tag)
            {
                case "Wall":
                    canMove = false;
                    bounce = true;
                    break;
                case "Animal":
                    EntityBase other = otherCollider.transform.GetComponent<EntityBase>();
                    if (other != null && other.MovingCoroutine != null)
                    {
                        canMove = true;
                        bounce = false;
                    }
                    else
                    {
                        if (other.isPushable)
                        {
                            if (other.Push(dir))
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
                            Debug.Log($"Here");
                            canMove = false;
                            bounce = true;
                        }
                    }
                    break;
                case "Food":
                    break;
                case "Item":
                    break;
            }
        }
        else
        {
            canMove = true;
            bounce = false;
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

    public bool Push(Vector3 direction)
    {
        return MoveTo(transform.position + direction.normalized);
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

    public void FollowEntity(EntityBase entity)
    {
        entity.OnMove += OnEntityFollowMove;
    }

    public void StopFollowingEntity(EntityBase entity)
    {
        entity.OnMove -= OnEntityFollowMove;
    }

    private void OnEntityFollowMove(Vector3 start, Vector3 end)
    {
        MoveTo(start, true);
    }
}
