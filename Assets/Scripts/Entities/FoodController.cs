using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class FoodController : MonoBehaviour
{
    public EntityBase Entity { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        Entity = GetComponent<EntityBase>(); 
    }
}
