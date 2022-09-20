using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Ranged Weapon ScriptableObject", menuName = "RPG Project/RangedWeaponSO", order = 0)]
    public class RangedWeaponSO : IWeaponSO
    {
        [SerializeField] private ProjectileSO projectile = null;

        public void Attack(CharacterFighter attacker, ITargetable target, Vector3 position, float extraDamage)
        {
            GameObject projectileGameObj = Instantiate(projectile.Prefab, position, Quaternion.identity);
            extraDamage += Damage;
            projectileGameObj.GetComponent<ProjectileMover>().InitializeProjectile(attacker, target, projectile.Speed, projectile.maxLifetime, extraDamage);
        }
    }
}
