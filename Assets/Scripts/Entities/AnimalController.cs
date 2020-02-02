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
    private EntityBase _followingEntity;

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
                        PairWithAnimal(otherAnimal);
                    }
                    else if (otherAnimal.animalType == AnimalType.Prey && animalType == AnimalType.Predator)
                    {
                        // handle eat
                        EatOther(otherAnimal);
                    }
                }
                else if (otherCollider.transform.tag == "Food")
                {
                    if(animalType != AnimalType.Predator && !EntityBase.isPullable)
                    {
                        EntityBase.FollowEntity(otherCollider.GetComponent<EntityBase>());
                    }
                }
            }
        }
    }

    public void PairWithAnimal(AnimalController otherAnimal)
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        

        Destroy(gameObject);
        Destroy(otherAnimal.gameObject);
    }

    public void EatOther(AnimalController otherAnimal)
    {
        Destroy(otherAnimal.gameObject);
    }

    private void OnDestroy()
    {
        if (this != null)
        {
            FindObjectOfType<PlayerController>()?.AnimalOnDestroy(EntityBase);
        }
    }
}
