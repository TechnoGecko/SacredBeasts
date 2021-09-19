using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] int enemyHealth;
    [SerializeField] float enemyDamage;

    void Start()
    {
        
    }

    void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    internal void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        Debug.Log("Damage taken");
    }
}
