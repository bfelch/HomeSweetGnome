using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    //is this item a key
    public bool isKey;
    //stored name
    public string name;

	// Use this for initialization
	void Start () {
        name = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
