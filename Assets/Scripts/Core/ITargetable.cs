using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    Transform targetTransform
    {
        get;
    }

    Vector3 targetPos
    {
        get;
    }

    Health targetHealth
    {
        get;
    }

    Vector3 focusPoint
    {
        get;
        set;
    }
}
