using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game{


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

    void findObjects () {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");

        playerLocation = new float[3];
        playerRotation = new float[3];

        //gnomeLocations = new float[enemies.Length][];
        //gnomeRotations = new float[enemies.Length][];
	}
	
    //public void saveGameValues()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
    //    GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
    //    GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");

    //    for(int k = 0; k < enemies.Length; k++)
    //    {
    //        //gnomeLocations[k][0] = enemies[k].transform.position.x;
    //        //gnomeLocations[k][1] = enemies[k].transform.position.y;
    //        //gnomeLocations[k][2] = enemies[k].transform.position.z;

    //        //gnomeRotations[k][0] = enemies[k].transform.rotation.x;
    //        //gnomeRotations[k][1] = enemies[k].transform.rotation.y;
    //        //gnomeRotations[k][2] = enemies[k].transform.rotation.z;
    //        //gnomeRotations[k][3] = enemies[k].transform.rotation.w;
    //    }
    //    playerLocation = new float[3] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
    //    playerRotation = new float[3]{ player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z};
    //    playerHealth = player.gameObject.GetComponent<Player>().sanity;
    //    pickUp_names = player.gameObject.GetComponent<PlayerInteractions>().pickUp_names;
    //    pickUp_values = player.gameObject.GetComponent<PlayerInteractions>().pickUp_values;
    //    useable_names = player.gameObject.GetComponent<PlayerInteractions>().useable_names;
    //    useable_values = player.gameObject.GetComponent<PlayerInteractions>().useable_values;

    //}

    //public void loadGameValues()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
    //    GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
    //    GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");
        
    //    for (int k = 0; k < enemies.Length; k++)
    //    {
    //        //enemies[k].transform.position = new Vector3(gnomeLocations[k][0], gnomeLocations[k][1], gnomeLocations[k][2]);
    //        //enemies[k].transform.rotation = new Quaternion(gnomeRotations[k][0], gnomeRotations[k][1], gnomeRotations[k][2], gnomeRotations[k][3]);
    //    }
    //    player.transform.position = new Vector3(playerLocation[0], playerLocation[1], playerLocation[2]);
    //    player.transform.rotation = new Quaternion(playerRotation[0], playerRotation[1], playerRotation[2],0);
    //    player.gameObject.GetComponent<Player>().sanity = playerHealth;
    //    player.gameObject.GetComponent<PlayerInteractions>().pickUp_names = pickUp_names;
    //    Debug.Log(pickUp_names.Count);
    //    player.gameObject.GetComponent<PlayerInteractions>().pickUp_values = pickUp_values;
    //    player.gameObject.GetComponent<PlayerInteractions>().useable_names = useable_names;
    //    player.gameObject.GetComponent<PlayerInteractions>().useable_values = useable_values;

        

    //}

    
}
