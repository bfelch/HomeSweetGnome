using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    public bool isKey;
    public string name;

	// Use this for initialization
	void Start () {
        name = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
