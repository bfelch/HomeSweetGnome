using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game : MonoBehaviour {

    private GameObject[] enemies;
    private GameObject player;
    private GameObject[] pickUps;
    private GameObject[] useable;
    private GameObject[] structures;
    public static Game current;
	// Use this for initialization
	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        player = GameObject.FindGameObjectWithTag("player");
        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        useable = GameObject.FindGameObjectsWithTag("Useable");
        structures = GameObject.FindGameObjectsWithTag("Structure");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
