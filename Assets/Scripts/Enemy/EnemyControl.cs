using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using System.Collections.Generic;
using System.Collections;

namespace RPG.Enemies
{
    public class EnemyControl : MonoBehaviour
    {
        CharacterMovement movement;
        CharacterFighter fighter;

        public PatrolPath patrolPath;
        private List<Transform> Waypoints;

        int waypointIndex = 0;
        private bool keepPatrol = true;

        private const double waypointCheckDistance = 1f;
        private const float suspiciounTime = 3f;

        void Start()
        {
            movement = GetComponent<CharacterMovement>();
            fighter = GetComponent<CharacterFighter>();

            GetPatrolPath();
            PatrolBehaviour();
        }

        private void GetPatrolPath()
        {
            if (patrolPath == null) return;
            Waypoints = patrolPath.ReturnWaypointTransforms();
        }

        private void PatrolBehaviour()
        {
            keepPatrol = true;
            if (Waypoints != null && Waypoints.Count > 1)
                StartCoroutine(EnemyPatrolWaypoints());
        }

        private IEnumerator EnemyPatrolWaypoints()
        {
            while (true && keepPatrol)
            {
                Vector3 wayPos = Waypoints[waypointIndex].position;
                movement.MoveCharacter(wayPos);

                if (IsReachedPosition(wayPos))
                {
                    waypointIndex++;
                }
                waypointIndex = waypointIndex % Waypoints.Count;
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator SuspicionBehaviour(Vector3 playerPos)
        {
            while(true)
            {
                if (IsReachedPosition(playerPos)) break;
                yield return new WaitForSeconds(suspiciounTime);
            }
            PatrolBehaviour();
        }

        private bool IsReachedPosition(Vector3 targetPos)
        {
            return Vector3.Distance(targetPos, transform.position) < waypointCheckDistance;
        }

        public void AttackRangeExit(Vector3 player)
        {
            movement.MoveCharacter(player);
            StartCoroutine(SuspicionBehaviour(player));
        }

        public void AttackToPlayer(MonoBehaviour player)
        {
            keepPatrol = false;
            fighter.AttackToTarget(player);
        }
    }
}

