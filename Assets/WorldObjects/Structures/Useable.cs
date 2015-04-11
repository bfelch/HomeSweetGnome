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
	private static bool[] keys = {false, false, false, false};

	private scrHighlightController highlighter;

	private static bool hatchOpen = false;
	private static bool motorRepaired = false;
	private static bool fuelFilled = false;

	// Use this for initialization
	void Start () 
    {
        try
        {
            playerGUI = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI;
        }
        catch{}

		highlighter = GameObject.Find ("Highlighter").GetComponent<scrHighlightController> ();

        
		/*
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
			requiredItems[3] = EndGames.allPickUps["Screwdriver"].GetComponent<Item>();
		}
		else if(this.gameObject.name == "MixingBowl")
		{
			requiredItems[0] = EndGames.allPickUps["SmallSapling"].GetComponent<Item>();
			requiredItems[1] = EndGames.allPickUps["GnomeEye"].GetComponent<Item>();
			requiredItems[2] = EndGames.allPickUps["GargoyleHead"].GetComponent<Item>();
			requiredItems[3] = EndGames.allPickUps["Bone"].GetComponent<Item>();
		}
        else if (this.gameObject.name == "ShedDoor")
        {
            requiredItems[0] = EndGames.allPickUps["ShedKey"].GetComponent<Item>();
        }
		else
		{
			//do nothing
		}*/
	}

    void Update()
    {
        elevatorStuff.ElevatorDoors();
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
			else if(type == UseableType.GATEKEYONE)
			{
				keys[0] = true;
				highlighter.Unhighlight(this.gameObject);

			
				GameObject keyOne = Instantiate(Resources.Load("Keys/KeyOne"), Vector3.zero, Quaternion.identity) as GameObject;
                keyOne.GetComponent<AudioSource>().Play();

                //keyOne.transform.parent = GameObject.Find ("LeftGateLock").transform;
                keyOne.name = "KeyOne";
				keyOne.transform.localPosition = new Vector3 (-42.8533f, 12.008f, 132.1556f);
				keyOne.transform.localEulerAngles = new Vector3(0f, 255f, 90);
				keyOne.transform.localScale = new Vector3(.75f,.75f,.75f);

				playerGUI.keyRing.RemoveKey(this.requiredItems[0].GetComponent<Item>());
                TurnKeys.turnKeyOne = true;

			/*Script to put key in hole */
			}
			else if (type == UseableType.GATEKEYTWO)
			{
				keys[1] = true;
				highlighter.Unhighlight(this.gameObject);
				
				GameObject keyTwo = Instantiate(Resources.Load("Keys/KeyTwo"), Vector3.zero, Quaternion.identity) as GameObject;
                keyTwo.name = "KeyTwo";
                keyTwo.GetComponent<AudioSource>().Play();
                //keyTwo.transform.parent = GameObject.Find ("LeftGateLock").transform;
				keyTwo.transform.localPosition = new Vector3 (-42.60429f, 12.007f, 132.2303f);
				keyTwo.transform.localEulerAngles = new Vector3(0f, 255f, 90);
				keyTwo.transform.localScale = new Vector3(.75f,.75f,.75f);

				playerGUI.keyRing.RemoveKey(this.requiredItems[0].GetComponent<Item>());
                TurnKeys.turnKeyTwo = true;
			}
			else if (type == UseableType.GATEKEYTHREE)
			{
				keys[2] = true;
				highlighter.Unhighlight(this.gameObject);
				
				GameObject keyThree = Instantiate(Resources.Load("Keys/KeyThree"), Vector3.zero, Quaternion.identity) as GameObject;
                keyThree.GetComponent<AudioSource>().Play();
                keyThree.name = "KeyThree";
                keyThree.transform.localPosition = new Vector3 (-40.2736f, 12.0194f, 132.755f);
				keyThree.transform.localEulerAngles = new Vector3(0f, 258, 90);
				keyThree.transform.localScale = new Vector3(.75f,.75f,.75f);

				playerGUI.keyRing.RemoveKey(this.requiredItems[0].GetComponent<Item>());
                TurnKeys.turnKeyThree = true;
			}
			else if (type == UseableType.GATEKEYFOUR)
			{
				keys[3] = true;
				highlighter.Unhighlight(this.gameObject);
				
				GameObject keyFour = Instantiate(Resources.Load("Keys/KeyFour"), Vector3.zero, Quaternion.identity) as GameObject;
                keyFour.GetComponent<AudioSource>().Play();
                keyFour.name = "KeyFour";
				keyFour.transform.localPosition = new Vector3(-39.9951f, 12.0084f, 132.7619f);
				keyFour.transform.localEulerAngles = new Vector3(0f, 258, 90);
				keyFour.transform.localScale = new Vector3(.75f,.75f,.75f);

				playerGUI.keyRing.RemoveKey(this.requiredItems[0].GetComponent<Item>());
                TurnKeys.turnKeyFour = true;
			}
            else if (type == UseableType.B_HATCH)
            {
				if(!hatchOpen)
				{
					//Unhighlight hatch
					highlighter.Unhighlight(this.gameObject);

					//Play openHatch animation
					transform.parent.gameObject.animation.Play();

					//Highlight motor
					highlighter.Highlight(GameObject.Find("Motor"), scrHighlightController.outline2);

					hatchOpen = true;
					Debug.Log("Hatch Open: " + hatchOpen);
				}
			}
			else if (type == UseableType.B_MOTOR && hatchOpen)
			{
				if(!motorRepaired)
				{
					if(playerGUI.RemoveFromSlot(requiredItems[0]) && playerGUI.RemoveFromSlot(requiredItems[1]))
					{
						//Unhighlight motor
						highlighter.Unhighlight(this.gameObject);
						
						//Play fix sound

						//Highlight fuel cap
						highlighter.Highlight(GameObject.Find ("FuelCap"), scrHighlightController.outline2);
						
						motorRepaired = true;
					}
				}
			}
			else if (type == UseableType.B_FUEL && motorRepaired == true)
			{
				if(!fuelFilled)
				{
					if(playerGUI.RemoveFromSlot(requiredItems[0]))
					{
						//Unhighlight fuel
						highlighter.Unhighlight(this.gameObject);

						//Play fill sound

						//Highlight boat ignition
						highlighter.Highlight(GameObject.Find ("Ignition"), scrHighlightController.outline2);

						fuelFilled = true;
					}
				}
			}
			else if (type == UseableType.B_IGNITION && fuelFilled == true)
			{
				if(playerGUI.RemoveFromSlot(requiredItems[0]))
				{
					//Unhighlight ignition
					highlighter.Unhighlight(this.gameObject);

					GameObject player = GameObject.Find("Player");
					GameObject boat = GameObject.Find("Boat");
					GameObject motor = GameObject.Find("Motor");
					
					AudioSource sound;
					AudioClip boatMotorSound = GameObject.Find("BoatSounds").GetComponent<BoatSounds>().boatMotorSound.clip;
					
					sound = SoundController.PlayClipAt(boatMotorSound, motor.transform.position);
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
						highlighter.Unhighlight(chandSwitch);

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

					if(this.gameObject.name == "Torch")
					{
						gameObject.GetComponent<ParticleSystem>().enableEmission = !gameObject.GetComponent<ParticleSystem>().enableEmission;
					}
				}
            }
            else if (type == UseableType.DROPTRAP)
            {
				//Drop the trap
				transform.parent.Find("DropTrap").gameObject.GetComponent<scrDropTrap>().Drop();
            }
            else if (type == UseableType.ELEVATORTOP)
            {
                elevatorStuff.openTopElevator = true;
            }
            else if (type == UseableType.ELEVATORBOTTOM)
            {
                if(GameObject.Find("Elevator").transform.localPosition.y > 28)
                {
                    elevatorStuff.inElevator = true;
                    elevatorStuff.direction = true;
                    elevatorStuff.activate = true;
                    elevatorStuff.callingDown = true;
                }
                else
                    elevatorStuff.openBottomElevator = true;

            }
			else if (type == UseableType.BOOK)
			{
				if(scrBook.bookOpen == false)
				{
					this.gameObject.GetComponent<scrBook>().OpenBook();
				}
			}
			else if (type == UseableType.JOURNAL)
			{
				if(scrJournal.journalOpen == false)
				{
					this.gameObject.GetComponent<scrJournal>().OpenJournalPage();
				}
			}
			if(checkGateKeys())
			{
				GameObject.Find("Player").GetComponent<Animation>().Play("GateEnding");
				GameObject.Find("FrontGate").GetComponent<Animation>().Play("OpenFrontGate");
				GameObject[] gnomes = GameObject.FindGameObjectsWithTag("Gnome");
				GameObject.Find ("Player").GetComponent<PlayerMovement>().enabled = false;

                GameObject.Find("KeyOne").SetActive(false);
                GameObject.Find("KeyTwo").SetActive(false);
                GameObject.Find("KeyThree").SetActive(false);
                GameObject.Find("KeyFour").SetActive(false);

				for (int i = 0; i < gnomes.Length; i++)
				{
					gnomes[i].GetComponent<Gnome>().enabled = false;
					gnomes[i].GetComponent<NavMeshAgent>().enabled = false;
				}
				GameObject player = GameObject.Find("Player");

				player.GetComponent<EndGames>().Escape();
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

	public bool checkGateKeys()
	{
		for(int i = 0; i < keys.Length; i++)
		{
			if(keys[i] == false)
				return false;
		}
		return true;
	}
}

public enum UseableType {DOOR, DIRTTRAP, GATEKEYONE, GATEKEYTWO, GATEKEYTHREE, GATEKEYFOUR, LIGHT, DROPTRAP, ELEVATORTOP, ATTICBOWL, B_HATCH, B_MOTOR, B_FUEL, B_IGNITION, BOOK, JOURNAL, ELEVATORBOTTOM};
