using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon ScriptableObject", menuName = "RPG Project/ProjectileSO", order = 0)]
public class ProjectileSO : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public float Damage;
    [SerializeField] public float Speed;
    [SerializeField] public float maxLifetime;
}
