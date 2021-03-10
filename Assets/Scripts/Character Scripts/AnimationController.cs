using UnityEngine;
using RPG.Movement;
using System;

namespace RPG.AnimationControl
{
    public class AnimationController : MonoBehaviour, IController
    {
        Animator animator;
        CharacterMovement movement;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            movement = GetComponent<CharacterMovement>();
        }

        private void AnimationController_OnCharacterDeath(object sender, EventArgs e)
        {
            Disable();
        }

        private void Update()
        {
            MovementAnimation();
        }

        private void MovementAnimation()
        {
            if (movement == null) return;

            Vector3 velocity = movement.GetVelocity();
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

        public void Disable()
        {
            enabled = false;
        }
    }
}
