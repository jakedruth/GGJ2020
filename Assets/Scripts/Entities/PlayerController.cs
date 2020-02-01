using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class PlayerController : MonoBehaviour
{
    // Components
    private EntityBase entityBase;

    // variables
       

    // Start is called before the first frame update
    void Awake()
    {
        entityBase = GetComponent<EntityBase>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        Vector3 pos = transform.position;

        if (Input.GetKeyDown(KeyCode.A))
            input.x -= 1;
        else if (Input.GetKeyDown(KeyCode.D))
            input.x += 1;
        else if (Input.GetKeyDown(KeyCode.S))
            input.y -= 1;
        else if (Input.GetKeyDown(KeyCode.W))
            input.y += 1;

        if (input != Vector3.zero)
        {
            entityBase.MoveTo(pos + input);
        }

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            AnimalController[] animals = FindObjectsOfType<AnimalController>();

            if (animals.Length != 0)
            {
                AnimalController closest = null;
                float closestSqrDist = float.PositiveInfinity;
                for (int i = 0; i < animals.Length; i++)
                {
                    float sqrDist = (animals[i].transform.position - transform.position).sqrMagnitude;
                    if(sqrDist < closestSqrDist)
                    {
                        closest = animals[i];
                        closestSqrDist = sqrDist;
                    }
                }

                closest.FollowEntity(entityBase);
            }
        }
    }
}
