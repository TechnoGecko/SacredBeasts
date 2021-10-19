using System;
using UnityEngine;

namespace Characters
{
    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] private Character _Character;
        public Character Character => _Character;
        
        [SerializeField] float runSpeed = 10f;


        private void FixedUpdate()
        {
            MoveCharacter(Character.InputDirection.x);
        }

        private void MoveCharacter(float horizontal)
        {
            Character.Body.Rigidbody2D.AddForce(Vector2.right * horizontal * runSpeed);
        }
    }
}
