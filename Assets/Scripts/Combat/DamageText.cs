using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color color;

    private float maxLifetime;
    private float damageAmount;
    private float lifeTime = 0;
    private Vector3 movement;

    private Camera mainCamera;

    public void Initialize(float maxLifetime, float damageAmount, float fontSize)
    {
        mainCamera = Camera.main;

        text = GetComponentInChildren<TextMeshProUGUI>();
        color = text.color;

        text.fontSize = fontSize;
        SetText(damageAmount.ToString("F0"));
        this.maxLifetime = maxLifetime;

        movement = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0, 1.5f), Random.Range(-0.2f, 0.2f)) * 2;

        StartCoroutine(AnimateDamageText());
    }

    private IEnumerator AnimateDamageText()
    {
        while(lifeTime < maxLifetime)
        {
            transform.forward = mainCamera.transform.forward;

            AnimateText();

            lifeTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    private void AnimateText()
    {
        text.color = new Color(color.r, color.g, color.b, GetLifetimeNormalized());
        transform.position += movement * Time.deltaTime;
        text.fontSize = GetLifetimeNormalized() * 0.5f;
    }

    private float GetLifetimeNormalized()
    {
        return 1 - Mathf.Clamp01(lifeTime / maxLifetime);
    }

    private void SetText(string t)
    {
        text.text = t;
    }
}
