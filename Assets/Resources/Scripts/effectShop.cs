using UnityEngine;
using System.Collections;

public class effectShop : MonoBehaviour
{
    public string saveKey = "none";
    public float jumpHeight = 6000f;
    public float sideForce = 2000f;
    public float duration = 2.0f;

    public void OnButtonClick()
    {
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);
        StartCoroutine(DeathAnimation(Random.Range(0, 2) == 0 ? -1f : 1f));
    }

    IEnumerator DeathAnimation(float direction)
    {
        RectTransform rect = GetComponent<RectTransform>();
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);

        float vVelocity = jumpHeight;
        float hVelocity = sideForce * direction;
        float gravity = jumpHeight * 16f;
        float elapsed = 0;

        if (TryGetComponent<UnityEngine.UI.Button>(out var btn)) btn.interactable = false;

        while (gameObject != null && elapsed < duration)
        {
            float dt = Time.unscaledDeltaTime;

            vVelocity -= gravity * dt;
            rect.anchoredPosition += new Vector2(hVelocity * dt, vVelocity * dt);
            rect.Rotate(0, 0, -direction * 300 * dt);

            elapsed += dt;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        if(gameObject != null) gameObject.SetActive(false);
    }

}
