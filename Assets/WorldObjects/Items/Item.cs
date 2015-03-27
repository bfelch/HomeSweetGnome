using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    //is this item a key
    public bool isKey;
    //stored name
    public string name;

    public ItemType type;

	// Use this for initialization
	void Start () 
	{
        name = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    void OnCollisionEnter(Collision col)
    {
		if(col.gameObject.name != "ItemHolder")
		{
        	this.GetComponent<Rigidbody>().isKinematic = true;
		}
    }
}

public enum ItemType { BOAT, ATTIC, GATE, MISC, NONE };
