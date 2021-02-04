using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Enemies;

namespace RPG.Controller
{
    public class PlayerControl : MonoBehaviour
    {
        CharacterFighter fighter;
        CharacterMovement movement;

        private void Start()
        {
            movement = GetComponent<CharacterMovement>();
            fighter = GetComponent<CharacterFighter>();
        }

        void Update()
        {
            PlayerMouseControl();
        }

        private void PlayerMouseControl()
        {
            if (Input.GetMouseButton(1))
            {
                ProcessMouseInput();
            }
        }

        private void ProcessMouseInput()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            bool isAttackMove = false;

            foreach(var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Enemy enemy))
                {
                    if(fighter.IsFeasibleTarget(enemy.ReturnEnemyHealthComponent()))
                    {
                        fighter.AttackToTarget(enemy);
                        isAttackMove = true;
                    }
                }
                else { continue; }
            }

            if (!isAttackMove)
            {
                if (Physics.Raycast(GetMouseRay(), out RaycastHit hit))
                {
                    movement.MoveCharacter(hit.point);
                }
            }
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
