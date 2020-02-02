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
    private EntityBase _followingEntity;

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

        Collider2D otherCollider = Physics2D.OverlapPoint(targetPoint);

        if (ignoreRaycast || otherCollider.isTrigger)
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


        if (otherCollider == null)
        {
            RaycastHit2D hit2 = Physics2D.Raycast(targetPoint, -dir, dir.magnitude);
            //Debug.Log($"This: {this.transform} and that: {hit2.transform}");

            if (hit2.transform == this.transform)
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
        else if (otherCollider.transform != transform)
        {
            switch (otherCollider.transform.tag)
            {
                //default:
                case "Wall":
                    canMove = false;
                    bounce = true;
                    break;
                case "Food":
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
                            canMove = false;
                            bounce = true;
                        }
                    }
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
        Vector3 start = transform.position;
        Vector3 pos = start;

        while (pos != target)
        {
            pos = Vector3.MoveTowards(pos, target, moveSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }

        MovingCoroutine = null;

        if (OnMove != null)
        {
            OnMove.Invoke(start, target);
        }
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
        _followingEntity = entity;
        entity.OnMove += OnEntityFollowMove;
    }

    public void StopFollowingEntity()
    {
        if(_followingEntity != null)
            _followingEntity.OnMove -= OnEntityFollowMove;
    }

    private void OnEntityFollowMove(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        Vector3 target = end - direction.normalized;

        bool moved = MoveTo(target, false);
        if(!moved)
        {
            Vector3 distance = target - transform.position;
            if (_followingEntity.tag != "Player")
            {
                
                    StopFollowingEntity();
            }
            //StopFollowingEntity();
        }
    }

    private void OnDestroy()
    {
        StopFollowingEntity();
    }
}
