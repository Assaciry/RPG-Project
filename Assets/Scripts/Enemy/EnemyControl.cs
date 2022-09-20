using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.AI;

namespace RPG.Enemies
{
    public class EnemyControl : MonoBehaviour, IController, ITargetable
    {
        [SerializeField] private Transform watchoutTransform;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float chaseSpeed = 6f;
        [SerializeField] private float mobRadius = 10f;
        [SerializeField] private Vector3 focusPoint;
        [SerializeField] GameObject exclamationMark;
        [SerializeField] GameObject questionMark;

        CharacterMovement movement;
        CharacterFighter fighter;

        public PatrolPath patrolPath;
        private List<Transform> Waypoints;

        int waypointInit = 0;
        public bool keepPatrol = true;

        private const float waypointCheckDistance = 1f;
        private const float suspiciounTime = 3f;
        private const float dwellingTime = 3f;

        public Transform targetTransform => transform;
        public Vector3 targetPos => transform.position;
        public Health targetHealth => GetComponent<Health>();
        Vector3 ITargetable.focusPoint { get => focusPoint; set => focusPoint = value; }

        void Awake()
        {
            movement = GetComponent<CharacterMovement>();
            fighter = GetComponent<CharacterFighter>();
            GetComponent<Health>().OnCharacterAggrevated += EnemyControl_OnCharacterAggrevated; ;

            GetPatrolPath();
        }

        private void EnemyControl_OnCharacterAggrevated(object sender, ITargetable player)
        { 
            AttackToPlayer(player);
            AggroMobCall(player);
        }

        public void AggroMobCall(ITargetable player)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, mobRadius);
            foreach(Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyControl control))
                    if (control.isActiveAndEnabled)
                        control.AttackToPlayer(player);
            }
        }

        private void Start()
        {
            InitializePatrolBehaviour();
        }

        private void InitializePatrolBehaviour()
        {
            keepPatrol = true;
            movement.SetSpeed(patrolSpeed);

            if (Waypoints != null && Waypoints.Count > 1)
                StartCoroutine(PatrolRoutine());
            else
                movement.MoveCharacter(watchoutTransform.position);
        }

        private void GetPatrolPath()
        {
            watchoutTransform = transform;
            if (patrolPath == null) return;
            Waypoints = patrolPath.ReturnWaypointTransforms(); 
        }

        private IEnumerator PatrolRoutine()
        {
            Vector3 destination = Waypoints[waypointInit].position;
            movement.MoveCharacter(destination);

            while (keepPatrol)
            {
                if (movement.IsReachedPosition(destination, waypointCheckDistance))
                {
                    waypointInit++;
                    waypointInit %= Waypoints.Count;

                    destination = Waypoints[waypointInit].position;
                    movement.MoveCharacter(destination);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void AttackToPlayer(ITargetable player)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, player.targetPos, NavMesh.AllAreas, path);
            if (!hasPath)
            { Debug.Log("no2"); return; }

            if (path.status != NavMeshPathStatus.PathComplete)
                return;

            keepPatrol = false;
            exclamationMark.SetActive(true);
            movement.SetSpeed(chaseSpeed);
            fighter.AttackToTarget(player);
        }

        public void AttackRangeExit(Vector3 playerLastSeen)
        {
            exclamationMark.SetActive(false);
            StartCoroutine(SuspicionRoutine(playerLastSeen));
        }

        private IEnumerator SuspicionRoutine(Vector3 playerLastSeen)
        {
            movement.SetSpeed(patrolSpeed);
            questionMark.SetActive(true);
            movement.MoveCharacter(playerLastSeen);
            yield return new WaitUntil(() => movement.IsReachedPosition(playerLastSeen, waypointCheckDistance));
            yield return new WaitForSeconds(suspiciounTime);
            questionMark.SetActive(false);
            InitializePatrolBehaviour();
        }

        public void Disable()
        {
            transform.Find("ChaseRangeTrigger").gameObject.SetActive(false);
            transform.Find("AttackRangeTrigger").gameObject.SetActive(false);
            exclamationMark.SetActive(false);
            questionMark.SetActive(false);
            enabled = false;
        }

        public void Enable()
        {
            transform.Find("ChaseRangeTrigger").gameObject.SetActive(true);
            transform.Find("AttackRangeTrigger").gameObject.SetActive(true);
            this.enabled = true;
        }
    }
}

