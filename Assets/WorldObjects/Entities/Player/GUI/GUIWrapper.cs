using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIWrapper : MonoBehaviour {
    //references to all slots
    public ItemSlot[] slots;
    //reference to key ring
    public KeyRing keyRing;
    //reference to energy bar
    public EnergyBar energyBar;

    void Start() {
    }

    void Update() {
    }

    public bool AddToSlot(Item item)
    {
        //add key to key ring
        if (item.isKey)
        {
            keyRing.AddKey(item);
            return true;
        }
        else
        {
            //add item to first empty slot
            foreach (ItemSlot slot in slots)
            {
                //if added, stop looking
                if (slot.AddItem(item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasItems(List<Item> items)
    {
        //number of required items
        int numRequiredItems = items.Count;

        //if any slot has a required item, decrease number of required items
        foreach (ItemSlot slot in slots)
        {
            if (items.Contains(slot.heldItem))
            {
                numRequiredItems--;
            }
        }

        //if any key is required, decrease number of required items
        foreach (Item key in keyRing.keys)
        {
            if (items.Contains(key))
            {
                numRequiredItems--;
            }
        }

        //if number of required items is <= 0, player has all required items
        if (numRequiredItems <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
