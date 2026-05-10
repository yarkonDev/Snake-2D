using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool _isPaused = false;
    public Image toggleImage;
    public Sprite falseSprite;
    public Sprite trueSprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        _isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private bool _isREnabled = false;

    void Start()
    {
        _isREnabled = PlayerPrefs.GetInt("REnabled", 0) == 1;
        UpdateToggleVisuals();
    }

    public void ToggleRKey()
    {
        _isREnabled = !_isREnabled;

        PlayerPrefs.SetInt("REnabled", _isREnabled ? 1 : 0);
        PlayerPrefs.Save();

        UpdateToggleVisuals();
    }

    private void UpdateToggleVisuals()
    {
        toggleImage.sprite = _isREnabled ? trueSprite : falseSprite;
    }

    public bool IsREnabled()
    {
        return _isREnabled;
    }
}