using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerInventoryUI : MonoBehaviour
{
    public GameObject noItemsMessage;
    public TextMeshProUGUI itemUseDesc;
    public List<Image> heroesItemTargetSprites;
    public List<TextMeshProUGUI> heroesItemTargetMANATexts;
    public List<TextMeshProUGUI> heroesItemTargetHPTexts;

    public List<Image> itemUIImages;
    public List<GameObject> itemSlots;
    public List<TextMeshProUGUI> itemNames;
    public List<TextMeshProUGUI> itemQuantities;
    public List<Button> itemUseBtns;

    //public Dictionary<string, Sprite> attackIcons;

    //les sprites ont besoin d'avoir le même nom que leur class associée
    public Sprite[] attackSpriteList;

    int itemIDToUse;

    private void Start()
    {
        foreach (GameObject slot in itemSlots)
        {
            slot.SetActive(false);
        }
        foreach(int e in PlayerInventory.instance.inventoryItems.Values)
        {
            if (e > 0)
            {
                noItemsMessage.SetActive(false);
                return;
            }
        }
        noItemsMessage.SetActive(true);

    }

    public void SetItemToUse(int itemID)
    {
        itemIDToUse = itemID;
        itemUseDesc.text = $"\"{PlayerInventory.instance.GetItemById(itemID).ItemDesc}\"\r\nSelect the target :";
    }

    public void UseItemOnTarget(int heroPos)
    {
       PlayerInventory.instance.UseItem(PlayerInventory.instance.GetItemById(itemIDToUse), TeamClass.instance.heroTeam[heroPos]);
    }

    public void UpdateItems()
    {
        int index = 0;
        foreach(var pair in PlayerInventory.instance.inventoryItems)
        {
            itemSlots[index].SetActive(false);
            if (pair.Value > 0)
            {
                //Lui attribuer un slot
                itemSlots[index].SetActive(true);
                itemUIImages[index].sprite = PlayerInventory.instance.itemSprites[pair.Key.ItemID];
                itemNames[index].text = pair.Key.ItemName;
                itemQuantities[index].text = $"{pair.Value}x";

                //Ajouter un listener avec le bon ItemID en paramètre d'entrée
                itemUseBtns[index].onClick.AddListener(() => SetItemToUse(pair.Key.ItemID));

                if(noItemsMessage.activeSelf) noItemsMessage.SetActive(false);
                index++;
            }
        }
        if(index == 0)
        {
            noItemsMessage.SetActive(true);
        }
    }
}
