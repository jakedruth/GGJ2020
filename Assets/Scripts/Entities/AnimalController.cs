using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class AnimalController : MonoBehaviour
{
    // Components
    private EntityBase entityBase;
    public bool isMovable;

    // Start is called before the first frame update
    void Awake()
    {
        entityBase = GetComponent<EntityBase>();
    }

    public bool MoveInDirection(Vector3 direction)
    {
        return entityBase.MoveTo(transform.position + direction.normalized);
    }

    public void FollowEntity(EntityBase entity)
    {
        entity.OnMove += OnEntityFollowMove;
    }

    private void OnEntityFollowMove(Vector3 start, Vector3 end)
    {
        entityBase.MoveTo(start, true);
    }
}
