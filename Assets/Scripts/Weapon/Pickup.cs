using RPG.Combat;
using RPG.Controller;
using UnityEngine;

public class Pickup : MonoBehaviour, IRaycastable
{
    [SerializeField] private IWeaponSO weaponPickup;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ObjectPickup(other);
        }
    }

    private void ObjectPickup(Collider other)
    {
        other.GetComponent<CharacterFighter>().EquipWeapon(weaponPickup);
        Destroy(gameObject);
    }

    public bool HandleRaycast()
    {
        return true;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }
}
