using UnityEngine;
using RPG.Controller;
using System;

namespace RPG.Enemies
{
    public class ChaseRangeExit : MonoBehaviour
    {
        EnemyControl control;
        private bool isChaseable;

        private void Start()
        {
            control = GetComponentInParent<EnemyControl>();
            isChaseable = false;
        }

        public void SetIsChaseable(bool isChaseable)
        {
            this.isChaseable = isChaseable;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerControl player))
            {
                if (isChaseable)
                {
                    control.AttackRangeExit(player.transform.position);
                    isChaseable = false;
                }
            }
        }
    }
}
