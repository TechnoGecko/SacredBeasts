using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkullHandler : MonoBehaviour
{
    private float skulls = 0;

    public TextMeshProUGUI skullCount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Skull"))
        {
            skulls++;
            skullCount.text = skulls.ToString();
            
            Debug.Log("Got'im");
            
            Destroy(other.gameObject);
        }
    }
}
