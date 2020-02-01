using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class PlayerController : MonoBehaviour
{
    // Components
    private EntityBase _base;

    // variables



    // Start is called before the first frame update
    void Awake()
    {
        _base = GetComponent<EntityBase>();
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
            _base.MoveTo(pos + input);
        }
    }
}
