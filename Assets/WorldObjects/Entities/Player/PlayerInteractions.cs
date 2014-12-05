using UnityEngine;
using System.Collections;

public class PlayerInteractions : MonoBehaviour
{
	private Camera mainC;
    private HingeJoint doorHinge;
    private ShedTutorial st;
    private string GUIString;

    //duration of play
    public float timePlayed;
    public string playerName = "d";

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
    public bool showGUI;

    //reference to gui
    public GUIWrapper playerGUI;
    private float lightingValue = .1f;
    private float xSens=10;
    private float ySens=10;
    public Texture2D crosshair;

    private bool optionsMenu = false;
    private bool pause;
    private bool pauseMenu;
    private Font bark;

    void Start()
    {
        //sets gui start
        ToggleGUI(showGUI);
        st = gameObject.GetComponent<ShedTutorial>();
        outline = Resources.Load("Outline") as Material;
        bark = GetComponent<Player>().bark;
    }

    void Update()
    {
        itemAction();
        GUIControl();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           if(!pause)
           { 
                pause = true;
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
               this.GetComponent<PlayerMovement>().enabled = true;
               this.GetComponent<Player>().enabled = true;
               Time.timeScale = 1.0f;
               mouseLook.enabled = true;
               cameraLook.enabled = true;
               pause = false;
           }
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            GameObject.Find("TempPlayerLight").GetComponent<Light>().enabled = !GameObject.Find("TempPlayerLight").GetComponent<Light>().enabled;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;
        GUI.skin.font = bark;
        if (pause && pauseMenu)
        {
            mouseLook.enabled = false;
            cameraLook.enabled = false;
            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 20, 300, Screen.height));
            //GUI.Box(new Rect(0, 0,200, 250), "");
            int second = (int)(timePlayed + Time.timeSinceLevelLoad);
            int minute = second / 60;
            int hour = minute / 60;
            second %= 60;
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;
            GUI.Box(new Rect(55, 50, 180, 40), "Time Played\n" + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00"));
            if (GUI.Button(new Rect(55, 100, 180, 40), "Resume"))
            {
                Screen.lockCursor = true;
                this.GetComponent<PlayerMovement>().enabled = true;
                this.GetComponent<Player>().enabled = true;
                Time.timeScale = 1.0f;
                mouseLook.enabled = true;
                cameraLook.enabled = true;
                pause = false;
            }
            if (GUI.Button(new Rect(55, 150, 180, 40), "Options"))
            {
                optionsMenu = true;
                pauseMenu = false;
            }
            if (GUI.Button( new Rect(55, 200, 180, 40), "Main Menu"))
            {
                GameObject.Find("Save").GetComponent<SaveLoad>().Save();
                Application.LoadLevel("MainMenu");
            }
            if (GUI.Button(new Rect(55, 250, 180, 40), "Quit"))
            {
                Application.Quit();
            }

            //layout end
            GUI.EndGroup();
        }
        if (optionsMenu && pause)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 20, 300, Screen.height));
            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(55, 90, 180, 30), "Adjust Lighting");
            GUI.backgroundColor = Color.white;
            lightingValue = GUI.HorizontalSlider(new Rect(55, 120, 180, 10), lightingValue, 0, .2f);
            GameObject.Find("DebugLight").GetComponent<Light>().intensity = lightingValue;

            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(55, 130, 180, 30), "Adjust X Sensitivity");
            GUI.backgroundColor = Color.white;
            xSens = GUI.HorizontalSlider(new Rect(55, 160, 180, 10), xSens, 2, 15);
            GameObject.Find("Player").GetComponent<MouseLook>().sensitivityX = xSens;

            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(55, 170, 180, 30), "Adjust Y Sensitivity");
            GUI.backgroundColor = Color.white;
            ySens = GUI.HorizontalSlider(new Rect(55, 200, 180, 10), ySens, 2, 15);
            GameObject.Find("Main Camera").GetComponent<MouseLook>().sensitivityY = ySens;

            if (GUI.Button(new Rect(55, 220, 180, 40), "Back"))
            {
                optionsMenu = false;
                pauseMenu = true;
            }

            //layout end
            GUI.EndGroup();
        }
        else if (showGUI)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = 1f;
            int playerUILayer = 11;
            int layerMask = 1 << playerUILayer;

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                GUISlot guiSlot = hit.collider.gameObject.GetComponent<GUISlot>();

                if (guiSlot.isKeyRing)
                {
                    KeyRing ring = guiSlot.GetComponent<KeyRing>();

                    Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
                    GUI.Box(box, "Keys: " + ring.keys.Count);
                }
                else if (guiSlot.isEnergyBar)
                {
                    EnergyBar bar = guiSlot.GetComponent<EnergyBar>();

                    Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 150, 30);
                    GUI.Box(box, "Energy: " + (int)bar.player.sanity + " / " + (int)bar.player.maxSanity);
                }
                else
                {
                    ItemSlot slot = guiSlot.GetComponent<ItemSlot>();

                    Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
                    if (slot.heldItem != null)
                    {
                        GUI.Box(box, slot.heldItem.name);

                        if (Input.GetMouseButtonUp(1))
                        {
                            slot.heldItem.gameObject.SetActive(true);

                            Vector3 pos = transform.position;

                            slot.heldItem.transform.position = new Vector3(pos.x, pos.y - 2.5f, pos.z);

                            slot.heldItem = null;
                        }
                    }
                    else
                    {
                        GUI.Box(box, "Empty");
                    }
                }

                /*KeyItem key = hit.collider.gameObject.GetComponent<KeyItem>();

                if (key != null) {
                    Rect box = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 30);
                    GUI.Box(box, key.name);
                }*/
            }
        }
        else
        {
            GUI.backgroundColor = Color.clear;
            if (canHover && activeTarget != null && !st.tutorial && !notUseable)
            {
                //Strip target name
                string targetName = activeTarget.name;
                string improvedName = "";
                for (int i = 0; i < targetName.Length; i++)
                {
                    if (char.IsUpper(targetName[i]) && i != 0)
                    {
                        improvedName += " ";
                    }
                    improvedName += targetName[i];
                }
                this.GetComponent<Player>().flashFade();
                //Display item name
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30), improvedName);
            }
            else if (notUseable && activeTarget != null)
            {
                this.GetComponent<Player>().flashFade();
                GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 150, Screen.width, 30), "You need the " + GUIString + " to continue.");
            }

            if (!showGUI)
            {
                GUI.Box(new Rect(Screen.width / 2-10, Screen.height / 2-10, 20, 20), "+");
            }
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

        if (Physics.Raycast(cam.position, cam.forward, out hit, 5, ignoreMask))
        {
            activeTarget = hit.collider.gameObject; //Store item being looked at

            //Is the item close and a pick up?
            if (activeTarget.tag == "PickUp")
            {
                //get the mesh renderer
                MeshRenderer currentMesh = activeTarget.GetComponent<MeshRenderer>();
                //save the current mesh material
                Material current = currentMesh.material;
                //create new array of materials of size 2
                Material[] mats = new Material[2];
                //set materials to the current one and the outline
                mats[0] = current;
                mats[1] = outline;
                //set the materials to the currentMesh
                currentMesh.materials = mats; 
                Item targetItem = activeTarget.GetComponent<Item>();
                PickUp(targetItem); //Pick it up
                //set the lastActiveTarget to this activeTarget
                lastActiveTarget = activeTarget;
            }
            //Is the item close and useable?
            else if (activeTarget.tag == "Useable" || activeTarget.name == "RightFrontGate" || activeTarget.name == "LeftFrontGate")
            {
                Useable targetUseable = activeTarget.GetComponent<Useable>();
                UseItem(targetUseable); //Use it
            }
            else if (activeTarget.tag == "Consumable")
            {
                //get the mesh renderer
                MeshRenderer currentMesh = activeTarget.GetComponent<MeshRenderer>();
                //save the current mesh material
                Material current = currentMesh.material;
                //create new array of materials of size 2
                Material[] mats = new Material[2];
                //set materials to the current one and the outline
                mats[0] = current;
                mats[1] = outline;
                //set the materials to the currentMesh
                currentMesh.materials = mats;
                Consume();
                //set the lastActiveTarget to this activeTarget
                lastActiveTarget = activeTarget;

            }
            else if(lastActiveTarget != null && lastActiveTarget != activeTarget)
            {
                //get the mesh renderer
                MeshRenderer currentMesh = lastActiveTarget.GetComponent<MeshRenderer>();
                //save the first material
                Material current = currentMesh.materials[0];
                //create new array of materials of size 1
                Material[] mats = new Material[1];
                //set the material 
                mats[0] = current;
                //set the material to the current mehs
                currentMesh.materials = mats;
                //set the lastActiveTarget to null
                lastActiveTarget = null;
            }
            else
            {
                //Check this (might not be needed)
                canHover = false; //Hide item name
                notUseable = false;
            }
        }
        else 
        {
            canHover = false;
        }
    }

    void PickUp(Item targetItem)
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (playerGUI.AddToSlot(targetItem))
            {
                //check if the target name is the shed key
                if (targetItem.name == "ShedKey")
                {
                    //get the gnome in the shed
                    GameObject gnome = GameObject.Find("GnomeShed");
                    //play the drop and laugh audio
                    gnome.GetComponent<AudioSource>().Play();
                    //rotate the gnome so it's facing you
                    gnome.transform.eulerAngles = new Vector3(gnome.transform.eulerAngles.x, 346, gnome.transform.eulerAngles.z);
                    //set it's position on the floor
                    gnome.transform.localPosition = new Vector3(-76.30657f, 39.7061f, 99.21371f);
                    //enable the nav mesh and the gnome script
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
        if (Input.GetKeyUp(KeyCode.E))
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
        if (Input.GetKeyUp(KeyCode.E))
        {
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
        if(col.gameObject.name == "EndGame")
        {
            Application.LoadLevel("MainMenu"); //should be player win screen
        }

        //First time exiting shed trigger
        if(col.gameObject.name == "ExitTutorial" && st.tutorial)
        {
            GameObject.Find("Weather").GetComponent<weatherScript>().changeWeather = true; //Start changing the weather randomly
            GetComponent<Player>().bgMusic.Play(); //Start playing background music
            st.tutorial = false;
        }
    }

    void GUIControl()
    {
        //listen for Q being pressed
        if (Input.GetKeyUp(KeyCode.Q) && !pause)
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

        //toggle movements, looking, cursor
        charMotor.enabled = !showGUI;
        mouseLook.enabled = !showGUI;
        cameraLook.enabled = !showGUI;
        Screen.lockCursor = !showGUI;
    }
}