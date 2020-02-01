using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class AnimalController : MonoBehaviour
{
    // Components
    public EntityBase EntityBase { get; private set; }
    public bool isMovable;

    // Start is called before the first frame update
    void Awake()
    {
        EntityBase = GetComponent<EntityBase>();
    }

    public bool MoveInDirection(Vector3 direction)
    {
        return EntityBase.MoveTo(transform.position + direction.normalized);
    }
}
