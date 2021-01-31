using UnityEngine;
using RPG.Combat;

public class EnemyAI : MonoBehaviour
{
    CharacterFighter fighter;

    private void Start()
    {
        fighter = GetComponent<CharacterFighter>();
    }

    public void AttackRangeEnter(MonoBehaviour player)
    {
        fighter.AttackToTarget(player);
    }
}
