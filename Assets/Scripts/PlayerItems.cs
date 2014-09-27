using UnityEngine;
using System.Collections;

public class PlayerItems : MonoBehaviour {

    public static ArrayList pickUp_names = new ArrayList();
    public static ArrayList pickUp_values = new ArrayList();

    private bool canHover = false;

    void Start()
    {
        pickUp_names.Add("ShedKey");
        pickUp_values.Add(false);
    }

    void Update()
    {
        PickUp();
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;
        if (canHover)
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 100, 30), "Shed Key");
    }

    void PickUp()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            if (hit.distance <= 5.0 && hit.collider.gameObject.tag == "PickUp")
            {
                canHover = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(hit.collider.gameObject);
                    for (int j = 0; j < pickUp_names.Count; j++)
                    {
                        if (pickUp_names.ToArray()[j].Equals(hit.collider.gameObject.name))
                        {
                            pickUp_values[j] = true;
                            Debug.Log(pickUp_values[j]);
                        }
                    }
                }
            }
            else
            {
                canHover = false;
            }
        }

    }



}
