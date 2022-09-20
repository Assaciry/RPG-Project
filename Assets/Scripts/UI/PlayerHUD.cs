using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    Health health;
    Transform healthContainer;
    Transform manaContainer;
    Transform xpContainer;

    void Awake()
    {
        health = GetComponentInParent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;

        healthContainer = transform.Find("PlayerStats").Find("HP").Find("HPContainer");
    }

    private void Health_OnHealthChanged(object sender, float e)
    {
        healthContainer.localScale = new Vector3(e, 1, 1);
    }
}
