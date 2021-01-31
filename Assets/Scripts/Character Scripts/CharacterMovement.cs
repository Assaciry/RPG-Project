using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class CharacterMovement : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        ActionScheduler scheduler;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            scheduler = GetComponent<ActionScheduler>();
        }

        public void MoveCharacter(Vector3 position)
        {
            agent.isStopped = false;
            scheduler.StartAction(this);

            agent.SetDestination(position);
        }

        public void AttackMoveCharacter(Vector3 position)
        {
            agent.isStopped = false;

            agent.SetDestination(position);
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }
    }
}
