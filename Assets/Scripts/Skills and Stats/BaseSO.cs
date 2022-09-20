using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "BaseSO", menuName = "RPG Branches/BaseSO", order = 0)]
    public class BaseSO : ScriptableObject
    {
        [SerializeField] public string Name;

        public float experienceMultipler = 1f;
        public float baseDamage = 5;
        public float damageMultiplierPerLevel = 1.1f;
        private Experience experience;

    }
}