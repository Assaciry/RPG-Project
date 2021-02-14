using UnityEngine;
using RPG.Movement;
using System;

namespace RPG.AnimationControl
{
    public class AnimationController : MonoBehaviour
    {
        Animator animator;
        CharacterMovement movement;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            movement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            MovementAnimation();
        }

        private void MovementAnimation()
        {
            if (movement == null) return;

            Func<Vector3> velocityFunc = movement.GetVelocity();
            Vector3 velocity = velocityFunc();
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public void AttackAnimationPlay()
        {
            animator.SetTrigger("attack");
        }

        public void AttackAnimationStop()
        {
            animator.SetTrigger("stopAttack");
        }

        public void DeathAnimationPlay()
        {
            animator.SetTrigger("death");
        }
    }
}
