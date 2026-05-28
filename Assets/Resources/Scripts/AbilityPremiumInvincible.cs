using UnityEngine;

public class AbilityPremiumInvincible : MonoBehaviour
{
    [Header("Ссылки")]
    public SnakeMovement snake;
    public GameObject premiumIconObject;
    public GameObject QoLText;
    public GameObject visualEffect;

    private bool _isUnlocked;
    private bool _isActive = false;

    void Start()
    {
        _isUnlocked = PlayerPrefs.GetInt("PremiumInvincible", 0) == 1;

        if (premiumIconObject) premiumIconObject.SetActive(_isUnlocked);
        if (QoLText) QoLText.SetActive(_isUnlocked);
        if (visualEffect) visualEffect.SetActive(false);
    }

    void Update()
    {
        if (!_isUnlocked || snake == null || snake._isDead) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            TogglePremium();
        }
    }

    public void TogglePremium()
    {
        if (!_isUnlocked) return;

        _isActive = !_isActive;

        snake.isInvincible = _isActive;
        if (visualEffect != null) visualEffect.SetActive(_isActive);
    }
}