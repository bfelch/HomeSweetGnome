using UnityEngine;
using System.Collections;

//this class holsd all the data that needs to be saved and loaded
[System.Serializable]
public class Leaderboard
{
    //store all the gnome locations
    [SerializeField]
    public float[] times;
    //store all the gnome rotations
    [SerializeField]
    public string[] names;

}
