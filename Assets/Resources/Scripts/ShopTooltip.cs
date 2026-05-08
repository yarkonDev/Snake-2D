using UnityEngine;
using TMPro;

public class ShopTooltip : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI descriptionText; 
    public string message;

    public void ShowInfo()
    {
        if (tooltipPanel.activeSelf && descriptionText.text == message)
        {
            tooltipPanel.SetActive(false);
        }
        else
        {
            descriptionText.text = message;
            tooltipPanel.SetActive(true);
            tooltipPanel.transform.position = new Vector3(450, 100, 0);
        }
    }
}