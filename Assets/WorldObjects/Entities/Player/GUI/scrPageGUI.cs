using UnityEngine;
using System.Collections;

public class scrPageGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        GameObject.Find("GUICamera").transform.position = transform.position;
        GameObject.Find("GUICamera").transform.rotation = transform.rotation;
        Debug.Log(GameObject.Find("GUICamera").transform.rotation);
	}
}
