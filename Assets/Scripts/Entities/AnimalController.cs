using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalType
{
    Prey,
    Predator,
    Neutral
}

[RequireComponent(typeof(EntityBase))]
public class AnimalController : MonoBehaviour
{
    // Components
    public EntityBase EntityBase { get; private set; }

    // Variables
    public AnimalType animalType;

    // Start is called before the first frame update
    void Awake()
    {
        EntityBase = GetComponent<EntityBase>();
    }

    public static readonly Vector3[] directions =
    {
        Vector3.right,
        Vector3.down,
        Vector3.left,
        Vector3.up
    };

    private void FixedUpdate()
    {

        foreach (Vector3 direction in directions)
        {
            Collider2D otherCollider = Physics2D.OverlapPoint(transform.position + direction);
            if(otherCollider != null)
            {
                if (otherCollider.transform.tag == "Animal")
                {
                    AnimalController otherAnimal = otherCollider.transform.GetComponent<AnimalController>();

                    // If otherAnimal.name == name
                    // they are the same
                    if (otherAnimal.name == name)
                    {
                        Destroy(gameObject);
                        Destroy(otherAnimal.gameObject);
                    }
                    else if (otherAnimal.animalType == AnimalType.Prey && animalType == AnimalType.Predator)
                    {
                        // handle eat
                        Destroy(otherAnimal.gameObject);
                    }
                }
            }
        }
    }
}
