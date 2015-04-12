using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyRing : MonoBehaviour {
    //reference to keys items in game
    public List<Item> keys;
    //reference to keys on ring
    public List<KeyItem> keysOnRing;
    //reference to gui slot
    public GUISlot gui;

    private int debugkeys;

	// Use this for initialization
	void Start () {
        //activate keys on ring if player has them
        foreach (KeyItem key in keysOnRing) {
            key.gameObject.SetActive(false);
        }

        debugkeys = 0;
	}
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetKeyUp(KeyCode.K) && debugkeys < keysOnRing.Count) {
            keysOnRing[debugkeys++].gameObject.SetActive(true);
        }*/
	}

    /*
    void OnGUI()
    {
        //show key count
        if (gui.hovering)
        {
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
            GUI.Box(box, "Keys: " + keys.Count);
        }
    }
    */

    public void AddKey(Item key)
    {
        //activate key on pickup
        foreach (KeyItem keyOnRing in keysOnRing) 
		{
            if (keyOnRing.key.Equals(key)) 
			{
                keyOnRing.gameObject.SetActive(true);
            }
        }
        keys.Add(key);
    }
	public void RemoveKey(Item key)
	{
		//activate key on pickup
		foreach (KeyItem keyOnRing in keysOnRing) {
			if (keyOnRing.key.Equals(key)) {
				keyOnRing.gameObject.SetActive(false);
			}
		}
		keys.Remove (key);
	}
}
