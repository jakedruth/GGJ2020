using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoPickupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.SetCanUseRope(true);
            Destroy(gameObject);
        }
    }
}
