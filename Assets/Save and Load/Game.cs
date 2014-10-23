using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{


    [SerializeField]
    public float[,] gnomeLocations;
    [SerializeField]
    public float[,] gnomeRotations;

    [SerializeField]
    public float[] playerLocation;
    [SerializeField]
    public float[] playerRotation;
    [SerializeField]
    public float playerHealth;

    [SerializeField]
    public ArrayList pickUp_names;
    [SerializeField]
    public ArrayList pickUp_values;
    [SerializeField]
    public ArrayList useable_names;
    [SerializeField]
    public ArrayList useable_values;

    void findObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");

        playerLocation = new float[3];
        playerRotation = new float[3];
    }



}
