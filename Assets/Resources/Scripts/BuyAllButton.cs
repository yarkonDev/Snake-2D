using UnityEngine;

public class BuyAllButton : MonoBehaviour
{
    public ShopAbility[] allItems;

    public GameObject price;
    public GameObject buyAllButtonObject;

    public PurchaseFX megaExplosionFX;

    void Start()
    {
        CheckIfAlreadyBoughtAll();
    }
    public void PurchaseEverything()
    {
        if (megaExplosionFX != null)
        {
            megaExplosionFX.PlayEffect();
        }
        if (buyAllButtonObject != null) buyAllButtonObject.SetActive(false);
        if (allItems != null)
        {
            foreach (ShopAbility item in allItems)
            {
                if (item != null)
                {
                    if (PlayerPrefs.GetInt(item.saveKey, 0) != 1)
                    {
                        PlayerPrefs.SetInt(item.saveKey, 1);
                        if (item.buyButton != null) item.buyButton.SetActive(false);
                        if (item.priceList != null) item.priceList.SetActive(false);
                        if (item.boughtButton != null) item.boughtButton.SetActive(true);
                    }
                }
            }
        }

        PlayerPrefs.Save();
        PlayerPrefs.SetInt("AllShopUnlocked", 1);
        PlayerPrefs.Save();
    }

    void CheckIfAlreadyBoughtAll()
    {
        if (PlayerPrefs.GetInt("AllShopUnlocked", 0) == 1)
        {
            if (buyAllButtonObject != null) buyAllButtonObject.SetActive(false);
            if (price != null) price.SetActive(false);
        }
    }
}