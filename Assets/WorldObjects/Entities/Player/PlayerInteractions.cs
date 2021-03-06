﻿using UnityEngine;
using System.Collections;

public class PlayerInteractions : MonoBehaviour
{
	private Camera mainC;
    private HingeJoint doorHinge;
    private ShedTutorial st;
    private string GUIString;
    public GUISkin daSkin;
	public GUISkin daSkin2;
	public GUISkin daSkin3;
	public GUISkin daSkin4;

    //duration of play
    public float timePlayed;
    public static string playerName = "";

    public GameObject activeTarget; //The item being looked at
    public bool canHover = false; //Show the item name being look at?
    public bool notUseable = false;
    //player movement
    public CharacterMotor charMotor;
    //horizontal look
    public MouseLook mouseLook;
    //vertical look
    public MouseLook cameraLook;
    private GameObject lastActiveTarget = null;

    private Material outline;
    //is gui active
    public static bool showGUI;
    public bool removePlus = false;
    private bool lookingAtGnome = false;
    private bool lookingAtGargoyle = false;
	private bool lookingAtDirt = false;
    //reference to gui
    public GUIWrapper playerGUI;
    private float lightingValue = 0.05F;
    private float xSens=7;
    private float ySens=7;
    public Texture2D crosshair;

    private bool optionsMenu = false;
    public static bool pause;
    public static bool pauseMenu;

    public static bool delayPause = false;
    private Font bark;
    private EndGames endgame;

	public Material defaultMat;

	static public int slotCounter;

    public static bool displayWarningMsg = true;
    private bool waitToDisableWarningMsg = false;
    private bool waitForCollision = false;

	public static bool objectOpened = false;

    public float startTimer = 10f;

    void Start()
    {
        showGUI = false;
        pause = false;
        StartCoroutine(DelayedStart());

        
        if (PlayerPrefs.GetFloat("Lighting") != null || PlayerPrefs.GetFloat("Lighting") != 0)
        {
            GameObject.Find("ContrastLight").GetComponent<Light>().intensity = PlayerPrefs.GetFloat("Lighting");
            GameObject.Find("Player").GetComponent<MouseLook>().sensitivityX = PlayerPrefs.GetFloat("XSens");
            GameObject.Find("Main Camera").GetComponent<MouseLook>().sensitivityY = PlayerPrefs.GetFloat("YSens");

            lightingValue = PlayerPrefs.GetFloat("Lighting"); ;
            xSens = PlayerPrefs.GetFloat("XSens"); ;
            ySens = PlayerPrefs.GetFloat("YSens");
        }
        
        
    }

	public IEnumerator DelayedStart()
	{
		//Wait spawn time
		yield return new WaitForEndOfFrame();
		
		//sets gui start
		ToggleGUI(false);
		st = gameObject.GetComponent<ShedTutorial>();
		outline = Resources.Load("Outline2") as Material;
		bark = GetComponent<Player>().bark;
		endgame = GetComponent<EndGames>();
	}

