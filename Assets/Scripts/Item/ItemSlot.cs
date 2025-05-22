using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;

        if (icon == null)
            Debug.LogError("icon is null in Clear()");
        else
            icon.gameObject.SetActive(false);

        if (quantityText == null)
            Debug.LogError("quatityText is null in Clear()");
        else
            quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
