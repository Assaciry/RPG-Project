using UnityEngine;
using RPG.Combat;

namespace RPG.Enemies
{
    public class AttackTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent<Health>(out Health player);
            GetComponentInParent<EnemyAI>().AttackRangeEnter(player);
        }
    }
}
