using UnityEngine;

public class BuyApplesPack : MonoBehaviour
{
    public PurchaseFX explosionFX;
    public ScoreManager scoreManager;
    public void Reward10Apples()
    {
        int currentGold = PlayerPrefs.GetInt("GoldApples", 0);
        currentGold += 10;
        PlayerPrefs.SetInt("GoldApples", currentGold);
        PlayerPrefs.Save();
        if (scoreManager != null) scoreManager.UpdateVisuals();
        if (explosionFX != null)
        {
            explosionFX.PlayEffect();
        }
    }
}