    public IEnumerator DelayUnpause()
    {
        //Wait spawn time
        yield return new WaitForSeconds(1f);
        delayPause = false;
    }
    void Update()
    {
        itemAction();
        GUIControl();
        if(Input.GetKeyDown(KeyCode.Escape) && !this.gameObject.animation.IsPlaying("OpeningCut") && !endgame.playerEscaped && !endgame.playerSlept && !endgame.playerFell && !scrBed.resting)
        {
           if(!pause)
           { 
                pause = true;
                delayPause = true;
                pauseMenu = true;
                this.GetComponent<PlayerMovement>().enabled = false;
                this.GetComponent<Player>().enabled = false;
                ToggleGUI(false);
                Screen.lockCursor = false;
                Time.timeScale = 0.0f;
                mouseLook.enabled = false;
                cameraLook.enabled = false;
            }
           else
           {
               Screen.lockCursor = true;
               if (!scrBook.bookOpen && !scrJournal.journalOpen)
               {
                   this.GetComponent<PlayerMovement>().enabled = true;
                   this.GetComponent<Player>().enabled = true;
                   mouseLook.enabled = true;
                   cameraLook.enabled = true;
               }
               Time.timeScale = 1.0f;
               pause = false;
               StartCoroutine(DelayUnpause());
               optionsMenu = false;
           }
        }

        /*
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameObject.Find("TempPlayerLight").GetComponent<Light>().enabled = !GameObject.Find("TempPlayerLight").GetComponent<Light>().enabled;
        }
        */
        
        /*
		//CHEATS!
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			//Gate Teleport
			transform.position = new Vector3(-40F, 11.23F, 124.59F);
		}
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			//Dock Teleport
			transform.position = new Vector3(81.74F, -20.22F, 51.01F);
		}
		if(Input.GetKeyDown(KeyCode.Alpha8))
		{
			//Attic Teleport
			transform.position = new Vector3(3.46F, 50.04F, -22.84F);
		}
		if(Input.GetKeyDown(KeyCode.Alpha9))
		{
			//Tunnel Teleport
			transform.position = new Vector3(53.2F, -18.12F, -15.15F);
        } 
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            //Tunnel Teleport
            transform.position = new Vector3(57.45F, 26.58F, 19.13F);
        }
         * */
    }

