using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "BranchSO", menuName = "RPG Branches/BranchSO", order = 0)]
    public class BranchSO : ScriptableObject
    {
        [SerializeField] public string Name;

    }
}
