using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeWrapper : MonoBehaviour 
{
    //list of all nodes in world
    public List<Node> nodeList;
    //list of items that spawn in nodes
    public List<Item> itemList;
    public bool newGame;

    private bool onlyOnce = true;

    
	// Use this for initialization
	void Start () 
	{
        //if starting new game, spawns items
        if (newGame) 
		{
            foreach (Item item in itemList)
            {
                if (item.type != ItemType.NONE)
                {
                    //for each item, find empty node that allows item
                    Node node;
                    do
                    {
                        int index = Random.Range(0, nodeList.Count);
                        node = nodeList[index];

                    } while (node.IsRestrictedItem(item) || node.hasItem || !node.IsValidItem(item) );

                    //set node's item
                    node.GiveItem(item.gameObject);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
     
}
