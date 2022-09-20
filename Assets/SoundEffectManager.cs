using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> damageClips;
    [SerializeField] AudioClip deathClip;
    AudioSource audioSource;
    Health health;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        health = GetComponentInParent<Health>();
        health.OnTookDamage += Health_OnTookDamage;
        health.OnCharacterDeath += Health_OnCharacterDeath;
    }

    private void Health_OnCharacterDeath(object sender, System.EventArgs e)
    {
        audioSource.PlayOneShot(deathClip);
    }

    private void Health_OnTookDamage(object sender, float e)
    {
        int randomIndex = Random.Range(0, damageClips.Count - 1);
        audioSource.PlayOneShot(damageClips[randomIndex]);
    }
}
