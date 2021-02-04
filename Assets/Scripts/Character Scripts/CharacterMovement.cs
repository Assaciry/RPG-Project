using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement
{
    public class CharacterMovement : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        ActionScheduler scheduler;
        Health agentHealth;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            scheduler = GetComponent<ActionScheduler>();
            agentHealth = GetComponent<Health>();
        }

        public void MoveCharacter(Vector3 position)
        {
            if(!agentHealth.IsCharacterDead())
            {
                agent.isStopped = false;
                scheduler.StartAction(this);

                agent.SetDestination(position);
            }   
        }

        public void AttackMoveCharacter(Vector3 position)
        {
            if (!agentHealth.IsCharacterDead())
            {
                agent.isStopped = false;

                agent.SetDestination(position);
            }
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public Vector3 GetVelocity()
        {
            return agent.velocity;
        }
    }
}
