using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D()
    {
        Destroy(gameObject);
    }
}
