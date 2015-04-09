using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {
    //reference to object spawned in node
    public GameObject heldObject;
    //whether or not the node has an item
    public bool hasItem;
    //list of items that cannot spawn in this node
    public List<GameObject> restrictedItems;

    public bool boatItems;
    public bool atticItems;
    public bool gateItems;
    public bool miscItems;

	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsRestrictedItem(Item checkItem){
        //checks that item is not in restricted list
        foreach (GameObject item in restrictedItems){
            if ((checkItem.name).Equals(item.name)) {
                return true;
            }
        }

        return false;
    }

    public void GiveItem(GameObject item) {
        //sets held item to parameter item
        heldObject = item;
        //sets parent of parameter to this node
        item.transform.parent = this.gameObject.transform.parent;
        //sets position of parameter to this node
        item.transform.position = this.transform.position;
        hasItem = true;
    }

    public bool IsValidItem(Item item)
    {
        switch (item.type)
        {
            case ItemType.BOAT:
                return boatItems;
            case ItemType.ATTIC:
                return atticItems;
            case ItemType.GATE:
                return gateItems;
            case ItemType.MISC:
                return miscItems;
        }

        return false;
    }
}
