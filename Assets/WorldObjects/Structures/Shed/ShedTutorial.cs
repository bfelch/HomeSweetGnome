using UnityEngine;
using System.Collections;

public class ShedTutorial : MonoBehaviour {
    //player interaction object
    private PlayerInteractions pi;
    //holds what stage we are in the tutorial
    private int interaction = 0;
    //timer for first instructions to display
    private double startTimer = 15;
    //are we in the tutorial?
    public bool tutorial = true;


	void Start () {
        //get the player interactions component
        pi = gameObject.GetComponent<PlayerInteractions>();
	}
	
	void Update () {
        //get the plaeyr interactions component
        pi = gameObject.GetComponent<PlayerInteractions>();
	}

    void OnGUI()
    {
        //check if we are in the tutorial
        if (tutorial)
        {
            //set color and background color
            GUI.color = Color.white;
            GUI.backgroundColor = Color.black;

            //check if we are at the second interaction and are not hovering over anything
            if (interaction == 2 && !pi.canHover)
            {
                //show GUI box to display to open the GUI
                GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                "Press 'Q' to open user & close interface.");
                //once the GUI is opened, switch the interaction so the GUI instructions no longer display
                if (Input.GetKeyDown(KeyCode.Q)) { interaction = 3; }
            }

            //check if we are hovering and the active target isn't null
            else if (pi.canHover && pi.activeTarget != null)
            {
                //check if the active target is a pick up
                if (pi.activeTarget.tag == "PickUp")
                {
                    //Display item name with instructions
                    GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                    "Press 'E' to pick up the " + pi.activeTarget.name);
                }
                //check if the active target is a useable object
                else if (pi.activeTarget.tag == "Useable")
                {
                    //Display item name with instructions
                    GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                    "Press 'E' to use the " + pi.activeTarget.name);
                }
                //check if the active target is a consumable object
                else if (pi.activeTarget.tag == "Consumable")
                {
                    //Display item name with instructions
                    GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 30),
                    "Press 'E' to consume the " + pi.activeTarget.name);
                }
                //after we have interaction with an object one, switch the interaction to stage 2
                if (interaction == 1 && Input.GetKeyDown(KeyCode.E))
                    interaction = 2;
            }
            //check if the interaction is 0
            if (interaction == 0)
            {
                //set GUI background color to be transparent
                GUI.backgroundColor = Color.clear;
                //start the display timer
                startTimer = startTimer - Time.deltaTime;
                //display instructions
                GUI.Box(new Rect(5, 5, 300, 40),
                "Use 'WASD' to move, use the 'MOUSE' to look. \n Use 'SHIFT' to sprint. Use 'CTRL' to crouch");

                //once timer reaches 0, switch interaction to one
                if (startTimer < 0) { interaction = 1; }
            }
        }
    }
}
