using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimal : MonoBehaviour
{
    public float moveSpeed;
    public List<Transform> path;
    private int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        targetIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != path[targetIndex].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[targetIndex].position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (targetIndex == path.Count - 1)
            {
                targetIndex = 0;
                transform.position = path[0].position;
            }
            else
            {
                targetIndex++;
            }
        }
    }
}
