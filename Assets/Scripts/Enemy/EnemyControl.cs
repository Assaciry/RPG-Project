using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using System.Collections.Generic;
using System.Collections;

namespace RPG.Enemies
{
    public class EnemyControl : MonoBehaviour
    {
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float chaseSpeed = 6f;

        CharacterMovement movement;
        CharacterFighter fighter;

        public PatrolPath patrolPath;
        private List<Transform> Waypoints;

        int waypointInit = 0;
        private bool keepPatrol = true;

        private const float waypointCheckDistance = 1f;
        private const float suspiciounTime = 3f;
        private const float dwellingTime = 3f;

        void Start()
        {
            movement = GetComponent<CharacterMovement>();
            fighter = GetComponent<CharacterFighter>();

            GetPatrolPath();
        }

        private void Update()
        {
            if(keepPatrol)
            {
                PatrolRoutine();
            }
        }

        private void GetPatrolPath()
        {
            if (patrolPath == null) return;
            Waypoints = patrolPath.ReturnWaypointTransforms(); 
        }

        private void PatrolRoutine()
        {
            if (Waypoints == null || Waypoints.Count <= 1) return;

            movement.SetSpeed(patrolSpeed);
            var destination = Waypoints[waypointInit].position;
            movement.MoveCharacter(destination);

            if(movement.IsReachedPosition(destination, waypointCheckDistance))
            {
                waypointInit++;
                waypointInit %= Waypoints.Count;
            }
        }

        public void AttackToPlayer(MonoBehaviour player)
        {
            keepPatrol = false;
            movement.SetSpeed(chaseSpeed);
            fighter.AttackToTarget(player);
        }

        public void AttackRangeExit(Vector3 player)
        {
            movement.MoveCharacter(player);
            keepPatrol = true;
        }
    }
}

