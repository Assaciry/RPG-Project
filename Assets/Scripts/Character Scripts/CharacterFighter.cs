using UnityEngine;
using RPG.Core;
using RPG.AnimationControl;
using RPG.Movement;
using System.Collections;

namespace RPG.Combat
{
    public class CharacterFighter : MonoBehaviour, IAction, IController
    {
        ActionScheduler scheduler;
        CharacterMovement movement;
        AnimationController animatonControl;

        public float weaponDamage = 10f;
        public float weaponRange = 2f;

        public float timeBetweenAttacks = 1f;
        private float timeSinceLastAttack = 0f;

        private bool cancelAttack = false;

        ITargetable targetable;

        private void Awake()
        {
            scheduler = GetComponent<ActionScheduler>();
            animatonControl = GetComponent<AnimationController>();
            movement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void AttackToTarget(ITargetable target)
        {
            scheduler.StartAction(this);
            cancelAttack = false;

            StartCoroutine(AttackChaseTarget(target));
        }

        private IEnumerator AttackChaseTarget(ITargetable target)
        {
            AdjustRotation(target.targetPos);

            while (!cancelAttack && !target.targetHealth.IsCharacterDead())
            {
                Vector3 targetP = target.targetPos;
                Vector3 targetDirection = (transform.position - targetP).normalized;
                Vector3 targetPos = targetP + weaponRange * targetDirection;

                float distance = Vector3.Distance(transform.position, targetPos);

                if (distance > weaponRange)
                {
                    movement.AttackMoveCharacter(targetPos);
                }

                else
                {
                    HitToTarget(target);
                    
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        private void HitToTarget(ITargetable target)
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                AdjustRotation(target.targetPos);

                targetable = target;
                animatonControl.AttackAnimationPlay();
                timeSinceLastAttack = 0f;
            }
        }

        void Hit()
        {
            targetable.targetHealth.TakeDamage(weaponDamage);
        }

        private void AdjustRotation(Vector3 target)
        {
            transform.LookAt(target);
        }

        public void Cancel()
        {
            cancelAttack = true;
        }

        public void Disable()
        {
            StopAllCoroutines();
            enabled = false;
        }
    }
}
