using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTextGame;
    public TextMeshProUGUI scoreTextMenu;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI goldApplesText;
    public TextMeshProUGUI goldApplesShopText;

    public GameObject Apple_Extra;

    private int _score = 0;

    void Awake() { UpdateVisuals(); }
    void Start() 
    {
        _score = 0; UpdateVisuals();
        bool isDoubleAppleBought = PlayerPrefs.GetInt("DoubleApple", 0) == 1;

        if (Apple_Extra != null)
        {
            Apple_Extra.SetActive(isDoubleAppleBought);
        }
    }

    public void AddScore(int amount)
    {
        _score += amount;
        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (_score > best)
        {
            PlayerPrefs.SetInt("BestScore", _score);
            PlayerPrefs.Save();
        }
        UpdateVisuals();
    }

    public void AddGoldApple()
    {
        int gold = PlayerPrefs.GetInt("GoldApples", 0) + 1;
        PlayerPrefs.SetInt("GoldApples", gold);
        PlayerPrefs.Save();
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (scoreTextMenu != null) scoreTextMenu.text = _score.ToString();
        if (scoreTextGame) scoreTextGame.text = _score.ToString();
        if (scoreTextMenu) scoreTextMenu.text = _score.ToString();

        if (goldApplesText) goldApplesText.text = PlayerPrefs.GetInt("GoldApples", 0).ToString();
        if (bestScoreText) bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        if (goldApplesShopText != null) goldApplesShopText.text = PlayerPrefs.GetInt("GoldApples", 0).ToString();

    }
}