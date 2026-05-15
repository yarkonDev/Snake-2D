using UnityEngine;

public class ShopAbility : MonoBehaviour
{
    public ScoreManager scoreManager;
    public PurchaseFX explosionFX;
    public GameObject buyButton;
    public GameObject boughtButton;
    public GameObject priceList;
    public int price = 1;
    public string saveKey = "None";
    public ShopWarning warningScript;

    void Start()
    {
        if (PlayerPrefs.GetInt(saveKey, 0) == 1)
        {
            buyButton.SetActive(false);
            priceList.SetActive(false);
            boughtButton.SetActive(true);
        }
    }

    public void Purchase()
    {
        int currentGold = PlayerPrefs.GetInt("GoldApples", 0);

        if (currentGold >= price)
        {
            currentGold -= price;
            if (explosionFX != null) explosionFX.PlayEffect();
            PlayerPrefs.SetInt("GoldApples", currentGold);
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
            boughtButton.SetActive(true);
            scoreManager.UpdateVisuals();
        }
        else
        {
            warningScript.ShowWarning();
        }
    }
}