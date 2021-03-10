using UnityEngine;
using RPG.Combat;

namespace RPG.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private Health health;



        private void Awake()
        {
            health = GetComponent<Health>();
        }

        public Health ReturnEnemyHealthComponent()
        {
            return health;
        }
    }
}
