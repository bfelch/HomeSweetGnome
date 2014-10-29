using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {
    public GUISlot gui;
    public Player player;
    public GameObject sanityMeter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.activeSelf) {
            sanityMeter.transform.localScale = new Vector3(.9f, .95f * player.sanity / player.maxSanity, 1);

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

    void OnGUI() {
        if (gui.hovering) {
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 150, 30);
            GUI.Box(box, "Energy: " + (int)player.sanity + " / " + (int)player.maxSanity);
        }
    }
}
