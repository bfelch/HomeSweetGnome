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

    private string[] tutorialText;

    void Start () {
        //get the player interactions component
        pi = gameObject.GetComponent<PlayerInteractions>();
        bark = GetComponent<Player>().bark;

        tutorialText = new string[8];

        tutorialText[0] = "[WASD] will make you move. The [MOUSE] will adjust your gaze.";
        tutorialText[1] = "[R] will close your eyes. [F] will hold your eyes open.";
        tutorialText[2] = "[SHIFT] will make you run.";
        tutorialText[3] = "[Q] will open your inventory and display your health.";
        tutorialText[4] = "[ESC] will pause the game and allow you to quit, switch your settings, and view controls.";
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
        if (tutorial && !this.GetComponent<PlayerInteractions>().notUseable && !scrBed.cantsleep)
        {
            //set color and background color
            GUI.color = Color.white;
            GUI.backgroundColor = Color.clear;

            //check if we are hovering and the active target isn't null
            if (pi.canHover && pi.activeTarget != null)
            {
                string targetName = pi.activeTarget.name;
                string improvedName = "";
                for (int i = 0; i < targetName.Length; i++)
                {
                    if ((char.IsUpper(targetName[i]) || char.IsNumber(targetName[i])) && i != 0)
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
                    "[E] or [LEFT CLICK] will pick up the " + improvedName + ".");
                }
                //check if the active target is a useable object
                else if (pi.activeTarget.tag == "Useable" && !scrBed.resting)
                {
                    //this.GetComponent<Player>().flashFade();
                    //Display item name with instructions
                    GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30),
                    "[E] or [LEFT CLICK] will interact with the " + improvedName + ".");
                }
                //check if the active target is a consumable object
                else if (pi.activeTarget.tag == "Consumable")
                {
                    //this.GetComponent<Player>().flashFade();
                    //Display item name with instructions
                    GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30),
                    "[E] or [LEFT CLICK] will consume the " + improvedName + ".");
                }
            }
            //check if the interaction is 0
            if (!pi.canHover && interaction < 5)
            {
                //start the display timer
                startTimer = startTimer - Time.deltaTime;
                //flash the text
                //this.GetComponent<Player>().flashFade();
                //display instructions
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2+150, Screen.width, 30),
                tutorialText[interaction]);
                /*"WASD will make you move. The MOUSE will shift your gaze. \n SHIFT will make you run.  \n Your eyes will hold open with F and they will close with R. \n *Headphones are recommended for full immersion.*");*/
                /*"Use 'WASD' to move, use the 'MOUSE' to look. \nUse 'SHIFT' to sprint. \nUse 'R' to close your eyes, use 'F' to hold them open. \n \n *Headphones are recommended for full immersion.*");*/


                //once timer reaches 0, switch interaction to one
                if (startTimer < 0) { interaction++; startTimer = 15; }
            }
        }
    }

   

}
