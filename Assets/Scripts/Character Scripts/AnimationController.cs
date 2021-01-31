using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.AnimationControl
{
    public class AnimationController : MonoBehaviour
    {
        Animator animator;
        NavMeshAgent agent;

        CharacterFighter fighter;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<CharacterFighter>();
        }

        private void AttackAnimationPlay()
        {
            animator.SetTrigger("attack");
        }

        private void Update()
        {
            PlayerAnimationController();
        }

        private void PlayerAnimationController()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}
