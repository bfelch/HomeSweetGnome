using UnityEngine;
using System.Collections;

public class PlayerInteractions : MonoBehaviour
{
	private Camera mainC;
    private HingeJoint doorHinge;
    private ShedTutorial st;
    private string GUIString;

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

    public Texture2D crosshair;

    private bool pause;

    void Start()
    {
        //sets gui start
        ToggleGUI(showGUI);
        st = gameObject.GetComponent<ShedTutorial>();
        outline = Resources.Load("Outline") as Material;
    }

    void Update()
    {
        itemAction();
        GUIControl();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = true;
            Screen.showCursor = true;
            Screen.lockCursor = false;
            Time.timeScale = 0.0f;

            mouseLook.enabled = false;
            cameraLook.enabled = false;
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

        if (pause)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 100, 300, 250));
            //GUI.Box(new Rect(0, 0,200, 250), "");

            if (GUI.Button(new Rect(55, 100, 180, 40), "Resume"))
            {
                Screen.showCursor = false;
                Screen.lockCursor = true;
                Time.timeScale = 1.0f;
                mouseLook.enabled = true;
                cameraLook.enabled = true;
                pause = false;
            }
            if (GUI.Button( new Rect(55, 150, 180, 40), "Main Menu"))
            {
                GameObject.Find("Save").GetComponent<SaveLoad>().Save();
                Application.LoadLevel("MainMenu");
            }
            if (GUI.Button(new Rect(55, 200, 180, 40), "Quit"))
            {
                Application.Quit();
            }

            //layout end
            GUI.EndGroup(); 
        }
        else
        {
            GUI.backgroundColor = Color.clear;
            if (canHover && activeTarget != null && !st.tutorial && !notUseable)
            {
                //Strip target name
                string targetName = activeTarget.name;
                string improvedName = "";
                for (int i = 0; i < targetName.Length; i++ )
                {
                    if(char.IsUpper(targetName[i]) && i != 0)
                    {
                        improvedName += " ";
                    }
                    improvedName += targetName[i];
                }
                    this.GetComponent<Player>().flashFade();
                    //Display item name
                    GUI.Box(new Rect(Screen.width - Screen.width / 2, Screen.height - Screen.height / 2, 250, 30), improvedName);
            }
            else if (notUseable && activeTarget != null)
            {
                this.GetComponent<Player>().flashFade();
                GUI.Box(new Rect(5, 5, 300, 30), "You need the " + GUIString + " to continue.");
            }

            if (!showGUI)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, crosshair.width, crosshair.height), crosshair);
            }
        }
    }

    void itemAction()
    {
		Transform cam = Camera.main.transform;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);

		//Player layer mask
		int playerLayer = 8;
		int playerMask = 1 << playerLayer;

        //Invert bitmask to only ignore this layer
        playerMask = ~playerMask;

		RaycastHit hit;
		Debug.DrawRay(cam.position, cam.forward * 5, Color.white);

        if (Physics.Raycast(cam.position, cam.forward, out hit, 5, playerMask))
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
            else if (activeTarget.tag == "Useable")
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
                    //set the nav mesh agent to be false
                    gnome.GetComponent<NavMeshAgent>().enabled = false;
                    //set the gnome scirpt to false
                    gnome.GetComponent<Gnome>().enabled = false;
                    //play the drop and laugh audio
                    gnome.GetComponent<AudioSource>().Play();
                    //rotate the gnome so it's facing you
                    gnome.transform.eulerAngles = new Vector3(gnome.transform.eulerAngles.x, 346, gnome.transform.eulerAngles.z);
                    //set it's position on the floor
                    gnome.transform.localPosition = new Vector3(-75.78156f, 39.829f, 98.48463f);
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
        if(col.gameObject.name == "ExitTutorial")
        {
            GameObject.Find("Weather").GetComponent<weatherScript>().start = true; //Start changing the weather randomly
            GetComponent<Player>().audio1.Play(); //Start playing background music
            st.tutorial = false;
        }
    }

    void GUIControl()
    {
        //listen for Q being pressed
        if (Input.GetKeyUp(KeyCode.Q))
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