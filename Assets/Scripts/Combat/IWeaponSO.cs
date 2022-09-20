using UnityEngine;
using System.Collections;

public class IWeaponSO : ScriptableObject
{
    [SerializeField]public GameObject Prefab = null;

    [SerializeField]public string Name = null;

    [SerializeField]public AnimatorOverrideController animationOverride;

    [SerializeField]public float Damage;
    [SerializeField]public float Range;
    [SerializeField]public float timeBetweenAttacks;
    [SerializeField]public float criticalChance;
    [SerializeField]public float criticalExtraDamage;

    [SerializeField]public bool isRightHanded = true;

    [SerializeField] public AudioClip weaponSound;

    public void Equip(Transform rightHand, Transform leftHand, Animator animator)
    {
        if (Prefab != null)
        {
            if (isRightHanded) Instantiate(Prefab, rightHand);
            else Instantiate(Prefab, leftHand);
        }

        if (animationOverride != null)
        {
            animator.runtimeAnimatorController = animationOverride;
        }
    }
}
