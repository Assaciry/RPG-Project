using UnityEngine;
using RPG.Core;
using RPG.AnimationControl;
using RPG.Movement;
using System.Collections;

namespace RPG.Combat
{
    public class CharacterFighter : MonoBehaviour, IAction
    {
        ActionScheduler scheduler;
        CharacterMovement movement;
        AnimationController animatonControl;

        Transform hitTarget;

        public float weaponDamage = 10f;
        public float weaponRange = 2f;

        public float timeBetweenAttacks = 1f;
        private float timeSinceLastAttack = 0f;

        private bool cancelAttack = false;

        private void Start()
        {
            scheduler = GetComponent<ActionScheduler>();
            animatonControl = GetComponent<AnimationController>();
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

            StartCoroutine(AttackChaseTarget(target));
            hitTarget = target.transform;
        }

        private IEnumerator AttackChaseTarget<T>(T target) where T: MonoBehaviour
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
                    HitToTarget(target);
                    AdjustRotation(target.transform);
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void HitToTarget<T>(T target) where T: MonoBehaviour
        {
            if(!target.GetComponent<Health>().IsCharacterDead())
            {
                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    animatonControl.AttackAnimationPlay();
                    timeSinceLastAttack = 0f;
                }
            }  
        }

        void Hit()
        {
            if (hitTarget == null) return;

            Health targetHealth = hitTarget.GetComponent<Health>();
            if (targetHealth.IsCharacterDead()) return;

            targetHealth.TakeDamage(weaponDamage);
        }

        public bool IsFeasibleTarget<T>(T target) where T : Health
        {
            if (target.IsCharacterDead()) return false;
            else return true;
        }

        private void AdjustRotation(Transform target)
        {
            if (GetComponent<Health>().IsCharacterDead() || target.GetComponent<Health>().IsCharacterDead()) return;
            transform.LookAt(target);
        }

        public void Cancel()
        {
            cancelAttack = true;
            hitTarget = null;
        }
    }
}
