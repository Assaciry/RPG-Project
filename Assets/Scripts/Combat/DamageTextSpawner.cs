using RPG.Combat;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damaTextPrefab;

    private void Awake()
    {
        GetComponentInParent<Health>().OnTookDamage += DamageTextSpawner_OnTookDamage; 
    }

    private void DamageTextSpawner_OnTookDamage(object sender, float e)
    {
        GameObject damageText = Instantiate(damaTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<DamageText>().Initialize(2f, e, 0.5f);
    }
}
