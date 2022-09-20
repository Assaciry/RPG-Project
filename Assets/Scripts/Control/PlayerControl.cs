using System;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Controller
{
    public class PlayerControl : MonoBehaviour, IController, ITargetable
    {
       

        [Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D Texture;
            public Vector2 Hotspot;
        }

        private Camera mainCamera;
        private EventSystem eventSystem;

        [SerializeField] private CursorMapping[] cursorMappings = null;
        [SerializeField] private float maxTraversableDist = 35;
        [SerializeField] private Vector3 focusPoint;


        public event EventHandler<Health> OnEnemyTargeted;

        CharacterFighter fighter;
        CharacterMovement movement;

        public Transform targetTransform => transform;

        public Vector3 targetPos => transform.position;

        public Health targetHealth => GetComponent<Health>();

        Vector3 ITargetable.focusPoint { get => focusPoint; set => focusPoint = value; }

        ITargetable attackTarget = null;

        private void Awake()
        {
            movement = GetComponent<CharacterMovement>();
            fighter = GetComponent<CharacterFighter>();

            mainCamera = Camera.main;
            eventSystem = EventSystem.current;
        }

        void Update()
        {
            if(InteractWithUI())
            {
                SetCursor(CursorType.UI);
                return;
            }

            if (InteractWithComponent()) return;

            if(InteractWithCombat())
            {
                SetCursor(CursorType.Combat);
                return;
            }

            if (InteractWithMovement())
            {
                SetCursor(CursorType.Movement);
                return;
            }

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            return eventSystem.IsPointerOverGameObject();
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast())
                    {
                        SetCursor(CursorType.Pickup);
                        if(Input.GetMouseButtonDown(1))
                        {
                            movement.MoveCharacter(hit.point);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out ITargetable target) &&
                    !target.targetHealth.IsCharacterDead() &&
                    target != this)
                {
                    if(Input.GetMouseButton(1))
                    {
                        if (target == attackTarget) return true;

                        attackTarget = target;
                        fighter.AttackToTarget(target);

                        OnEnemyTargeted?.Invoke(this, target.targetHealth);
                    }

                    return true;
                }

                else { continue; }
            }

            return false;
        }

        private bool InteractWithMovement()
        {
             attackTarget = null;
            bool hasHit = RaycastNavmesh(out Vector3 hitPos);
             if (hasHit)
             {
                if (Input.GetMouseButton(1))
                {
                    movement.MoveCharacter(hitPos);
                }
                return true;
            }

            return false;
        }

        private bool RaycastNavmesh(out Vector3 hitPos)
        {
            // Raycast to terrain
            // Find nearest navmesh point
            // Return true if found
            // Navmesh.SamplePosition(Vector3 sourcePos (raycasthit pos), out NavMeshHit hit, float maxDist, int areaMask)
            // NavMeshCalculatePath(transform.position, targetpos,int areamask, NavMeshPath path)

            hitPos = new Vector3();

            if (!Physics.Raycast(GetMouseRay(), out RaycastHit hit))  
                return false;

            if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas))
                return false;

            hitPos = navMeshHit.position;
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, hitPos, NavMesh.AllAreas, path);
            if (!hasPath)
              return false;

            if (path.status != NavMeshPathStatus.PathComplete)
                return false;

            if (GetPathLength(path) > maxTraversableDist)
                return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float length = 0;
            if (path.corners.Length < 2) return length;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return length;
        }

        private Ray GetMouseRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.Texture, cursorMapping.Hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach(CursorMapping cursorMapping in cursorMappings)
            {
                if (cursorMapping.cursorType == cursorType)
                    return cursorMapping;
            }

            return cursorMappings[0];
        }

        public void Disable()
        {
            this.enabled = false;
        }

        public void Enable()
        {
            this.enabled = true;
        }
    }
}
