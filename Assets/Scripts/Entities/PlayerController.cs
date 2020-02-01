using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class PlayerController : MonoBehaviour
{
    // Components
    public EntityBase EntityBase { get; private set; }
    public Transform AimCursor;

    // variables
    public RopeController rope;
    public float ropeLength;
    private bool isUsingRope;
    
    private EntityBase _lassoedEntity;
    
    private Vector3 _lastInput;

    // Start is called before the first frame update
    void Awake()
    {
        EntityBase = GetComponent<EntityBase>();
        _lastInput = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.A))
            input.x -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;
        if (Input.GetKey(KeyCode.S))
            input.y -= 1;
        if (Input.GetKey(KeyCode.W))
            input.y += 1;

        if(input.x != 0 && input.y != 0)
        {
            input.y = 0;
        }

        if(input != Vector3.zero)
        {
            _lastInput = input;
        }

        Vector3 direction = input == Vector3.zero ? _lastInput : input;

        if (!Input.GetKey(KeyCode.Space) && rope.animatingRopeCoroutine == null)
        {
            AimCursor.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.S))
            {
                EntityBase.MoveTo(pos + input);
            }
        }
        else
        {
            // show aiming
            if (isUsingRope)
            {
                AimCursor.gameObject.SetActive(false);
            }
            else
            {
                AimCursor.gameObject.SetActive(true);
                float angle = Vector3.SignedAngle(direction, Vector3.right, Vector3.back);
                AimCursor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isUsingRope)
            {
                Ray2D ray = new Ray2D(transform.position + direction, direction);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, ropeLength - 1);

                Vector3 ropePoint = ray.GetPoint(ropeLength);
                if(hit.transform != null)
                {
                    switch (hit.transform.tag)
                    {
                        default:
                        case "Wall":
                            ropePoint = hit.point;
                            break;
                        case "Animal":
                            ropePoint = hit.transform.position;
                            break;
                    }
                }

                rope.MoveToPoint(ropePoint, OnRopeDoneAnimating);

                //Debug.DrawLine(ray.origin, hitPosition, Color.red, 0.5f);

                //Collider2D other = Physics2D.OverlapPoint(transform.position + direction);
                //if (other != null)
                //{
                //    AnimalController animal = other.transform.GetComponent<AnimalController>();
                //    if (animal != null)
                //    {
                //        _lassoedEntity = animal.EntityBase;
                //        _lassoedEntity.FollowEntity(EntityBase);
                //        rope.SetTarget(_lassoedEntity.transform);
                //        isUsingRope = true;
                //    }
                //}
            }
            else
            {
                isUsingRope = false;
                //rope.RemoveTarget();
                //_lassoedEntity.StopFollowingEntity(EntityBase);
            }
        }
    }

    public void OnRopeDoneAnimating(Transform hit)
    {

    }
}
