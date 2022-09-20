using UnityEngine;
using System.Collections;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Ranged Weapon ScriptableObject", menuName = "RPG Project/MeleeWeaponSO", order = 0)]
    public class MeleeWeaponSO : IWeaponSO
    {
        [SerializeField] public float damageRadius = 2;

        public void Attack(Vector3 position, CharacterFighter attacker, float extraDamage)
        {
            Collider[] colliders = Physics.OverlapSphere(position, damageRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out ITargetable targetable))
                {
                    if (targetable == attacker.Instance) continue;
                    targetable.targetHealth.TakeDamage(attacker, Damage + extraDamage);
                }
            }
        }

        public void Attack(CharacterFighter attakcer, ITargetable target, float extraDamage)
        {
            target.targetHealth.TakeDamage(attakcer, Damage + extraDamage);
        }
    }
}
