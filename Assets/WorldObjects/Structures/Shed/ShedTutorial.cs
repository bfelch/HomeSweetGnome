using UnityEngine;
using System.Collections;

public class ShedTutorial : MonoBehaviour {
    private PlayerInteractions pi;
    private int interaction = 0;
    private double startTimer = 15;
    public bool tutorial = true;


	// Use this for initialization
	void Start () {
        pi = gameObject.GetComponent<PlayerInteractions>();
	}
	
	// Update is called once per frame
	void Update () {
        pi = gameObject.GetComponent<PlayerInteractions>();
	}

    void OnGUI()
    {
        if (tutorial)
        {
            GUI.color = Color.white;
            GUI.backgroundColor = Color.black;

            if (interaction == 2 && !pi.canHover)
            {
                GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                "Press 'Q' to open user & close interface.");
                if (Input.GetKeyDown(KeyCode.Q)) { interaction = 3; }
            }

            else if (pi.canHover && pi.activeTarget != null && tutorial)
            {
                if (pi.activeTarget.tag == "PickUp")
                {
                    //Display item name
                    GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                    "Press 'E' to pick up the " + pi.activeTarget.name);
                }
                else if (pi.activeTarget.tag == "Useable")
                {
                    //Display item name
                    GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                    "Press 'E' to use the " + pi.activeTarget.name);
                }

                else if (pi.activeTarget.tag == "Consumable")
                {
                    //Display item name
                    GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                    "Press 'E' to consume the " + pi.activeTarget.name);
                }
                if (interaction == 1 && Input.GetKeyDown(KeyCode.E))
                    interaction = 2;
            }
            if (interaction == 0)
            {
                GUI.backgroundColor = Color.clear;
                startTimer = startTimer - Time.deltaTime;
                GUI.Box(new Rect(5, 5, 300, 40),
                "Use 'WASD' to move, use the 'MOUSE' to look. \n Use 'SHIFT' to sprint. Use 'CTRL' to crouch");

                if (startTimer < 0) { interaction = 1; }
            }
        }
    }
}
