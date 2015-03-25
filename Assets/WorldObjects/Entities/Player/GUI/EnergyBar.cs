using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {
    //reference to the energy bar
    public GUISlot gui;
    //reference to player
    public Player player;
    //reference to meter
    public GameObject meterToScale;
	public GameObject sanityMeter;
	
	// Update is called once per frame
	void Update () 
	{
        //checks if object is active
        if (this.gameObject.activeSelf) 
		{
            //alters scale relative to health
            meterToScale.transform.localScale = new Vector3(1, 1 * player.sanity / player.maxSanity, 1);

            //alters color relative to health
            float sanity = player.sanity / player.maxSanity;

            if (sanity > .6f) 
			{
				sanityMeter.renderer.material.color = new Color32(0,255, 0, 30);
            } 
			else if (sanity > .3f) 
			{
				sanityMeter.renderer.material.color = new Color32(255,255, 0, 30);
            } 
			else 
			{
				sanityMeter.renderer.material.color = new Color32(255,0, 0, 30);
            }
        }
	}

    /*
    void OnGUI() {
        //displays remaining energy and max energy
        if (gui.hovering) 
		{
            Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 150, 30);
            GUI.Box(box, "Energy: " + (int)player.sanity + " / " + (int)player.maxSanity);
        }
    }
    */
}
