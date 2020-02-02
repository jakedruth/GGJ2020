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
    public bool IsFollowing { get { return _followingEntity != null; } }
    
    private EntityBase _beingFollowedBy;
    public bool IsBeingFollowed { get { return _beingFollowedBy != null; } }

    public Coroutine MovingCoroutine { get; set; }

    public UnityAction<Vector3, Vector3> OnMove;

    private void Awake()
    {
        _bounds = GetComponent<BoxCollider2D>();
    }

    public bool TryMoveTo(Vector3 targetPoint)
    {
        // Check if moving
        if (MovingCoroutine != null)
            return false;

        //if (ignoreRaycast)
        //{
        //    if (OnMove != null)
        //        OnMove.Invoke(transform.position, targetPoint);

        //    MovingCoroutine = StartCoroutine(MoveToPosition(targetPoint));
        //    return true;
        //}

        // Check to see if there is an open spot
        Vector3 dir = targetPoint - transform.position;
        bool canMove = false;
        bool bounce = false;

        Collider2D otherCollider = Physics2D.OverlapPoint(targetPoint);

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
        else if (otherCollider.isTrigger)
        {
            canMove = true;
            bounce = false;
        }
        else if (otherCollider.transform != transform)
        {
            EntityBase other = otherCollider.transform.GetComponent<EntityBase>();
            switch (otherCollider.transform.tag)
            {
                //default:
                case "Wall":
                    canMove = false;
                    bounce = true;
                    SoundManager.instance.Play("False");
                    break;
                case "Food":
                case "Animal":
                    if (other == null || other.MovingCoroutine != null)
                    {
                        canMove = false;
                        bounce = true;
                    }
                    else
                    {
                        if (other.isPushable)
                        {
                            if (other.TryPush(dir))
                            {
                                canMove = true;
                                bounce = false;
                            }
                            else
                            {
                                if (other.tag == "Animal")
                                {
                                    SoundManager.instance.Play("False");
                                    EmoteSystemManager.instance.CreateEmote(other.transform, "faceAngry");
                                }
                                canMove = false;
                                bounce = true;
                            }
                        }
                        else
                        {
                            SoundManager.instance.Play("False");
                            EmoteSystemManager.instance.CreateEmote(other.transform, "anger");
                            canMove = false;
                            bounce = true;
                        }
                    }
                    break;
                case "Item":
                    
                    if (other == null || other.MovingCoroutine != null)
                    {
                        canMove = false;
                        bounce = true;
                        break;
                    }

                    ItemController item = other.GetComponent<ItemController>();

                    // if you are the player, pick it up
                    if (tag == "Player")
                    {
                        if(item.TryPickUp())
                        {
                            canMove = true;
                            bounce = false;
                            SoundManager.instance.Play("Pickup");
                            EmoteSystemManager.instance.CreateEmote(transform, "star");
                            break;
                        }
                    }

                    if (other.isPushable)
                    {
                        if (other.TryPush(dir))
                        {
                            canMove = true;
                            bounce = false;
                        }
                        else
                        {
                            canMove = false;
                            bounce = true;
                        }
                    }
                    else
                    {
                        //EmoteSystemManager.instance.CreateEmote(other.transform, "anger");
                        canMove = false;
                        bounce = true;
                    }



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

    public bool TryPush(Vector3 direction)
    {
        return TryMoveTo(transform.position + direction.normalized);
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
        if (_followingEntity != null)
            StopFollowingEntity();

        _followingEntity = entity;
        _followingEntity._beingFollowedBy = this;

        entity.OnMove += OnEntityFollowMove;
    }

    public void StopFollowingEntity()
    {
        if (_followingEntity != null)
        {
            _followingEntity.OnMove -= OnEntityFollowMove;
            _followingEntity._beingFollowedBy = null;
        }

        _followingEntity = null;
    }

    private void OnEntityFollowMove(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        Vector3 target = end - direction.normalized;

        bool moved = TryMoveTo(target);
        if(!moved)
        {
            if (_followingEntity.tag != "Player")
            {
                StopFollowingEntity();
            }
        }
    }

    private void OnDestroy()
    {
        StopFollowingEntity();
    }
}
