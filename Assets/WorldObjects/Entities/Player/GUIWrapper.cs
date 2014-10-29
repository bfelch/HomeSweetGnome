using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIWrapper : MonoBehaviour {
    public ItemSlot[] slots;
    public KeyRing keyRing;
    public EnergyBar energyBar;

    void Start() {
    }

    void Update() {
    }

    public bool AddToSlot(Item item)
    {
        if (item.isKey)
        {
            keyRing.AddKey(item);
            return true;
        }
        else
        {
            foreach (ItemSlot slot in slots)
            {
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
        int numRequiredItems = items.Count;

        foreach (ItemSlot slot in slots)
        {
            if (items.Contains(slot.heldItem))
            {
                numRequiredItems--;
            }
        }

        foreach (Item key in keyRing.keys)
        {
            if (items.Contains(key))
            {
                numRequiredItems--;
            }
        }

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
