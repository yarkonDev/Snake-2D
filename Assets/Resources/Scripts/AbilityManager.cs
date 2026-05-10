using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    public SnakeMovement snake; 
    public GameObject speedIcon;
    public Image speedCooldownDim;
    public GameObject lightningEffect;

    [Header("Настройки способностей")]
    public float boostDuration = 5f;
    public float cooldownDuration = 3f;
    public float speedMultiplier = 1.3f;

    private float _normalSpeed;
    private bool _isSpeedUnlocked = false;
    private bool _canUseSpeed = true;
    private bool _isBoosting = false;

    void Awake()
    {
        _isSpeedUnlocked = PlayerPrefs.GetInt("SpeedUnlocked", 0) == 1;
    }

    void Start()
    {
        if (snake != null) _normalSpeed = snake.speed;

        if (speedIcon != null) speedIcon.SetActive(_isSpeedUnlocked);
        if (speedCooldownDim != null) speedCooldownDim.fillAmount = 0;
        if (lightningEffect != null) lightningEffect.SetActive(false);
    }

    void Update()
    {
        if (!_isSpeedUnlocked || !_canUseSpeed) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpeed();
        }
    }
    public void ActivateSpeed()
    {
        if (_canUseSpeed && _isSpeedUnlocked)
        {
            StartCoroutine(SpeedRoutine());
        }
    }

    IEnumerator SpeedRoutine()
    {
        _canUseSpeed = false;
        _isBoosting = true;

        if (lightningEffect != null) lightningEffect.SetActive(true);

        snake.speed = _normalSpeed / speedMultiplier;
        snake.UpdateSpeed();

        yield return new WaitForSeconds(boostDuration);

        snake.speed = _normalSpeed;
        snake.UpdateSpeed();

        _isBoosting = false;
        if (lightningEffect != null) lightningEffect.SetActive(false);

        float elapsed = 0;
        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            if (speedCooldownDim != null)
            {
                speedCooldownDim.fillAmount = 1 - (elapsed / cooldownDuration);
            }
            yield return null;
        }

        if (speedCooldownDim != null) speedCooldownDim.fillAmount = 0;
        _canUseSpeed = true;
    }
}