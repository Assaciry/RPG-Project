using UnityEngine;
using RPG.Core;
using RPG.AnimationControl;
using RPG.Movement;
using System.Collections;
using RPG.ScratchSaving;
using RPG.Stats;

namespace RPG.Combat
{
    public class CharacterFighter : MonoBehaviour, IAction, IController, IScratchSaveable
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private IWeaponSO defaultWeapon;
        [SerializeField] private bool isCleaveDamager = false;

        ActionScheduler scheduler;
        CharacterMovement movement;
        AnimationController animationControl;

        AudioSource audioSource;

        private float timeSinceLastAttack = 0f;
        public float extraDamage = 0;
        private bool cancelAttack = false;
        private IWeaponSO currentWeapon;

        private BaseStats baseStats;

        public ITargetable Instance;
        ITargetable targetable;

        private void Awake()
        {
            scheduler = GetComponent<ActionScheduler>();
            animationControl = GetComponent<AnimationController>();
            movement = GetComponent<CharacterMovement>();
            baseStats = GetComponent<BaseStats>();
            audioSource = GetComponent<AudioSource>();

            baseStats.OnLevelUp += BaseStats_OnLevelUp;
            baseStats.OnStateRestored += BaseStats_OnStateRestored;

            Instance = GetComponent<ITargetable>();

            if (defaultWeapon == null)
            {
                defaultWeapon = Resources.Load("Fists", typeof(IWeaponSO)) as IWeaponSO;
            }

            currentWeapon = defaultWeapon;

            extraDamage = baseStats.extraDamage;
        }

        private void Start()
        {
            EquipWeapon(currentWeapon);
        }

        private void BaseStats_OnStateRestored(object sender, BaseStats.Saveables e)
        {
            extraDamage = e.extraDamage;
        }

        private void BaseStats_OnLevelUp(object sender, BaseStats.OnLevelUpEvents e)
        {
            extraDamage = e.extraDamage;
        }

        public void EquipWeapon(IWeaponSO weapon)
        {
            if (currentWeapon != null)
                Unequip();

            currentWeapon = weapon;
            currentWeapon.Equip(rightHandTransform, leftHandTransform, animationControl.GetAnimator());
        }

        public void Unequip()
        {
            string pfName = "";

            if (currentWeapon.Prefab != null)
            {
                pfName = currentWeapon.Prefab.name + "(Clone)";

                Transform oldWeapon;

                if (currentWeapon.isRightHanded) { oldWeapon = rightHandTransform.Find(pfName); }
                else { oldWeapon = leftHandTransform.Find(pfName); }

                if (oldWeapon != null) Destroy(oldWeapon.gameObject);
            }
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
                Vector3 targetPos = targetP + currentWeapon.Range * targetDirection;

                float distance = Vector3.Distance(transform.position, targetPos);
 
                if (distance > currentWeapon.Range)
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
            if (timeSinceLastAttack > currentWeapon.timeBetweenAttacks)
            {
                AdjustRotation(target.targetPos);

                targetable = target;
                animationControl.AttackAnimationPlay();
                timeSinceLastAttack = 0f;
            }
        }

        public void Shoot()
        {
            Vector3 direction = (targetable.targetPos - transform.position).normalized;
            (currentWeapon as RangedWeaponSO).Attack(this, targetable,  leftHandTransform.position + transform.forward, extraDamage);

            audioSource.PlayOneShot(currentWeapon.weaponSound);
        }

        void Hit()
        {
            AdjustRotation(targetable.targetPos);

            if (isCleaveDamager)
                (currentWeapon as MeleeWeaponSO).Attack(transform.position, this, extraDamage);
            else
                (currentWeapon as MeleeWeaponSO).Attack(this, targetable, extraDamage);

            audioSource.PlayOneShot(currentWeapon.weaponSound);
        }

        public void AdjustRotation(Vector3 target)
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

        public void Enable()
        {
            enabled = true;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            IWeaponSO weapon = Resources.Load((string)state, typeof(IWeaponSO)) as IWeaponSO;
            EquipWeapon(weapon);
        }
    }
}
