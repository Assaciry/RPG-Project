using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    Vector3 targetPos
    {
        get;
    }

    Health targetHealth
    {
        get;
    }
}
