using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
using System;
using RPG.ScratchSaving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public class CharacterMovement : MonoBehaviour, IAction, IScratchSaveable, IController
    {
        private NavMeshAgent agent;
        private ActionScheduler scheduler;
        private Health agentHealth;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            scheduler = GetComponent<ActionScheduler>();
            agentHealth = GetComponent<Health>();
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

        public void SetSpeed(float speed)
        {
            if(agent != null)
                agent.speed = speed;
        }

        public Vector3 GetVelocity()
        {
            return agent.velocity;
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
            scheduler.CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;

            transform.position = ((SerializableVector3)state).ToVector3();

            GetComponent<NavMeshAgent>().enabled = true;
        }

        public void Disable()
        {
            Cancel();
            agent.enabled = false;
            enabled = false;
        }

        public void Enable()
        {
            agent.enabled = true;
            enabled = true;
        }
    }
}
