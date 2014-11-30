using UnityEngine;
using System.Collections;

public class ItemSlot : MonoBehaviour {
    //reference to held item
    public Item heldItem;
    //reference to the gui slot
    public GUISlot gui;

	// Use this for initialization
	void Start () {
        heldItem = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*void OnGUI()
    {
        if (gui.hovering)
        {
            //if hovering, draw name on mouse position
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
            if (heldItem != null)
            {
                GUI.Box(box, heldItem.name);

                if (Input.GetMouseButtonUp(1)) {
                    heldItem.gameObject.SetActive(true);

                    Vector3 pos = transform.position;

                    heldItem.transform.position = new Vector3(pos.x, pos.y - 2.5f, pos.z);

                    heldItem = null;
                }
            }
            else
            {
                GUI.Box(box, "Empty");
            }
        }
    }
    */

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
}
