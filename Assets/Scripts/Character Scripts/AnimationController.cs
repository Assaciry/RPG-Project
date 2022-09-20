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

            GetComponent<Combat.Health>().OnCharacterDeath += AnimationController_OnCharacterDeath;
        }

        private void AnimationController_OnCharacterDeath(object sender, EventArgs e)
        {
            DeathAnimationPlay();
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

        public void AttackAnimationOverride(AnimatorOverrideController weaponAnimOverride)
        {
            animator.runtimeAnimatorController = weaponAnimOverride;
        }

        public void DeathAnimationPlay()
        {
            animator.SetTrigger("death");
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public void Disable()
        {
            animator.SetFloat("forwardSpeed", 0);
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }
    }
}
