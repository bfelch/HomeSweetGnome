using UnityEngine;
using System.Collections;

public class PlayerInteractions : MonoBehaviour
{
    public static ArrayList pickUp_names = new ArrayList();  //Stores items that can be picked up
    public static ArrayList pickUp_values = new ArrayList();  //Stores value for whether items have been picked up

    public static ArrayList useable_names = new ArrayList(); //Stores items that can be interacted with
    public static ArrayList useable_values = new ArrayList(); //Stores value for whether items have been interacted with

    private bool canHover = false; //Show the item name being look at?
    private GameObject activeTarget; //The item being looked at
	private Camera mainC;

    void Start()
    {
        pickUp_names.Add("ShedKey"); //Index 1
		pickUp_names.Add("Shovel"); //Index 2

        pickUp_values.Add(false); //Index 1
		pickUp_values.Add(false); //Index 2

        useable_names.Add("ShedDoor"); //Index 1
		useable_names.Add("Dirt"); //Index 2

        useable_values.Add(false); //Index 1
		useable_values.Add(false); //Index 2
    }

    void Update()
    {
        itemAction();
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        openDoor(hit);
    }

    void itemAction()
    {
		Transform cam = Camera.main.transform;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);
       
		Ray ray = new Ray(cam.position, cam.forward);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Physics.Raycast(cam.position, cam.forward, out hit))
        {
            activeTarget = hit.collider.gameObject; //Store item being looked at

            //Is the item close and a pick up?
            if (hit.distance <= 5.0 && activeTarget.tag == "PickUp")
            {
                PickUp(); //Pick it up
            }
            //Is the item close and useable?
            else if (hit.distance <= 5.0 && activeTarget.tag == "Useable")
            {
                UseItem(activeTarget); //Use it
            }
            else
            {
                canHover = false; //Hide item name
            }
        }
    }

    void PickUp()
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(activeTarget); //Remove item

            //Find index of item being looked at
            for (int j = 0; j < pickUp_names.Count; j++)
            {
                if (pickUp_names.ToArray()[j].Equals(activeTarget.name))
                {
                    pickUp_values[j] = true; //Item is picked up
                }
            }
        }
    }

    void UseItem(GameObject activeTarget)
    {
        canHover = true; //Display item name

        //Pressing the E (Interact) key?
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Find index of item being looked at
            for (int j = 0; j < useable_names.Count; j++)
            {
                if (useable_names.ToArray()[j].Equals(activeTarget.name))
                {
                    if ((bool)pickUp_values.ToArray()[j] == true)
                    {
                        if(activeTarget.name == "ShedDoor")
                        {
                            activeTarget.tag = "Door";
                        }

						if(activeTarget.name == "Dirt")
						{
							Destroy(activeTarget); //Remove item
						}
                        //Destroy(activeTarget); //Remove item
                        useable_values[j] = true; //Item was interacted with
                    }
                }
            }
        }
    }

    void openDoor(ControllerColliderHit hit)
    {
        float pushPower = 2.0f;

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) { return; }
        if (hit.collider.gameObject.tag.Equals("Door"))
        {
            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;
        }
    }
}