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
    private Font bark;

    void Start () {
        //get the player interactions component
        pi = gameObject.GetComponent<PlayerInteractions>();
        bark = GetComponent<Player>().bark;
	}
	
	void Update () {
        //get the plaeyr interactions component
        pi = gameObject.GetComponent<PlayerInteractions>();
	}

    void OnGUI()
    {
        GUI.skin.font = bark;
        GUI.skin.box.alignment = TextAnchor.LowerCenter;
        //check if we are in the tutorial
        if (tutorial && !this.GetComponent<PlayerInteractions>().notUseable)
        {
            //set color and background color
            GUI.color = Color.white;
            GUI.backgroundColor = Color.clear;

            //check if we are at the second interaction and are not hovering over anything
            if (interaction == 2 && !pi.canHover)
            {
                //this.GetComponent<Player>().flashFade();
                //show GUI box to display to open the GUI
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30),
                "Press 'Q' to open user & close interface.");
                //once the GUI is opened, switch the interaction so the GUI instructions no longer display
                if (Input.GetKeyDown(KeyCode.Q)) { interaction = 3; }
            }

            //check if we are hovering and the active target isn't null
            else if (pi.canHover && pi.activeTarget != null)
            {
                string targetName = pi.activeTarget.name;
                string improvedName = "";
                for (int i = 0; i < targetName.Length; i++)
                {
                    if (char.IsUpper(targetName[i]) && i != 0)
                    {
                        improvedName += " ";
                    }
                    improvedName += targetName[i];
                }
                //check if the active target is a pick up
                if (pi.activeTarget.tag == "PickUp")
                {
                   // this.GetComponent<Player>().flashFade();
                    //Display item name with instructions
                    GUI.Box(new Rect(0, Screen.height - Screen.height/2 + 150, Screen.width, 30),
                    "Press 'E' to pick up the " + improvedName);
                }
                //check if the active target is a useable object
                else if (pi.activeTarget.tag == "Useable")
                {
                    //this.GetComponent<Player>().flashFade();
                    //Display item name with instructions
                    GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30),
                    "Press 'E' to use the " + improvedName);
                }
                //check if the active target is a consumable object
                else if (pi.activeTarget.tag == "Consumable")
                {
                    //this.GetComponent<Player>().flashFade();
                    //Display item name with instructions
                    GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30),
                    "Press 'E' to consume the " + improvedName);
                }
                //after we have interaction with an object one, switch the interaction to stage 2
                if (interaction == 1 && Input.GetKeyDown(KeyCode.E))
                    interaction = 2;
            }
            //check if the interaction is 0
            if (interaction == 0 && !pi.canHover)
            {
                //start the display timer
                startTimer = startTimer - Time.deltaTime;
                //flash the text
                //this.GetComponent<Player>().flashFade();
                //display instructions
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2+100, Screen.width, 150),
                "Use 'WASD' to move, use the 'MOUSE' to look. \nUse 'SHIFT' to sprint. Use 'CTRL' to crouch \nUse 'R' to close your eyes, use 'F' to hold them open. \n \n *Headphones are recommended for full immersion.*");


                //once timer reaches 0, switch interaction to one
                if (startTimer < 0) { interaction = 1; }
            }
        }
    }

   

}
