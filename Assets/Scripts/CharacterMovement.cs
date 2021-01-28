using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveCharacter(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void AttackMoveCharacter(Transform target)
    {
        // Move to attackable position
        Vector3 targetDirection = (transform.position - target.position).normalized;
        agent.SetDestination(target.position + agent.stoppingDistance * targetDirection);

        AdjustRotation(targetDirection);
    }

    public Vector3 GetCharacterVelocity()
    {
        return agent.velocity;
    }

    private void AdjustRotation(Vector3 targetDirection)
    {
        var newRotation = Quaternion.LookRotation(-targetDirection);
        newRotation.x = 0f;
        newRotation.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, agent.angularSpeed);
    }

}
