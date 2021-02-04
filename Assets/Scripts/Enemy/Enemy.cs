using UnityEngine;
using RPG.Combat;

namespace RPG.Enemies
{
    public class Enemy : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

        public Health ReturnEnemyHealthComponent()
        {
            return health;
        }
    }
}