    void OnGUI()
    {
        /*
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;
        GUI.skin.font = bark;
         * */
        GUI.skin = daSkin;
        if (pause && pauseMenu)
        {
            mouseLook.enabled = false;
            cameraLook.enabled = false;
            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 40, 300, Screen.height));
            //GUI.Box(new Rect(0, 0,200, 250), "");
            int second = (int)(timePlayed + Time.timeSinceLevelLoad);
            int minute = second / 60;
            int hour = minute / 60;
            second %= 60;
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;
            GUI.Box(new Rect(20, 0, 250, 124), "Time Played\n" + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00"));
            if (GUI.Button(new Rect(20, 130, 250, 50), "Resume"))
            {
                Screen.lockCursor = true;
                Time.timeScale = 1.0f;
                if (!scrBook.bookOpen && !scrJournal.journalOpen)
                {
                    this.GetComponent<PlayerMovement>().enabled = true;
                    this.GetComponent<Player>().enabled = true;
                    mouseLook.enabled = true;
                    cameraLook.enabled = true;
                }
                pause = false;
                StartCoroutine(DelayUnpause());

            }
			GUI.skin = daSkin2;

            if (GUI.Button(new Rect(20, 180, 250, 50), "Options"))
            {
                optionsMenu = true;
                pauseMenu = false;
            }
			GUI.skin = daSkin3;
            if (GUI.Button( new Rect(20, 230, 250, 50), "Main Menu"))
            {
                //LoadUnload.showEverything();
                this.GetComponent<SaveLoad>().Save(true);
            }
			GUI.skin = daSkin4;
            if (GUI.Button(new Rect(20, 280, 250, 50), "Quit"))
            {
                Application.Quit();
            }

            //layout end
            GUI.EndGroup();
        }
        if (optionsMenu && pause)
        {
            int sliderPos = 170;
            int controlPos = 0;

            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 20, 360, Screen.height));
            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(sliderPos, 90, 180, 30), "Adjust Lighting");
            GUI.backgroundColor = Color.white;
            lightingValue = GUI.HorizontalSlider(new Rect(sliderPos, 120, 180, 10), lightingValue, 0, 0.1f);
            GameObject.Find("ContrastLight").GetComponent<Light>().intensity = lightingValue;
            PlayerPrefs.SetFloat("Lighting", lightingValue);

            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(sliderPos, 130, 180, 30), "Adjust X Sensitivity");
            GUI.backgroundColor = Color.white;
            xSens = GUI.HorizontalSlider(new Rect(sliderPos, 160, 180, 10), xSens, 2, 12);
            GameObject.Find("Player").GetComponent<MouseLook>().sensitivityX = xSens;
            PlayerPrefs.SetFloat("XSens", xSens);

            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(sliderPos, 170, 180, 30), "Adjust Y Sensitivity");
            GUI.backgroundColor = Color.white;
            ySens = GUI.HorizontalSlider(new Rect(sliderPos, 200, 180, 10), ySens, 2, 12);
            GameObject.Find("Main Camera").GetComponent<MouseLook>().sensitivityY = ySens;
            PlayerPrefs.SetFloat("YSens", ySens);

            GUI.backgroundColor = Color.clear;
            GUI.skin.box.alignment = TextAnchor.MiddleLeft;

            GUI.Box(new Rect(controlPos, 90, 180, 25), "WASD - Move");
            GUI.Box(new Rect(controlPos, 110, 180, 25), "Q - Inventory");
            GUI.Box(new Rect(controlPos, 130, 180, 25), "E/Left Click - Interact");
            GUI.Box(new Rect(controlPos, 150, 180, 25), "R - Close Eyes");
            GUI.Box(new Rect(controlPos, 170, 180, 25), "F - Open Eyes");
            GUI.Box(new Rect(controlPos, 190, 180, 25), "Shift - Sprint");

            GUI.backgroundColor = Color.white;
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;

			GUI.skin = daSkin;
            if (GUI.Button(new Rect(50, 230, 250, 50), "Back"))
            {
                optionsMenu = false;
                pauseMenu = true;
            }

            //layout end
            GUI.EndGroup();
        }
        else if (showGUI)
        {
            GUI.skin = daSkin2;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = 1f;
            int playerUILayer = 11;
            int playerUIKeyLayer = 12;
            LayerMask layerMask = 1 << playerUILayer | 1 << playerUIKeyLayer;

            //layerMask = ~layerMask;

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                GUISlot guiSlot = hit.collider.gameObject.GetComponent<GUISlot>();
                KeyItem keyItem = hit.collider.gameObject.GetComponent<KeyItem>();

                if (keyItem) {
                    Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
                    GUI.Box(box, keyItem.name);
                } else {
                    if (guiSlot.isKeyRing) {
                        KeyRing ring = guiSlot.GetComponent<KeyRing>();

                        Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
                        GUI.Box(box, "Keys: " + ring.keys.Count);
                    } else if (guiSlot.isEnergyBar) {
                        EnergyBar bar = guiSlot.GetComponent<EnergyBar>();

                        Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 150, 30);
                        GUI.Box(box, "Energy: " + (int)bar.player.sanity + " / " + (int)bar.player.maxSanity);
                    } else {
                        ItemSlot slot = guiSlot.GetComponent<ItemSlot>();

                        Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 25);
                        if (slot.heldItem != null) {
                            box.width *= 1.5f;
                            box.height *= 2;
                            GUI.Box(box, getImprovedName(slot.heldItem.name) + "\nDrop (Right Click)");
						
                            if (Input.GetMouseButtonUp(1)) {

								GameObject.Find("Player").GetComponent<Player>().itemDropSound.Play();

                                slot.heldItem.gameObject.SetActive(true);

                                Vector3 pos = transform.position;

                                slot.heldItem.transform.position = pos + Camera.main.transform.forward;
                                slot.heldItem.gameObject.GetComponent<Rigidbody>().isKinematic = false;

								slot.renderer.material = defaultMat;
                                slot.heldItem = null;
                            }
                        } else {
                            GUI.Box(box, "Empty");
                        }
                    }
                }
            }
        }
        else
        {
            GUI.backgroundColor = Color.clear;
            if (canHover && activeTarget != null && !st.tutorial && !notUseable)
            {
                //Strip target name
                string targetName = activeTarget.name;
                //Display item name
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30), getImprovedName(targetName));
            }
            else if(lookingAtGnome
			        && Gnome.gnomeLevel == 1
			        && activeTarget.name != "GnomeShed"
			        && activeTarget.name != "DarkGnome(Clone)"
			        && activeTarget.GetComponent<Gnome>().fallen == false
			        && activeTarget.GetComponent<Gnome>().pushed == false)
            {
				if(displayWarningMsg)
				{
	                GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 50), "[E] or [LEFT CLICK] will push a gnome. \n WARNING: This will do you harm, but temporarily disable the gnome.");
	                waitToDisableWarningMsg = true;
				}
				else
				{
					GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 50), "Push");
				}
            }
            else if (lookingAtGargoyle)
            {
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 50), "[E] or [LEFT CLICK] will push the gargoyle.");

            }
            else if (notUseable && activeTarget != null)
            {
               // this.GetComponent<Player>().flashFade();

				GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30), "You need the " + GUIString + " to continue.");

				if(activeTarget.name == "Dirt")
				{
					lookingAtDirt = false;
				}
            }
			else if(lookingAtDirt)
			{
				GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 50), "Dig");
			}

            if (!showGUI && !removePlus)
            {
                GUI.Box(new Rect(Screen.width / 2-10, Screen.height / 2-10, 20, 20), "+");
            }



        }

        if (scrBed.cantsleep)
        {
            GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30),
                    "You are not tired enough to sleep.");

            startTimer = startTimer - Time.deltaTime;
            //once timer reaches 0, switch interaction to one
            if (startTimer < 0) { scrBed.cantsleep = false; startTimer = 10f; }
        }
    }

    void itemAction()
    {
		Transform cam = Camera.main.transform;

		//Player layer mask
		int playerLayer = 8;
		int invisibleLayer = 10;
		LayerMask ignoreMask = 1 << playerLayer | 1 << invisibleLayer;

        //Invert bitmask to only ignore this layer
        ignoreMask = ~ignoreMask;

		RaycastHit hit;
		Debug.DrawRay(cam.position, cam.forward * 5, Color.white);

        if(Physics.Raycast(cam.position, cam.forward, out hit, 5, ignoreMask))
        {
            activeTarget = hit.collider.gameObject; //Store item being looked at

            //Is the item close and a pick up?
            if (activeTarget.tag == "PickUp")
            {
				//Highlight the object white
				GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Highlight(activeTarget, scrHighlightController.outline1);

                Item targetItem = activeTarget.GetComponent<Item>();
                PickUp(targetItem); //Pick it up
                //set the lastActiveTarget to this activeTarget
                lastActiveTarget = activeTarget;
            }
            //Is the item close and useable?
            else if (activeTarget.tag == "Useable" || activeTarget.name == "RightGateLock" || activeTarget.name == "LeftGateLock" || activeTarget.tag == "Trap")
            {
				if(activeTarget.name == "Dirt")
				{
					lookingAtDirt = true;
				}

                Useable targetUseable = activeTarget.GetComponent<Useable>();
                UseItem(targetUseable); //Use it
            }
            else if (activeTarget.tag == "Consumable")
            {
				//Highlight the object white
				GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Highlight(activeTarget, scrHighlightController.outline1);

                Consume();

                //set the lastActiveTarget to this activeTarget
                lastActiveTarget = activeTarget;
            }
            else if(activeTarget.tag == "Gnome")
            {
                lookingAtGnome = true;
            }
            else if (activeTarget.tag == "Gargoyle")
            {
                lookingAtGargoyle = true;
            }
            else if(lastActiveTarget != null && lastActiveTarget != activeTarget)
            {
				//Highlight the object white
				GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Unhighlight(lastActiveTarget);
                //set the lastActiveTarget to null
                lastActiveTarget = null;
            }
            else
            {
                //Check this (might not be needed)
                canHover = false; //Hide item name
                notUseable = false;
                lookingAtGnome = false;
                lookingAtGargoyle = false;
				lookingAtDirt = false;
            }
        }
        else 
        {
			if(lastActiveTarget != null)
			{
				//Highlight the object white
				GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Unhighlight(lastActiveTarget);
				//set the lastActiveTarget to null
				lastActiveTarget = null;
			}

            canHover = false;
            lookingAtGnome = false;
            lookingAtGargoyle = false;
			lookingAtDirt = false;
        }
    }

    void PickUp(Item targetItem)
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0) && !pause && !delayPause)
        {
            if (playerGUI.AddToSlot(targetItem))
            {
                //check if the target name is the shed key
                if (targetItem.name == "ShedKey")
                {
                    //get the gnome in the shed
                    GameObject gnome = GameObject.Find("GnomeShed");
                    //play the drop and laugh audio
                    //gnome.GetComponent<AudioSource>().Play();
                    gnome.GetComponent<NavMeshAgent>().enabled = true;
                    gnome.GetComponent<Gnome>().enabled = true;
                }

                activeTarget.SetActive(false);
            }
        }
    }

    void UseItem(Useable targetUseable)
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0) && !pause && !delayPause)
        {
            if(targetUseable.Interact() != "")
            {
                notUseable = true;
                canHover = false;
                GUIString = targetUseable.Interact();
            }
        }
    }

    void Consume()
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButton(0) && !pause && !delayPause)
        {
			GameObject.Find("Player").GetComponent<Player>().itemEatSound.Play();

            Destroy(activeTarget); //Remove item

            if (activeTarget.name == "EnergyBar")
            {
                //Increase the player's energy
                GetComponent<Player>().sanity += 20.0F;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //First time exiting shed trigger
        if(col.gameObject.name == "ExitTutorial" && st.tutorial)
        {
            GameObject.Find("Weather").GetComponent<weatherScript>().changeWeather = true; //Start changing the weather randomly
			GameObject.Find("GlobalSoundController").GetComponent<SoundController>().eerieSound.Play(); //Start playing background music
            st.tutorial = false;
        }
    }

    void GUIControl()
    {
        //listen for Q being pressed
        if (Input.GetKeyUp(KeyCode.Q) 
		    && !pause 
		    && !this.gameObject.animation.IsPlaying("OpeningCut")
		    && charMotor.IsGrounded()
		    && !scrJournal.journalNoGUI
		    && !scrBook.bookNoGUI
		    && !scrBed.resting)
        {
            ToggleGUI(!showGUI);
        }
    }

    void ToggleGUI(bool activeGUI)
    {
        showGUI = activeGUI;
		//activate/deactivate gui
		playerGUI.gameObject.SetActive(showGUI);
		
		if (showGUI && playerGUI.slots != null)
		{
			foreach (ItemSlot slot in playerGUI.slots)
			{
				//reset rotation for each slot
				slot.gui.ResetRotation();
			}
			
			//reset rotation for key ring and energy bar
			playerGUI.keyRing.gui.ResetRotation();
			playerGUI.energyBar.gui.ResetRotation();
		}

		GameObject.Find("Player").GetComponent<Blink>().reading = showGUI;

	    //toggle movements, looking, cursor
	    mouseLook.enabled = !showGUI;
	    cameraLook.enabled = !showGUI;
	    Screen.lockCursor = !showGUI;
		GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = !showGUI;
		//GameObject.Find("Player").GetComponent<Player>().enabled = !showGUI;
		charMotor.jumping.enabled = !showGUI;
		charMotor.canControl = !showGUI;
    }

    string getImprovedName(string targetName)
    {
        string improvedName = "";
        for (int i = 0; i < targetName.Length; i++)
        {
            if ((char.IsUpper(targetName[i]) || char.IsNumber(targetName[i])) && i != 0)
            {
                improvedName += " ";
            }
            improvedName += targetName[i];
        }

        return improvedName;
    }
}