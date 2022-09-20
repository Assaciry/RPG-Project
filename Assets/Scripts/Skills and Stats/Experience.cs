using RPG.ScratchSaving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, IScratchSaveable
    {
        [SerializeField] float experinecePoints;
        public BaseStats baseStats;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        public void GainExperience(float experienceAmount)
        {
            experinecePoints += experienceAmount;

            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            if(experinecePoints >= baseStats.GetLevelUpExp())
            {
                baseStats.LevelUp();
            }
        }

        public object CaptureState()
        {
            return experinecePoints;
        }

        public void RestoreState(object state)
        {
            experinecePoints = (float)state;
        }
    }
}
