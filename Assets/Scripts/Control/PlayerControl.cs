using UnityEngine;

namespace RPG.Controller
{
    public class PlayerControl : MonoBehaviour
    {
        Animator animator;
        CharacterMovement movement;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            movement = GetComponent<CharacterMovement>();
        }

        void Update()
        {
            PlayerMouseControl();
            PlayerAnimationController();
        }

        private void PlayerMouseControl()
        {
            if (Input.GetMouseButton(0))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;

                if (Physics.Raycast(mouseRay, out raycastHit))
                {
                    if (raycastHit.transform.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        movement.AttackMoveCharacter(enemy.transform);
                    }
                    else
                    {
                        movement.MoveCharacter(raycastHit.point);

                    }
                }
            }
        }

        private void PlayerAnimationController()
        {
            Vector3 velocity = movement.GetCharacterVelocity();
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}
