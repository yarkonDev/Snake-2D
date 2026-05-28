using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilitySpeed : MonoBehaviour
{
    [Header("Ссылки")]
    public SnakeMovement snake;
    public AbilitySlow slowAbility;
    public GameObject speedIcon;
    public Image durationBar;
    public Image cooldownDim;
    public GameObject lightningEffect;
    public GameObject gameOverPanel;
    public GameObject QoLText;

    [Header("Настройки")]
    public float duration = 5f;
    public float cooldown = 3f;

    private float _normalSpeed;
    private bool _isUnlocked;
    private bool _canUse = true;
    private bool _isActive = false;
    private float _cooldownTimer = 0f;
    private Coroutine _activeRoutine;

    void Start()
    {
        _normalSpeed = snake.speed;
        _isUnlocked = PlayerPrefs.GetInt("SpeedAbility", 0) == 1;

        if (speedIcon) speedIcon.SetActive(_isUnlocked);
        if (QoLText) QoLText.SetActive(_isUnlocked);
        ResetVisuals();
    }

    void Update()
    {
        if (snake == null || snake._isDead)
        {
            ForceStop();
            return;
        }
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if (cooldownDim) cooldownDim.fillAmount = _cooldownTimer / cooldown;
            _canUse = false;
        }
        else
        {
            if (cooldownDim != null) cooldownDim.fillAmount = 0;
            if (!_isActive) _canUse = true;
        }

        if (!_isUnlocked || !_canUse || _isActive) return;

        if (gameOverPanel.activeSelf == false) if (Input.GetKeyDown(KeyCode.E)) Activate();
    }

    public void Activate()
    {
        if (slowAbility != null) slowAbility.ForceStop();
        _activeRoutine = StartCoroutine(SpeedRoutine());
    }

    public void ForceStop()
    {
        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        if (_isActive) _cooldownTimer = cooldown;

        _isActive = false;
        ResetVisuals();

        snake.speed = _normalSpeed;
        snake.UpdateSpeed();
    }

    IEnumerator SpeedRoutine()
    {
        _isActive = true;
        _canUse = false;

        if (lightningEffect) lightningEffect.SetActive(true);
        if (durationBar) { durationBar.gameObject.SetActive(true); durationBar.fillAmount = 1f; }

        snake.speed = _normalSpeed / 1.3f;
        snake.UpdateSpeed();

        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (durationBar) durationBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        _cooldownTimer = cooldown;
        ForceStop();
    }

    void ResetVisuals()
    {
        if (lightningEffect) lightningEffect.SetActive(false);
        if (durationBar) durationBar.gameObject.SetActive(false);
    }
}