using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Lasso,
}

[RequireComponent(typeof(EntityBase))]
public class ItemController : MonoBehaviour
{
    // Components
    public EntityBase Entity { get; private set; }

    // Variables
    public ItemType itemType;

    void Awake()
    {
        Entity = transform.GetComponent<EntityBase>();
    }

    public void PickUp()
    {
        switch (itemType)
        {
            case ItemType.Lasso:
                PlayerController pc = FindObjectOfType<PlayerController>();
                pc.SetCanUseRope(true);
                break;
        }

        Destroy(gameObject);
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Player")
    //    {
    //        PlayerController player = collision.GetComponent<PlayerController>();
    //        player.SetCanUseRope(true);
    //        Destroy(gameObject);
    //    }
    //}
}
