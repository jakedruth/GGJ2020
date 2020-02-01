using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class AnimalController : MonoBehaviour
{
    // Components
    private EntityBase _base;
    public bool isMovable;

    // Start is called before the first frame update
    void Awake()
    {
        _base = GetComponent<EntityBase>();
    }

    public bool MoveInDirection(Vector3 direction)
    {
        return _base.MoveTo(transform.position + direction.normalized);
    }

    public void FollowEntity(EntityBase entity)
    {
        entity.OnMove += OnEntityFollowMove;
    }

    private void OnEntityFollowMove()
    {

    }
}
