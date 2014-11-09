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

	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsRestrictedItem(GameObject checkItem){
        //checks that item is not in restricted list
        foreach (GameObject item in restrictedItems){
            if (checkItem.Equals(item)) {
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
}
