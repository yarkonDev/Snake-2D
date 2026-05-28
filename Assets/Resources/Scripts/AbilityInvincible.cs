using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityInvincible : MonoBehaviour
{
    [Header("Ссылки")]
    public SnakeMovement snake;
    public GameObject iconObject;
    public Image durationBar;
    public Image cooldownDim;
    public GameObject visualEffect;
    public GameObject QoLText;

    [Header("Настройки")]
    public float duration = 3f;
    public float cooldown = 6f;

    private bool _isUnlocked;
    private bool _canUse = true;
    private bool _isActive = false;
    private float _cooldownTimer = 0f;
    private Coroutine _activeRoutine;

    void Start()
    {
        _isUnlocked = PlayerPrefs.GetInt("NormalInvincible", 0) == 1;

        if (iconObject) iconObject.SetActive(_isUnlocked);
        if (QoLText) QoLText.SetActive(_isUnlocked);
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
            if (cooldownDim) cooldownDim.fillAmount = 0;
            if (!_isActive) _canUse = true;
        }

        if (!_isUnlocked || !_canUse || _isActive || snake == null || snake._isDead) return;
        if (Input.GetKeyDown(KeyCode.F)) Activate();
    }

    public void Activate()
    {
        if (_canUse) _activeRoutine = StartCoroutine(InvincibleRoutine());
    }

    public void ForceStop()
    {
        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        if (_isActive) _cooldownTimer = cooldown;

        _isActive = false;
        if (snake != null) snake.isInvincible = false;
        ResetVisuals();
    }

    IEnumerator InvincibleRoutine()
    {
        _isActive = true;
        _canUse = false;

        if (visualEffect) visualEffect.SetActive(true);
        if (durationBar) { durationBar.gameObject.SetActive(true); durationBar.fillAmount = 1f; }

        snake.isInvincible = true;

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
        if (visualEffect) visualEffect.SetActive(false);
        if (durationBar) durationBar.gameObject.SetActive(false);
    }
}