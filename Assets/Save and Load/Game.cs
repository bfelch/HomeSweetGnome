using UnityEngine;
using System.Collections;

//this class holsd all the data that needs to be saved and loaded
[System.Serializable]
public class Game
{
    //store all the gnome locations
    [SerializeField]
    public float[,] gnomeLocations;
    //store all the gnome rotations
    [SerializeField]
    public float[,] gnomeRotations;

    [SerializeField]
    public float[,] gargoyleLocations;

    //store the player's location
    [SerializeField]
    public float[] playerLocation;
    //store the player's rotation
    [SerializeField]
    public float[] playerRotation;
    //store the player's health
    [SerializeField]
    public float playerHealth;

    //store all the held items
    [SerializeField]
    public string[] heldItems;
    //store all the held keys
    [SerializeField]
    public string[] keys;
    //store all the useable's rotations (mainly for doors)
    [SerializeField]
    public float[,] useableRotations;
    [SerializeField]
    //store all the useable's locations
    public float[,] useableLocations;
    [SerializeField]
    //store all the traps's locations
    public string[] traps;
    [SerializeField]
    //store all the consumables
    public float[,] consumables;
    [SerializeField]
    //store all the consumables
    public float[,] pickUps;
    [SerializeField]
    //store all the consumables
    public string[] pickUpNames;

    [SerializeField]
    public float timePlayed;

}
