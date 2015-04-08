using UnityEngine;
using System.Collections;

public class ItemSlot : MonoBehaviour 
{
    //reference to held item
    public Item heldItem;
    //reference to the gui slot
    public GUISlot gui;

	// Use this for initialization
	void Start () 
	{
        heldItem = null;
		PlayerInteractions.slotCounter++;

	}

    public bool AddItem(Item item)
    {
        //if no item in slot, add it
        if (heldItem == null)
        {
            heldItem = item;
            return true;
        }

        return false;
    }

	public bool RemoveItem(Item item)
	{
		//if no item in slot, add it
		if(heldItem == item)
		{
			heldItem = null;
			return true;
		}
		
		return false;
	}
}