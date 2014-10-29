using UnityEngine;
using System.Collections;

public class chandStuff : MonoBehaviour 
{
    public bool dropping = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (dropping)
        {
            this.collider.enabled = true;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        dropping = false;
        this.collider.enabled = false;
    }
}
