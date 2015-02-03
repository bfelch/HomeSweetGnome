using UnityEngine;
using System.Collections;

public class HedgemazeGate : MonoBehaviour {

    Animator gate;
	// Use this for initialization
	void Start () {
        gate = GameObject.Find("HedgemazeGate").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("should be opening");
        if(col.name == "OpenGate")
        {
            gate.recorderStartTime = 1;
            gate.recorderStopTime = 90;
            gate.Play("Open");
 
        }


    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("should be closing");

        if (col.name == "OpenGate")
        {
            gate.recorderStartTime = 90;
            gate.recorderStopTime = 1;
            gate.Play("Open");

        }


    }
}
