using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeWrapper : MonoBehaviour {
    public List<Node> nodeList;
    public List<Item> itemList;
    public bool newGame;

	// Use this for initialization
	void Start () {
        if (newGame) {
            foreach (Item item in itemList) {
                Node node;
                do {
                    int index = Random.Range(0, nodeList.Count);
                    node = nodeList[index];
                } while (node.hasItem || node.IsRestrictedItem(item.gameObject));

                node.GiveItem(item.gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
