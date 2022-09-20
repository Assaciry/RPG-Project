using UnityEngine;
using RPG.Stats;
using RPG.ScratchSaving;
using System;

namespace RPG.Combat
{
    public class Health : MonoBehaviour, IScratchSaveable 
    {
        [SerializeField] private float healthPoints = 100f;
        [SerializeField] private ParticleSystem healFX;

        private float maxHealthPoints = 100f;

        public event EventHandler OnCharacterDeath;
        public event EventHandler<float> OnTookDamage;
        public event EventHandler<float> OnHealthChanged;
        public event EventHandler<ITargetable> OnCharacterAggrevated;

        private bool isDead = false;
        private BaseStats stats;

        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            stats.OnLevelUp += Stats_OnLevelUp;
        }

        private void Start()
        {
            maxHealthPoints = stats.GetStartingHealth();
            healthPoints = maxHealthPoints;
        }

        private void Stats_OnLevelUp(object sender, BaseStats.OnLevelUpEvents e)
        {
            maxHealthPoints = e.newHealth;
            HealCompletely();
        }

        public void TakeDamage(CharacterFighter attacker, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            OnTookDamage?.Invoke(this, damage);
            OnHealthChanged?.Invoke(this, GetHealthAmountNormalized());
            OnCharacterAggrevated?.Invoke(this, attacker.GetComponent<ITargetable>());

            if (healthPoints <= 0f && !isDead)
            {
                AwardAttackerWithXP(attacker);
                CharacterDeath();
            }
        }

        private void AwardAttackerWithXP(CharacterFighter attacker)
        {
            float exp = GetComponent<Experience>().baseStats.GetExperienceAwardOfThis();
            attacker.GetComponent<Experience>().GainExperience(exp);
            Debug.Log(attacker.transform.name + " Gained " + exp + "XP.");
        }

        private void CharacterDeath()
        {
            isDead = true;
            OnCharacterDeath?.Invoke(this, EventArgs.Empty);
        }

        public bool IsCharacterDead()
        {
            return isDead;
        }

        public void Heal(float healAmount)
        {
            healthPoints += healAmount;
            healthPoints = Mathf.Min(healthPoints, maxHealthPoints);

            ParticleSystem heal = healFX;
            var emission = heal.emission.GetBurst(0).count;
            emission = healAmount;

            Instantiate(heal, transform.position + Vector3.up, Quaternion.identity);

            OnHealthChanged?.Invoke(this, GetHealthAmountNormalized());

        }

        public void HealCompletely()
        {
            healthPoints = maxHealthPoints;
            Instantiate(healFX, transform.position + Vector3.up, Quaternion.identity);
            OnHealthChanged?.Invoke(this, GetHealthAmountNormalized());
        }

        public float GetHealthAmountNormalized()
        {
            return healthPoints / maxHealthPoints;
        }

        public object CaptureState()
        {
            return healthPoints;
        }


        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints <= 0f && !isDead)
            {
                CharacterDeath();
            }
        }
    }
}
