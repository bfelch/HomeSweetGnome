using UnityEngine;
using System.Collections;

public class elevatorStuff : MonoBehaviour 
{
    private bool direction = false;
    private bool activate = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (activate)
        {
            if (direction)
            {
                transform.Translate(Vector3.down * 3 * Time.deltaTime);

                if (transform.localPosition.y <= -41.4F)
                {
                    Debug.Log("Stop Down");
                    activate = false;
                }
            }
            else
            {
                transform.Translate(Vector3.up * 3 * Time.deltaTime);

                if (transform.localPosition.y >= -23.7F)
                {
                    Debug.Log("Stop Up");
                    activate = false;
                }
            }
        }
	}

    public void Activate()
    {
        //do stuff
        direction = !direction;
        activate = true;
    }
}
