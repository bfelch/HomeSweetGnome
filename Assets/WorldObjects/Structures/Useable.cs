using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Useable : MonoBehaviour 
{
    //door, dirt, gate, light, chadelier, elevator...
    public UseableType type;
    //list of items required to use this item
    public List<Item> requiredItems;
    //reference to player gui
    public GUIWrapper playerGUI;
	
    public bool activate = false;
    public static GUIWrapper gui;

	public bool chandReady = false;
	private bool chandOn = false; //For one time chandelier light event
	
	// Use this for initialization
	void Start () 
    {
        try
        {
            playerGUI = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI;
        }
        catch{}

		if(this.gameObject.name == "LeftGateLock" || this.gameObject.name == "RightGateLock")
		{
			requiredItems[0] = EndGames.allPickUps["GateKeyOne"].GetComponent<Item>();
			requiredItems[1] = EndGames.allPickUps["GateKeyTwo"].GetComponent<Item>();
		    requiredItems[2] = EndGames.allPickUps["GateKeyThree"].GetComponent<Item>();
			requiredItems[3] = EndGames.allPickUps["GateKeyFour"].GetComponent<Item>();
		}
		else if(this.gameObject.name == "MotorHatch")
		{
			requiredItems[0] = EndGames.allPickUps["BoatKeys"].GetComponent<Item>();
			requiredItems[1] = EndGames.allPickUps["Fuel"].GetComponent<Item>();
			requiredItems[2] = EndGames.allPickUps["SparkPlug"].GetComponent<Item>();
			requiredItems[3] = EndGames.allPickUps["Wrench"].GetComponent<Item>();
		}
		else if(this.gameObject.name == "MixingBowl")
		{
			requiredItems[0] = EndGames.allPickUps["SmallSapling"].GetComponent<Item>();
			requiredItems[1] = EndGames.allPickUps["Bone"].GetComponent<Item>();
			requiredItems[2] = EndGames.allPickUps["Bone"].GetComponent<Item>();
			requiredItems[3] = EndGames.allPickUps["Bone"].GetComponent<Item>();
		}
        else if (this.gameObject.name == "ShedDoor")
        {
            requiredItems[0] = EndGames.allPickUps["ShedKey"].GetComponent<Item>();
        }
		else
		{
			//do nothing
		}
	}

    public string Interact()
    {
        //checks that player has all required items
        if (playerGUI.HasItems(requiredItems))
        {
            if (type == UseableType.DOOR)
            {
                //opens door
                this.gameObject.GetComponent<DoorInteraction>().DoorKeyOpen();
            }
            else if (type == UseableType.DIRTTRAP)
            {
				//Dig the dirt
				this.gameObject.GetComponent<scrDirtTrap>().Dig();
            }
			else if (type == UseableType.GATE)
            {
                GameObject.Find("Player").GetComponent<Animation>().Play("GateEnding");
                GameObject.Find("FrontGate").GetComponent<Animation>().Play("OpenFrontGate");
                GameObject[] gnomes = GameObject.FindGameObjectsWithTag("Gnome");
                for (int i = 0; i < gnomes.Length; i++)
                {
                    gnomes[i].GetComponent<Gnome>().enabled = false;
                    gnomes[i].GetComponent<NavMeshAgent>().enabled = false;

                }
                //Run the escape function inside the Player script
            }
            else if (type == UseableType.BOAT)
            {
                GameObject player = GameObject.Find("Player");
                GameObject boat = GameObject.Find("Boat");
				GameObject motor = GameObject.Find("Motor");

				AudioSource sound;
				AudioClip boatMotorSound = GameObject.Find("BoatSounds").GetComponent<BoatSounds>().boatMotorSound.clip;

				sound = GameObject.Find("LightFlash").GetComponent<scrLightFlash>().PlayClipAt(boatMotorSound, motor.transform.position);
				StartCoroutine(SoundController.FadeAudio(12.0F, SoundController.Fade.Out, sound));

               // player.transform.parent = boat.transform;
               // player.GetComponent<Animation>().Play("BoatEnding");
                //boat.GetComponent<Animation>().Play("BoatEnding");
                //Run the escape function inside the Player script
                player.GetComponent<EndGames>().Escape();
                //for(int i = 0; i < EndGames.dockGnomes.Length; i++)
                //{
                    //EndGames.dockGnomes[i].SetActive(true);
                //}
            }
            else if(type == UseableType.ATTICBOWL)
            {
				GameObject player = GameObject.Find("Player");
				GameObject.Find("MixingBowl").GetComponent<AudioSource>().Play();

                //Run the escape function inside the Player script
                player.GetComponent<EndGames>().Experiment();
            }
            else if (type == UseableType.LIGHT)
            {
				if(this.gameObject.name == "ChandelierSwitch" && chandOn == false && chandReady == true)
				{
					chandOn = true;

					gameObject.GetComponent<Light>().enabled = true;
					GameObject.Find("gnomeTrapCircle").GetComponent<MeshRenderer>().enabled = true;

					//enable darkness scripted event
					GameObject.Find("Darkness").GetComponent<scrDarkness>().PrepareEvent();

					//Unhighlight Chand Switch
					GameObject chandSwitch = GameObject.Find("ChandSwitch");
					GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Unhighlight(chandSwitch);

				}
				else if(this.gameObject.name == "ShedLight")
				{
					gameObject.GetComponent<Light>().enabled = !gameObject.GetComponent<Light>().enabled;

					//Only flicker when turning on
					if(gameObject.GetComponent<Light>().enabled == true)
					{
						gameObject.GetComponent<scrFlicker>().oneTimeFlicker();
					}
					else
					{
						this.renderer.material = this.gameObject.GetComponent<scrFlicker>().matLightOff; //Switch material
						(GetComponent("Halo") as Behaviour).enabled = false; //Disable halo
					}
				}
				else if(this.gameObject.name != "ChandelierSwitch" && this.gameObject.name != "ShedLight" )
				{
					//Toggles light
					gameObject.GetComponent<Light>().enabled = !gameObject.GetComponent<Light>().enabled;
				}
            }
            else if (type == UseableType.DROPTRAP)
            {
				//Drop the trap
				transform.parent.Find("DropTrap").gameObject.GetComponent<scrDropTrap>().Drop();
            }
            else if (type == UseableType.ELEVATOR)
            {
                GameObject.Find("Elevator").GetComponent<elevatorStuff>().Activate();
            }
			else if (type == UseableType.BOOK)
			{
				if(this.gameObject.GetComponent<scrBook>().bookOpen == false)
				{
					this.gameObject.GetComponent<scrBook>().OpenBook();
				}
			}
            return "";
        }

        string items = "";
        for (int i = 0; i < requiredItems.Count; i++)
        {
            if(i == requiredItems.Count -1)
                items += requiredItems.ToArray()[i] + " ";
            else
                items += requiredItems.ToArray()[i] + " and ";

        }
        return items;
    }
}

public enum UseableType {DOOR, DIRTTRAP, GATE, LIGHT, DROPTRAP, ELEVATOR, ATTICBOWL, BOAT, BOOK};
