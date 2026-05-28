using UnityEngine;
using TMPro;

public class ShopTooltip : MonoBehaviour
{
    public GameObject tooltipPanel;
    public GameObject overlay;
    public TextMeshProUGUI descriptionText; 
    public string message;

    public void ShowInfo()
    {
        if (tooltipPanel.activeSelf && descriptionText.text == message)
        {
            tooltipPanel.SetActive(false);
            overlay.SetActive(false);
        }
        else
        {
            descriptionText.text = message;
            tooltipPanel.SetActive(true);
            tooltipPanel.transform.position = new Vector3(800, 200, 0);
            overlay.SetActive(true);
        }
    }
    public void CloseInfo()
    {
        tooltipPanel.SetActive(false);
        overlay.SetActive(false);
    }
}