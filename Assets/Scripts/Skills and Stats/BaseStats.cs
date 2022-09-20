using RPG.ScratchSaving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, IScratchSaveable
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] ProgressionSO progression;
        [SerializeField] BaseSO Base;
        [SerializeField] List<BranchSO> Branches;

        public float extraDamage;
        private float extraDamageMultiplier;

        public event EventHandler<OnLevelUpEvents> OnLevelUp;
        public event EventHandler<Saveables> OnStateRestored;

        public class OnLevelUpEvents : EventArgs
        {
            public int newLevel;
            public float newHealth;
            public float extraDamage;
        }

        [Serializable]
        public class Saveables
        {
            public float extraDamage;
            public int currentLevel;

            public Saveables(float extraDamage, int currentLevel)
            {
                this.extraDamage = extraDamage;
                this.currentLevel = currentLevel;
            }
        }

        [SerializeField] private int currentLevel = 0;

        private void Awake()
        {
            if(currentLevel == 0)
                currentLevel = startingLevel;

            extraDamage = Base.baseDamage;
            extraDamageMultiplier = Base.damageMultiplierPerLevel;
        }

        public void LevelUp()
        {
            currentLevel++;
            float health = progression.GetHealth(Base, currentLevel);
            extraDamage *= extraDamageMultiplier;

            OnLevelUp?.Invoke(this, new OnLevelUpEvents { newHealth = health, newLevel = currentLevel, extraDamage = extraDamage });
        }

        public float GetStartingHealth()
        {
            return progression.GetHealth(Base, currentLevel);
        }

        public float GetLevelUpExp()
        {
            return progression.GetNextLevelXp(Base, currentLevel);
        }

        public float GetExperienceAwardOfThis()
        {
            float experienceAward;
            experienceAward = Base.experienceMultipler * currentLevel;

            return experienceAward;
        }

        public object CaptureState()
        {
            return new Saveables(extraDamage, currentLevel);
        }

        public void RestoreState(object state)
        {
            Saveables restored = (Saveables)state;

            extraDamage = restored.extraDamage;
            currentLevel = restored.currentLevel;

            OnStateRestored?.Invoke(this, restored);
        }
    }
}
