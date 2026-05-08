using UnityEngine;

public class ShopEntranceExir : MonoBehaviour
{
    [Header("Магазин")]
    public GameObject shopPanel;

    public void OpenShop()
    {
        shopPanel.SetActive(true); 
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
