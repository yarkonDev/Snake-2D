using UnityEngine;

public class ShopEntranceExir : MonoBehaviour
{
    [Header("Магазин")]
    public GameObject shopPanel;
    public GameObject shopWarning;

    public void OpenShop()
    {
        shopPanel.SetActive(true); 
    }

    public void CloseShop()
    {
        shopWarning.SetActive(false);
        shopPanel.SetActive(false);
    }
}
