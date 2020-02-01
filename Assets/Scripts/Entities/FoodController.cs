using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class FoodController : MonoBehaviour
{
    public EntityBase EntityBase { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        EntityBase = GetComponent<EntityBase>(); 
    }
}
