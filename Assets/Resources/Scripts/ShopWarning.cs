using UnityEngine;
using TMPro;
using System.Collections;

public class ShopWarning : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private int _AttemptsCount = 0;

    private string[] _funnyMessages = {
        "Иди нафарми золотых яблок!",
        "Тебе не хватает! :)",
        "Если что, кнопка выхода в углу",
        "Яблоки сами себя не соберут!",
        "Ты меня слышишь?)"
    };

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void ShowWarning()
    {
        if (this == null) return;
        _AttemptsCount++;
        StopAllCoroutines();
        gameObject.SetActive(true);

        if (_AttemptsCount <= 3)
        {
            _text.text = "Не хватает золотых яблок!";
        }
        else
        {
            _text.text = _funnyMessages[Random.Range(0, _funnyMessages.Length)];
        }

        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        gameObject.SetActive(true);
        Vector3 startPos = transform.localPosition;
        Color startColor = _text.color;

        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;

            transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one * 0.8f, progress);

            _text.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }

        gameObject.SetActive(false);
        transform.localPosition = startPos;
        transform.localScale = Vector3.one;
    }
}