using UnityEngine;

public class ShopAbility : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject buyButton;
    public GameObject boughtButton;
    public int price = 3;
    public string saveKey = "SpeedUnlocked";
    public ShopWarning warningScript;

    void Start()
    {
        if (PlayerPrefs.GetInt(saveKey, 0) == 1)
        {
            buyButton.SetActive(false);
            boughtButton.SetActive(true);
        }
    }

    public void Purchase()
    {
        int currentGold = PlayerPrefs.GetInt("GoldApples", 0);

        if (currentGold >= price)
        {
            currentGold -= price;
            PlayerPrefs.SetInt("GoldApples", currentGold);
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
            buyButton.SetActive(false);
            boughtButton.SetActive(true);
            scoreManager.UpdateVisuals();
        }
        else
        {
            warningScript.ShowWarning();
        }
    }
}