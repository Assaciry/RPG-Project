using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    private const float gizmoRadius = 1f;
    
    public List<Transform> ReturnWaypointTransforms()
    {
        List<Transform> waypointsList = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            waypointsList.Add(transform.GetChild(i));
        }

        return waypointsList;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount ; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(transform.GetChild(i).position, gizmoRadius);
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
        }
    }

    private int GetNextIndex(int i)
    {
        if (i < transform.childCount - 1) return i + 1;
        else return 0;
    }
}
