using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class PlayerController : MonoBehaviour
{
    // Components
    public EntityBase Entity { get; private set; }
    public Transform AimCursor;

    // variables
    public RopeController rope;
    public bool canUseRope;
    public float ropeLength;
    private bool isRopeAnimating;
    
    private EntityBase _lassoedEntity;
    
    private Vector3 _lastInput;

    // Start is called before the first frame update
    void Awake()
    {
        Entity = GetComponent<EntityBase>();
        _lastInput = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            EmoteSystemManager.instance.CreateEmote(transform, "alert");
        }

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

        if (!Input.GetKey(KeyCode.Space) && !isRopeAnimating)
        {
            AimCursor.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.S))
            {
                Entity.TryMoveTo(pos + input);
            }
        }
        else if(canUseRope)
        {
            // show aiming
            if (_lassoedEntity != null)
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

        if (canUseRope && Input.GetKeyUp(KeyCode.Space))
        {
            if (_lassoedEntity == null && !isRopeAnimating)
            {
                isRopeAnimating = true;

                Ray2D ray = new Ray2D(transform.position + direction, direction);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, ropeLength - 1);

                if (hit.transform != null)
                {
                    switch (hit.transform.tag)
                    {
                        default:
                        case "Wall":
                            rope.SendOutRopeAndReturn(hit.point, ()=> { isRopeAnimating = false; });
                            break;
                        case "Food":
                        case "Animal":
                        case "Item":
                            rope.SendOutRopeToEntity(hit.transform.GetComponent<EntityBase>(), onRopeHitEntity);
                            break;
                    }
                }
                else
                {
                    rope.SendOutRopeAndReturn(ray.GetPoint(ropeLength), () => { isRopeAnimating = false; });
                }
            }
            else
            {
                RetractRope();
            }
        }

        if(_lassoedEntity != null)
        {
            Vector3 displacement = _lassoedEntity.transform.position - transform.position;
            if(displacement.sqrMagnitude > ropeLength * ropeLength)
            {
                RetractRope();
            }
        }
    }

    public void onRopeHitEntity(EntityBase entityOther)
    {
        isRopeAnimating = false;
        _lassoedEntity = entityOther;

        if (_lassoedEntity.isPullable)
        {

            if(_lassoedEntity.tag == "Animal")
                EmoteSystemManager.instance.CreateEmote(_lassoedEntity.transform, "alert");
            else
                EmoteSystemManager.instance.CreateEmote(transform, "exclamation");

            _lassoedEntity.FollowEntity(Entity);

            // get direction to entity
            Vector3 displacement = entityOther.transform.position - transform.position;
            Vector3 direction = displacement.normalized;

            entityOther.TryMoveTo(transform.position + direction);
        }
        else
        {
            if (_lassoedEntity.tag == "Animal")
                EmoteSystemManager.instance.CreateEmote(entityOther.transform, "anger");
            else
                EmoteSystemManager.instance.CreateEmote(transform, "");
        }

        rope.AnimateRopeFollowTransform(_lassoedEntity.transform);
    }

    public void RetractRope()
    {
        isRopeAnimating = false;
        _lassoedEntity.StopFollowingEntity();
        _lassoedEntity = null;
        rope.RemoveFollowTarget();
        rope.SendOutRopeToPoint(Vector3.zero);
    }

    public void AnimalOnDestroy(EntityBase animal)
    {
        if (animal == _lassoedEntity)
        {
            RetractRope();
        }
    }

    public void SetCanUseRope(bool value)
    {
        canUseRope = value;
    }
}
