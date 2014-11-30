using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {
    //reference to the energy bar
    public GUISlot gui;
    //reference to player
    public Player player;
    //reference to meter
    public GameObject sanityMeter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //checks if object is active
        if (this.gameObject.activeSelf) {
            //alters scale relative to health
            sanityMeter.transform.localScale = new Vector3(.9f, .95f * player.sanity / player.maxSanity, 1);

            //alters color relative to health
            float sanity = player.sanity / player.maxSanity;
            if (sanity > .6f) {
                sanityMeter.renderer.material.color = Color.green;
            } else if (sanity > .3f) {
                sanityMeter.renderer.material.color = Color.yellow;
            } else {
                sanityMeter.renderer.material.color = Color.red;
            }
        }
	}

    /*
    void OnGUI() {
        //displays remaining energy and max energy
        if (gui.hovering) {
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 150, 30);
            GUI.Box(box, "Energy: " + (int)player.sanity + " / " + (int)player.maxSanity);
        }
    }
    */
}
