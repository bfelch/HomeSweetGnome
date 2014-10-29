using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {
    public GameObject heldObject;
    public bool hasItem;
    public List<GameObject> restrictedItems;

	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsRestrictedItem(GameObject checkItem){
        foreach (GameObject item in restrictedItems){
            if (checkItem.Equals(item)) {
                return true;
            }
        }

        return false;
    }

    public void GiveItem(GameObject item) {
        heldObject = item;
        item.transform.parent = this.gameObject.transform.parent;
        item.transform.position = this.transform.position;
        hasItem = true;
    }
}
