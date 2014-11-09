using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyItem : MonoBehaviour {
    //reference to key
    public Item key;
    private bool hovering;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnGUI() {
        //if hovering, show key name
        if (hovering) {
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
            GUI.Box(box, this.name);
        }
    }

    void OnMouseEnter() {
        ToggleKeyHover(true);
    }

    void OnMouseExit() {
        ToggleKeyHover(false);
    }

    void ToggleKeyHover(bool hovering) {
        this.hovering = hovering;

        //set material based on hover
        if (this.hovering) {
            this.renderer.material.color = Color.red;
        } else {
            this.renderer.material.color = Color.white;
        }
    }
}
