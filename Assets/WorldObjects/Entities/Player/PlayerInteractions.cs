using UnityEngine;
using System.Collections;

public class PlayerInteractions : MonoBehaviour
{
    private bool canHover = false; //Show the item name being look at?
    private GameObject activeTarget; //The item being looked at
	private Camera mainC;
    private HingeJoint doorHinge;

    public CharacterMotor charMotor;
    public MouseLook mouseLook;
    public MouseLook cameraLook;

    public bool showGUI;

    public GUIWrapper playerGUI;

    void Start()
    {
        ToggleGUI(showGUI);
    }

    void Update()
    {
        itemAction();
        GUIControl();
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;
        if (canHover && activeTarget != null)
        {
            //Display item name
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 100, 30), activeTarget.name);
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
            Debug.Log(activeTarget);

            //Is the item close and a pick up?
            if (activeTarget.tag == "PickUp")
            {
                Item targetItem = activeTarget.GetComponent<Item>();
                Debug.Log("Pick Up");
                PickUp(targetItem); //Pick it up
            }
            //Is the item close and useable?
            else if (activeTarget.tag == "Useable")
            {
                Useable targetUseable = activeTarget.GetComponent<Useable>();
                UseItem(targetUseable); //Use it
            }
            else
            {
                Debug.Log("No Pick Up");
                canHover = false; //Hide item name
            }
        }
        else { Debug.Log("NO OBJECT"); }
    }

    void PickUp(Item targetItem)
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (playerGUI.AddToSlot(targetItem))
            {
                Destroy(activeTarget);
            }
        }
    }

    void UseItem(Useable targetUseable)
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyUp(KeyCode.E))
        {
            targetUseable.Interact();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "EndGame")
        {
            Application.LoadLevel("MainMenu"); //should be player win screen
        }
    }

    void GUIControl()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            ToggleGUI(!showGUI);
        }
    }

    void ToggleGUI(bool activeGUI)
    {
        showGUI = activeGUI;
        playerGUI.gameObject.SetActive(showGUI);

        if (showGUI && playerGUI.slots != null)
        {
            foreach (ItemSlot slot in playerGUI.slots)
            {
                slot.gui.ResetRotation();
            }
        }

        charMotor.enabled = !showGUI;
        mouseLook.enabled = !showGUI;
        cameraLook.enabled = !showGUI;
        Screen.lockCursor = !showGUI;
    }
}