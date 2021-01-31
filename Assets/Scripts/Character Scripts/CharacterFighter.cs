using UnityEngine;
using RPG.Core;
using System;
using RPG.Movement;
using System.Collections;

namespace RPG.Combat
{
    public class CharacterFighter : MonoBehaviour, IAction
    {
        ActionScheduler scheduler;
        CharacterMovement movement;

        Transform hitTarget;

        public float weaponDamage = 10f;
        public float weaponRange = 2f;

        public float timeBetweenAttacks = 1f;
        private float timeSinceLastAttack = 0f;

        private bool cancelAttack = false;

        private void Start()
        {
            scheduler = GetComponent<ActionScheduler>();
            movement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void AttackToTarget<T>(T target) where T: MonoBehaviour
        {
            scheduler.StartAction(this);
            cancelAttack = false;

            StartCoroutine(AttackChaseTarget(target.transform));

            hitTarget = target.transform;      
        }

        private IEnumerator AttackChaseTarget(Transform target)
        {  
            while(true && !cancelAttack)
            {
                Transform targetTransform = target.transform;
                Vector3 targetDirection = (transform.position - targetTransform.position).normalized;
                Vector3 targetPos = targetTransform.position + weaponRange * targetDirection;

                float distance = Vector3.Distance(transform.position, targetPos);

                if (distance > weaponRange)
                {
                    movement.AttackMoveCharacter(targetPos);
                }

                else
                {
                    AdjustRotation(targetDirection);
                    HitToTarget();   
                }

                yield return new WaitForSeconds(1f);
            }
            
        }

        public void HitToTarget()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                BroadcastMessage("AttackAnimationPlay");
                timeSinceLastAttack = 0f;
            }
        }

        void Hit()
        {
            if (hitTarget == null) return;
            hitTarget.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        private void AdjustRotation(Vector3 targetDirection)
        {
            var newRotation = Quaternion.LookRotation(-targetDirection);
            newRotation.x = 0f;
            newRotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 180 * Time.deltaTime);
        }

        public void Cancel()
        {
            cancelAttack = true;
            hitTarget = null;
        }
    }
}
