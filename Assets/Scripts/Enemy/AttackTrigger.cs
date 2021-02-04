using UnityEngine;
using RPG.Controller;
using RPG.Core;
using RPG.Combat;

namespace RPG.Enemies
{
    public class AttackTrigger : MonoBehaviour
    {
        ChaseRangeExit rangeExit;
        EnemyControl control;

        private void Start()
        {
            control = GetComponentInParent<EnemyControl>();
            rangeExit = control.GetComponentInChildren<ChaseRangeExit>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out PlayerControl player))
            {
                rangeExit.SetIsChaseable(true);
                control.AttackToPlayer(player);
            }
        }

    }
}
