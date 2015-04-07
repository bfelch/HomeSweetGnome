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
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public bool AddItem(Item item)
    {
        //if no item in slot, add it
        if (heldItem == null)
        {
            heldItem = item;
			Debug.Log(heldItem);
			Debug.Log(gui.name);
            return true;
        }

        return false;
    }
}
