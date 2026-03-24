using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public PlayerInventoryUI ui;
    public bool Stone1State;
    public bool Stone2State;
    public bool Stone3State;
    public Dictionary<IItem, int> inventoryItems;

    //Based on item ID
    public Sprite[] itemSprites;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        inventoryItems = new Dictionary<IItem, int>(new ItemEqualityComparer());
        Stone1State = true;
        Stone2State = true;
        Stone3State = true;
    }

    public void AddItem(IItem item, int nbr = 1)
    {
        for (int i = 0; i < nbr; i++) 
        {
            if (inventoryItems.ContainsKey(item))
            {
                inventoryItems[item]++;
            }
            else
            {
                inventoryItems.Add(item, 1);
            }
        }
        PauseUIManager.instance.UpdateInvItemsUI();
    }

    public void AddItem(int itemID, int nbr = 1)
    {
        IItem item = null;
        switch (itemID)
        {
            case 1:
                //Heal potion
                item = new PotionSoin();
                break;
            case 2:
                //Mana potion
                item = new PotionMana();
                break;
            default:
                //Nothing
                break;
        }

        for (int i = 0; i < nbr; i++)
        {
            try
            {
                if (inventoryItems.ContainsKey(item))
                {
                    inventoryItems[item]++;
                }
                else
                {
                    inventoryItems.Add(item, 1);
                }
            } catch {
                Debug.LogError("Error adding item to inventory by ID");
            }
            
        }
        PauseUIManager.instance.UpdateInvItemsUI();
    }

    public void UseItem(IItem item, Character target)
    {
        inventoryItems[item]--;
        item.Use(target);
        PauseUIManager.instance.UpdateInvItemsUI();
    }

    public IItem GetItemById(int id)
    {
        foreach(IItem item in inventoryItems.Keys)
        {
            if (item.ItemID == id) return item;
        }
        return null;
    }

}
