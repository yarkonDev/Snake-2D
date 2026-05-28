using UnityEngine;
using UnityEngine.UI;

public class MuteButtonHandler : MonoBehaviour
{
    [Header("Спрайты для кнопки")]
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private Image _buttonImage;

    void Start()
    {
        _buttonImage = GetComponent<Image>();
        bool isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        UpdateVisuals(isMuted);
    }

    public void ClickMute()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMute();
            bool isMutedNow = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
            UpdateVisuals(isMutedNow);
        }
    }

    void UpdateVisuals(bool isMuted)
    {
        if (_buttonImage == null) return;
        if (isMuted)
        {
            if (musicOffSprite != null) _buttonImage.sprite = musicOffSprite;
        }
        else
        {
            if (musicOnSprite != null) _buttonImage.sprite = musicOnSprite;
        }
    }
}