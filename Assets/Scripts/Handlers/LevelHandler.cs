using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public List<AnimalController> animalList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (AnimalController animal in FindObjectsOfType<AnimalController>())
        {
            animalList.Add(animal);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (animalList.Count != 0)
        {
            for (int i = animalList.Count - 1; i >= 0; i--)
            {
                if (animalList[i] == null)
                {
                    animalList.Remove(animalList[i]);
                }
            }
        }
        else
        {
            //end level
        }
    }
}
