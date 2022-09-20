using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField] public GameObject HitFX;

    private bool isHit = false;
    private float lifeTime = 0;

    private CharacterFighter attacker;
    private float maxLifetime;
    private float speed;
    private Transform targetTransform;
    float extraDamage;

    public void InitializeProjectile(CharacterFighter attacker, ITargetable target, float speed, float maxLifetime, float extraDamage)
    {
        this.speed = speed;
        this.maxLifetime = maxLifetime;
        this.attacker = attacker;
        this.extraDamage = extraDamage;

        //float yRotation = -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg + 90;
        //float xRotation = direction.y <= 0 ? Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg + 180 : Mathf.Atan2(direction.y, direction.z);

        //transform.eulerAngles = new Vector3(xRotation, yRotation);

        transform.LookAt(target.targetPos + target.focusPoint);
        StartCoroutine(NonLockedMove());
    }

    // Guided projectile initialization
    //public void InitializeProjectile(Transform targetTransform, float speed, float maxLifetime)
    //{
    //    this.targetTransform = targetTransform;
    //    this.speed = speed;
    //    this.maxLifetime = maxLifetime;
    //}

    void Update()
    {
        if (lifeTime > maxLifetime) { StopAllCoroutines(); Destroy(gameObject); }
        lifeTime += Time.deltaTime;
    }

    IEnumerator NonLockedMove()
    {
        while(!isHit)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITargetable targetable))
        {
            isHit = true;
            targetable.targetHealth.TakeDamage(attacker, extraDamage);

            if (HitFX != null) Instantiate(HitFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
