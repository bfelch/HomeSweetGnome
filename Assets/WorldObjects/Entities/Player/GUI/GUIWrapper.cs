using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIWrapper : MonoBehaviour 
{
    //references to all slots
    public ItemSlot[] slots;
    //reference to key ring
    public KeyRing keyRing;
    //reference to energy bar
    public EnergyBar energyBar;

	//Gui Materials
	public Material shovelMat;
	public Material boneMat;
	public Material saplingMat;
	public Material eyeMat;
	public Material fuelMat;
	public Material sparkMat;
	public Material screwMat;
	public Material headMat;
	public Material boatKeysMat;
	public Material defaultMat;

	//Gui Objects
	public GameObject objTopLeft;
	public GameObject objTopCenter;
	public GameObject objTopRight;
	public GameObject objBotLeft;
	public GameObject objBotRight;

    public bool AddToSlot(Item item)
    {
        //add key to key ring
        if (item.isKey)
        {
			GameObject.Find("Player").GetComponent<Player>().keyPickUpSound.Play();
            keyRing.AddKey(item);
            return true;
        }
        else
        {
            //add item to first empty slot
            foreach(ItemSlot slot in slots)
			{
                //if added, stop looking
                if(slot.AddItem(item))
                {
					switch(item.name)
					{
						case "Shovel":
							slot.renderer.material = shovelMat;
							break;
						case "Bone":
							slot.renderer.material = boneMat;
							break;
						case "SmallSapling":
							slot.renderer.material = saplingMat;
							break;
						case "GnomeEye":
							slot.renderer.material = eyeMat;
							break;
						case "GargoyleHead":
							slot.renderer.material = headMat;
							break;
						case "Fuel":
							slot.renderer.material = fuelMat;
							break;
						case "SparkPlug":
							slot.renderer.material = sparkMat;
							break;
						case "Screwdriver":
							slot.renderer.material = screwMat;
							break;
						case "BoatKeys":
							slot.renderer.material = boatKeysMat;
							break;
					}
					
					GameObject.Find("Player").GetComponent<Player>().itemPickUpSound.Play();
                    return true;
                }
            }
        }

        return false;
    }

	public bool RemoveFromSlot(Item item)
	{
		//add key to key ring
		if(item.isKey == false)
		{
			//remove item
			foreach(ItemSlot slot in slots)
			{
				//if removed, stop looking
				if(slot.RemoveItem(item))
				{
					slot.renderer.material = defaultMat;

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
            if(items.Contains(slot.heldItem))
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
