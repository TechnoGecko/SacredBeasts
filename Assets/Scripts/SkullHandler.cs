using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullHandler : MonoBehaviour
{
    private float skulls = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Skull"))
        {
            skulls++;
            Destroy(other.gameObject);
            Debug.Log("Got'im");
            
        }
    }
}
