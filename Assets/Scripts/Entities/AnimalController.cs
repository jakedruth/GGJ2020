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
    public EntityBase Entity { get; private set; }

    // Variables
    public AnimalType animalType;
    private EntityBase _followingEntity;

    // Start is called before the first frame update
    void Awake()
    {
        Entity = GetComponent<EntityBase>();
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
        if (Entity.MovingCoroutine == null)
        {
            foreach (Vector3 direction in directions)
            {
                Vector3 displacement = direction * 0.75f;
                Collider2D otherCollider = Physics2D.OverlapPoint(transform.position + displacement);

                if (otherCollider == null)
                    continue;

                if (otherCollider.transform == transform)
                    continue;

                EntityBase otherEntity = otherCollider.transform.GetComponent<EntityBase>();
                if (otherEntity == null)
                    continue;

                if (otherEntity.MovingCoroutine != null)
                    continue;

                if (otherCollider.transform.tag == "Animal")
                {
                    AnimalController otherAnimal = otherEntity.GetComponent<AnimalController>();

                    if (otherAnimal == null)
                        continue;

                    string myName = name.Split(' ')[0];
                    string otherName = otherAnimal.name.Split(' ')[0];

                    if (myName.Equals(otherName))
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
                    if (animalType != AnimalType.Predator && !Entity.isPullable)
                    {
                        if (!Entity.IsFollowing && !otherEntity.IsBeingFollowed)
                        {
                            EmoteSystemManager.instance.CreateEmote(transform, "hungry");
                            Entity.FollowEntity(otherCollider.GetComponent<EntityBase>());
                        }
                    }
                }
            }
        }
    }

    public void PairWithAnimal(AnimalController otherAnimal)
    {
        Vector3 point = Vector3.Lerp(transform.position, otherAnimal.transform.position, .5f);
        EmoteSystemManager.instance.CreateEmote(point, "hearts");

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
            FindObjectOfType<PlayerController>()?.AnimalOnDestroy(Entity);
        }
    }
}
