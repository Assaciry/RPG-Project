using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
using System;
using RPG.Saving;

namespace RPG.Movement
{
    public class CharacterMovement : MonoBehaviour, IAction, ISaveable
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

        public void SetSpeed(float speed)
        {
            if(agent != null)
                agent.speed = speed;
        }

        public Func<Vector3> GetVelocity()
        {
            return () => agent.velocity;
        }

        public bool IsReachedPosition(Vector3 targetPos, float checkDistance)
        {
            return Vector3.Distance(targetPos, transform.position) < checkDistance;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;

            agent.enabled = false;
            transform.position = position.ToVector();
            agent.enabled = true;
        }
    }
}
