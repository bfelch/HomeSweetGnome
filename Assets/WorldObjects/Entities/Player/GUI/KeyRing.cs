using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyRing : MonoBehaviour {
    public List<Item> keys;
    public List<KeyItem> keysOnRing;
    public GUISlot gui;

	// Use this for initialization
	void Start () {
        foreach (KeyItem key in keysOnRing) {
            key.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (gui.hovering)
        {
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
            GUI.Box(box, "Keys: " + keys.Count);
        }
    }

    public void AddKey(Item key)
    {
        foreach (KeyItem keyOnRing in keysOnRing) {
            if (keyOnRing.key.Equals(key)) {
                keyOnRing.gameObject.SetActive(true);
            }
        }
        keys.Add(key);
    }
}
