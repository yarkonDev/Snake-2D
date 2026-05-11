using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilitySlow : MonoBehaviour
{
    [Header("Ссылки")]
    public SnakeMovement snake;
    public AbilitySpeed speedAbility;
    public GameObject slowIcon;
    public Image durationBar;
    public Image cooldownDim;
    public GameObject slowEffect;

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
        _isUnlocked = PlayerPrefs.GetInt("SlowAbility", 0) == 1;

        if (slowIcon) slowIcon.SetActive(_isUnlocked);
        ResetVisuals();
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.E)) Activate();
    }

    public void Activate()
    {
        if (speedAbility != null) speedAbility.ForceStop();
        _activeRoutine = StartCoroutine(SlowRoutine());
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

    IEnumerator SlowRoutine()
    {
        _isActive = true;
        _canUse = false;

        if (slowEffect) slowEffect.SetActive(true);
        if (durationBar) { durationBar.gameObject.SetActive(true); durationBar.fillAmount = 1f; }

        snake.speed = _normalSpeed * 1.75f;
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
        if (slowEffect) slowEffect.SetActive(false);
        if (durationBar) durationBar.gameObject.SetActive(false);
    }
}