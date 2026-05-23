using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PurchaseFX : MonoBehaviour
{
    private List<RectTransform> _particlesList = new List<RectTransform>();

    [Header("Звуковые эффекты")]
    public AudioSource audioSource;
    public AudioClip buySound;
    public AudioClip laughSound;

    void Awake()
    {
        foreach (RectTransform child in transform)
        {
            _particlesList.Add(child);
        }
    }

    public void PlayEffect()
    {
        Vector2 mousePosition = Input.mousePosition;

        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePosition,
                    canvas.worldCamera,
                    out Vector2 localPoint
                );
                rectTransform.anchoredPosition = localPoint;
            }
        }

        gameObject.SetActive(true);

        if (audioSource != null)
        {
            if (buySound != null) audioSource.PlayOneShot(buySound);

            if (laughSound != null) audioSource.PlayOneShot(laughSound);
        }
        int totalParticles = _particlesList.Count;
        for (int i = 0; i < totalParticles; i++)
        {
            if (_particlesList[i] != null)
            {
                float angle = i * (Mathf.PI * 2f) / totalParticles;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                StartCoroutine(FlyParticle(_particlesList[i], direction));
            }
        }
    }

    IEnumerator FlyParticle(RectTransform p, Vector2 dir)
    {
        Image img = p.GetComponent<Image>();
        p.anchoredPosition = Vector2.zero;
        if (img != null) img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);

        float duration = 0.6f;
        float elapsed = 0f;
        float speed = Random.Range(300f, 500f);

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;

            p.anchoredPosition += dir * speed * Time.unscaledDeltaTime;
            p.Rotate(0, 0, Random.Range(400f, 700f) * Time.unscaledDeltaTime);

            if (img != null)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f - progress);
            }

            yield return null;
        }

        if (img != null) img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }
}