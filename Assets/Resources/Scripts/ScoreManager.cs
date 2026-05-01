using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTextMenu;
    public TextMeshProUGUI scoreTextGame;
    private int _score = 0;

    public void AddScore(int amount)
    {
        _score += amount;
        scoreTextMenu.text = _score.ToString();
        scoreTextGame.text = _score.ToString();
    }

    public void ResetScore()
    {
        _score = 0;
        scoreTextMenu.text = "0";
        scoreTextGame.text = "0";
    }
}