using UnityEngine;
using System.Collections;

public class HedgemazeGate : MonoBehaviour 
{
    Animation gate; //The animator component

	void Start()
	{
		gate = GetComponent<Animation>();
	}

	//Calls when object is enabled or set to active
	void OnEnable()
	{
		//gate = GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "Player")
        {
            Debug.Log("in player");
			gate.animation["HedgeGateOpen"].speed = 2f; //Play animation fowards
			//gate.animation["HedgeGateOpen"].time = 0; //Start from beginning of animation
            gate.Play("HedgeGateOpen");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Player")
        {
			gate.animation["HedgeGateOpen"].speed = -2f; //Play animation backwards
            if (!gate.animation.IsPlaying("HedgeGateOpen"))
            {
                gate.animation["HedgeGateOpen"].time = gate.animation["HedgeGateOpen"].length;
           }
			//gate.animation["HedgeGateOpen"].time = gate.animation["HedgeGateOpen"].length; //Start from beginning of animation
			gate.Play("HedgeGateOpen");
        }
    }
}
