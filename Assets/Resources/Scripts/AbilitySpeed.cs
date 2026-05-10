using UnityEngine;
using UnityEngine.UI;

public class AbilitySpeed : MonoBehaviour
{
    public SnakeMovement snake;
    public Image cooldownImage;
    public GameObject iconObject;

    private bool _isUnlocked = false;
    private bool _canUse = true;
    private float _cooldownTime = 3f;
    private float _duration = 5f;

    void Start()
    {
        _isUnlocked = PlayerPrefs.GetInt("SpeedUnlocked", 0) == 1;
        iconObject.SetActive(_isUnlocked);
        cooldownImage.fillAmount = 0;
    }

    void Update()
    {
        if (!_isUnlocked || !_canUse) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility();
        }
    }

    public void OnClick()
    {
        if (_isUnlocked && _canUse) UseAbility();
    }

    void UseAbility()
    {
        _canUse = false;
        Invoke(nameof(StartCooldown), _duration);
    }

    void StartCooldown()
    {
        _canUse = true;
    }
}