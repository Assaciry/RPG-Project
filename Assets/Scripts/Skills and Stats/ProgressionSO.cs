using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "ProgressionSO", menuName = "RPG Branches/ProgressionSO", order = 0)]
    public class ProgressionSO : ScriptableObject
    {
        [SerializeField] ProgressionBaseSO[] progressionBaseSOs = null;

        Dictionary<BaseSO, float[]> HpLut = null;
        Dictionary<BaseSO, float[]> ExpLut = null;

        public float GetHealth(BaseSO baseSO, int level)
        {
            InitializeHpLut();

            return HpLut[baseSO][level];
        }

        public float GetNextLevelXp(BaseSO baseSO, int level)
        {
            InitializeExpLut();

            return ExpLut[baseSO][level];
        }

        private void InitializeHpLut()
        {
            if (HpLut != null) return;
            else
            {
                HpLut = new Dictionary<BaseSO, float[]>();
                foreach (ProgressionBaseSO progressionBase in progressionBaseSOs)
                {
                    HpLut.Add(progressionBase.baseSO, progressionBase.Health);
                }
            }
        }

        private void InitializeExpLut()
        {
            if (ExpLut != null) return;
            else
            {
                ExpLut = new Dictionary<BaseSO, float[]>();
                foreach (ProgressionBaseSO progressionBase in progressionBaseSOs)
                {
                    ExpLut.Add(progressionBase.baseSO, progressionBase.TotalExpForLevelUp);
                }
            }
        }

    }

    

    [System.Serializable]
    class ProgressionBaseSO
    {
        [SerializeField] public BaseSO baseSO;
        [SerializeField] public float[] Health;
        [SerializeField] public float[] TotalExpForLevelUp;
    }
}