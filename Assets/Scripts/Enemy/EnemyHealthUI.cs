using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    private Health health;
    private Transform healthContainer;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;
        health.OnCharacterDeath += Health_OnCharacterDeath;

        healthContainer = transform.Find("Canvas").Find("healthBarContainer");

        gameObject.SetActive(false);
    }

    private void Health_OnHealthChanged(object sender, float e)
    {
        gameObject.SetActive(true);

        healthContainer.localScale = new Vector3(e, 1, 1);
    }

    private void Health_OnCharacterDeath(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
