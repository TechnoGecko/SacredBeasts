using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class FiendAttack : MonoBehaviour
{

    private float timeBetweenAttack;
    public float startTimeBetweenAttack;
    NewCharacter2DController controller;

    public Transform attackPosition;
    public float attackRange;
    public LayerMask enemyLayerMask;
    public int damage;


    private void Start()
    {
        controller = GetComponent<NewCharacter2DController>();
    }
    void Update()
    {
        if (timeBetweenAttack <= 0)
        {
            //then attack
            if (Input.GetButtonDown("Fire1"))
            {
                controller.anim.Play("Player_attack1");
                Collider2D[] _enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemyLayerMask);
                for (int i = 0; i < _enemiesToDamage.Length; i++)
                {
                    _enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }

            timeBetweenAttack = startTimeBetweenAttack;
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;
                
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

}