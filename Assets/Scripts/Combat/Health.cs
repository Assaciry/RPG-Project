using UnityEngine;
using RPG.AnimationControl;
using RPG.Saving;

namespace RPG.Combat
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        private bool isDead = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            if(health <= 0f && !isDead)
            {
                CharacterDeath();

            }
        }

        private void CharacterDeath()
        {
            isDead = true;
            AnimationController animation = GetComponent<AnimationController>();
            animation.DeathAnimationPlay();
        }

        public bool IsCharacterDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;

            if (health <= 0f && !isDead)
            {
                CharacterDeath();

            }
        }
    }
}
