using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    //is the game saving?
    private bool saving = false;
    //display time for saving notification
    private int displayTime = 300;
    public static bool loaded = false;
    public static bool savingStatus = false;
    public static float[] leaderboardTimes;
    public static string[] leaderboardNames;

    //this method saves the values into playerInfo.dat
    public void Save(bool menu)
    {
        LoadUnload.showEverything();
        //create new binary formatter
        BinaryFormatter bf = new BinaryFormatter();
        //create new filestream
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        //create new Game object
        Game data = new Game();
        //put data into the Game object
        saveGameValues(data);

        //save the Game object
        bf.Serialize(file, data);
        //close the file
        file.Close();

        if (menu)
        {
            Application.LoadLevel("MainMenu");
        }
    }

    public static void saveLeaderboard()
    {       
        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/leaderboards.dat", FileMode.Open);
        PlayerInteractions player = GameObject.Find("Player").GetComponent<PlayerInteractions>();
        Leaderboard leader;

        if (File.Exists(Application.persistentDataPath + "/leaderboards.dat"))
        {
            leader = (Leaderboard)bf.Deserialize(file);

            leaderboardTimes = leader.times;
            leaderboardNames = leader.names;
            file.Close();

            file = File.Create(Application.persistentDataPath + "/leaderboards.dat");
            leader = new Leaderboard();
            
            int index = -1;
            for (int i = 0; i < leaderboardTimes.Length; i++)
            {
                if (leaderboardTimes[i] > player.timePlayed || leaderboardTimes[i] == 0)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                int i = leaderboardTimes.Length - 1;
                while(i != index)
                {
                    leaderboardTimes[i] = leaderboardTimes[i-1];
                    leaderboardNames[i] = leaderboardNames[i-1];
                    i--;
                }

                leaderboardTimes[index] = player.timePlayed;
                leaderboardNames[index] = PlayerInteractions.playerName;
            }
            else
            {
                PlayerInteractions.playerName = "NotOnLeaderboard";
            }

            leader.times = leaderboardTimes;
            leader.names = leaderboardNames;
            bf.Serialize(file, leader);
		}
		else
		{
			Debug.LogError("Leaderboard file doesn't exist");
		}

        file.Close();
    }

	public static void CreateLeaderboard()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file;
		//PlayerInteractions player = GameObject.Find("Player").GetComponent<PlayerInteractions>();
		Leaderboard leader;

		file = File.Create(Application.persistentDataPath + "/leaderboards.dat");
		leader = new Leaderboard();
		
		leader.times = new float[5];
		//leader.times[0] = player.timePlayed;
		leader.names = new string[5];
		
		for(int i = 0; i < leader.names.Length; i++)
		{
			leader.names[i] = "Empty";
			leader.times[i] = 0.0F;
		}
		
		//leader.names[0] = PlayerInteractions.playerName;
		bf.Serialize(file, leader);

		file.Close ();
	}

    public void Load()
    {
        //check if there is a file to load
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            //creat a binary formatter
            BinaryFormatter bf = new BinaryFormatter();
            //create a new file stream
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            //get the game data from the file
            Game data = (Game)bf.Deserialize(file);
            //close the file
            file.Close();

            //apply the loaded game values to the scene
            loadGameValues(data);
            GameObject.Find("Player").GetComponent<ShedTutorial>().enabled = false;
            GameObject.Find("Nodes").GetComponent<NodeWrapper>().newGame = false;
        }
        else
        {
            //display if the file could not be loaded
            Debug.LogError("Couldn't Load File.");
        }

    }

    public static void loadLeaderboards()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        //PlayerInteractions player = GameObject.Find("Player").GetComponent<PlayerInteractions>();
        Leaderboard leader;

        if (File.Exists(Application.persistentDataPath + "/leaderboards.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/leaderboards.dat", FileMode.Open);
            leader = (Leaderboard)bf.Deserialize(file);
            leaderboardNames = leader.names;
            leaderboardTimes = leader.times;
            file.Close();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        //did we enter a save trigger?
        if (gameObject.name == "Save" && other.name == "Player")
        {
            //save the game
            Save(false);
            //set saving to true
            saving = true;
        }

    }
    public void saveGameValues(Game data)
    {

        savingStatus = true;

        //retreive all the objects that need data saved from the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Gnome");
        GameObject[] gargoyles = GameObject.FindGameObjectsWithTag("Gargoyle");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        Array.Sort(useable, CompareObNames);
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        Array.Sort(useable, CompareObNames);
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
        ItemSlot[] held = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.slots;
        KeyRing keyRing = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.keyRing;

        //create new gnome location and rotations arrays
        data.gnomeLocations = new float[enemies.Length, 3];
        data.gnomeRotations = new float[enemies.Length, 4];

        //store all the gnome locations and rotations into the arrays
        for (int k = 0; k < enemies.Length; k++)
        {
            data.gnomeLocations[k, 0] = enemies[k].transform.position.x;
            data.gnomeLocations[k, 1] = enemies[k].transform.position.y;
            data.gnomeLocations[k, 2] = enemies[k].transform.position.z;

            data.gnomeRotations[k, 0] = enemies[k].transform.rotation.x;
            data.gnomeRotations[k, 1] = enemies[k].transform.rotation.y;
            data.gnomeRotations[k, 2] = enemies[k].transform.rotation.z;
            data.gnomeRotations[k, 3] = enemies[k].transform.rotation.w;
        }

        data.gargoyleLocations = new float[gargoyles.Length/4,3];
        for (int k = 0; k < gargoyles.Length; k++)
        {
            if (gargoyles[k].name == "Gargoyle")
            {
                data.gargoyleLocations[k/4, 0] = gargoyles[k/4].transform.position.x;
                data.gargoyleLocations[k/4, 1] = gargoyles[k/4].transform.position.y;
                data.gargoyleLocations[k/4, 2] = gargoyles[k/4].transform.position.z;
            }
        }

        //store the player location, rotation, and health
        data.playerLocation = new float[3] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
        data.playerRotation = new float[4] { player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w };
        data.playerHealth = player.gameObject.GetComponent<Player>().sanity;

        //create new array to hold all held items
        data.heldItems = new string[held.Length];
        //save all the held item names
        for (int i = 0; i < held.Length; i++)
        {
            if (held[i].heldItem != null)
            {
                data.heldItems[i] = held[i].heldItem.name;
            }
            else
                data.heldItems[i] = null;
        }

        //create new array to hold all the keys
        data.keys = new string[keyRing.keys.Count];
        //save all the key names
        for (int j = 0; j < keyRing.keys.Count; j++)
        {
          data.keys[j] = keyRing.keys.ToArray()[j].name;
        }

        //create new array to hold the useable's locations and rotations
        data.useableLocations = new float[useable.Length, 3];
        data.useableRotations = new float[useable.Length, 4];
        //store all the useable's locations and rotations into the arrays
        for (int m = 0; m < useable.Length; m++)
        {
         
            data.useableLocations[m, 0] = useable[m].transform.position.x;
            data.useableLocations[m, 1] = useable[m].transform.position.y;
            data.useableLocations[m, 2] = useable[m].transform.position.z;

            data.useableRotations[m, 0] = useable[m].transform.rotation.x;
            data.useableRotations[m, 1] = useable[m].transform.rotation.y;
            data.useableRotations[m, 2] = useable[m].transform.rotation.z;
            data.useableRotations[m, 3] = useable[m].transform.rotation.w;
        }
        //create new array to hold the consumables
        data.consumables = new float[consumables.Length, 3];
        //save all the consumable's locations
        for(int n = 0; n < consumables.Length; n++)
        {
            data.consumables[n, 0] = consumables[n].transform.position.x;
            data.consumables[n, 1] = consumables[n].transform.position.y;
            data.consumables[n, 2] = consumables[n].transform.position.z;
        }

        data.traps = new string[traps.Length];
        for (int h = 0; h < traps.Length; h++)
        {
            data.traps[h] = traps[h].name;
        }

        //create new array to hold the consumables
        data.pickUps = new float[pickUps.Length, 3];
        data.pickUpNames = new string[pickUps.Length];
        //save all the consumable's locations
        for (int n = 0; n < pickUps.Length; n++)
        {
            data.pickUps[n, 0] = pickUps[n].transform.position.x;
            data.pickUps[n, 1] = pickUps[n].transform.position.y;
            data.pickUps[n, 2] = pickUps[n].transform.position.z;
            data.pickUpNames[n] = pickUps[n].name;
        }

        //save time played
        data.timePlayed = player.GetComponent<PlayerInteractions>().timePlayed + Time.timeSinceLevelLoad;
        savingStatus = false;

        LoadUnload.checkTrigger = true;

    }

    public void loadGameValues(Game data)
    {


        //retreive all the objects that need data loaded into them
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Gnome");
        GameObject[] gargoyles = GameObject.FindGameObjectsWithTag("Gargoyle");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        Array.Sort(useable, CompareObNames);
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        Array.Sort(traps, CompareObNames);
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
        ItemSlot[] held = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.slots;
        KeyRing keyRing = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.keyRing;

		//EndGames.getAllItems(pickUps);

        if (data.gnomeLocations.GetLength(0) != enemies.Length)
        {
            int lengthDiff = enemies.Length - data.gnomeLocations.GetLength(0);
            for(int i = 0; i < lengthDiff; i++)
            {
                DestroyImmediate(enemies[i]);
            }
            enemies = GameObject.FindGameObjectsWithTag("Gnome");

        }
        //loop through the enemies(gnomes)
        for (int k = 0; k < enemies.Length; k++)
        {
            //disable the gnome nav mesh's so they can be moved
            enemies[k].GetComponent<NavMeshAgent>().enabled = false;
            //move the gnomes into position and adjust their rotation
            enemies[k].transform.position = new Vector3(data.gnomeLocations[k, 0], data.gnomeLocations[k, 1], data.gnomeLocations[k, 2]);
            enemies[k].transform.rotation = new Quaternion(data.gnomeRotations[k, 0], data.gnomeRotations[k, 1], data.gnomeRotations[k, 2], data.gnomeRotations[k, 3]);
            //re-enable the nav mesh agent on the gnomes
            enemies[k].GetComponent<NavMeshAgent>().enabled = true;

        }

        if (data.gargoyleLocations.GetLength(0) != gargoyles.Length/4)
        {
            int lengthDiff = gargoyles.Length - data.gargoyleLocations.GetLength(0);
            for (int i = 0; i < lengthDiff; i++)
            {
                if (gargoyles[i].name == "Body")
                {
                    gargoyles[i].GetComponent<groundCheck>().BreakApart(false);
                    i = (4-(i % 4)) + i;
                }

            }
            gargoyles = GameObject.FindGameObjectsWithTag("Gargoyle");

        }
        for (int k = 0; k < gargoyles.Length; k++)
        {
            if (gargoyles[k].name == "Gargoyle")
            {
                //move the garoyles into position
                gargoyles[k/4].transform.position = new Vector3(data.gargoyleLocations[k/4, 0], data.gargoyleLocations[k/4, 1], data.gargoyleLocations[k/4, 2]);
            }
        }
        //position the player
        player.transform.position = new Vector3(data.playerLocation[0], data.playerLocation[1], data.playerLocation[2]);
        player.transform.rotation = new Quaternion(data.playerRotation[0], data.playerRotation[1], data.playerRotation[2], data.playerRotation[3]);
        //set the player's health
        player.gameObject.GetComponent<Player>().sanity = data.playerHealth;

        //load the player's inventory
        for (int i = 0; i < held.Length; i++)
        {
            //if there was no item, set it to null
            if (data.heldItems[i] == null)
                held[i].heldItem = null;
            //if their was an item, set the item in the GUI and remove it from the scene
            else
            {
                Debug.Log(data.heldItems[i]);

                    held[i].heldItem = GameObject.Find(data.heldItems[i]).GetComponent<Item>();
                    held[i].heldItem.name = data.heldItems[i];
                    GameObject.Find(data.heldItems[i]).SetActive(false);
                

            }
        }

        //load the player's keys
        for (int j = 0; j < data.keys.Length; j++)
        {
            //add the keys back into the inventory and remove them from the scene
            keyRing.AddKey(GameObject.Find(data.keys[j]).GetComponent<Item>());
            keyRing.keys.ToArray()[j].name = data.keys[j];
            GameObject.Find(data.keys[j]).SetActive(false);
        }

        //set the useable's position and rotation
        for (int m = 0; m < useable.Length; m++)
        {

            useable[m].transform.position = new Vector3(data.useableLocations[m, 0], data.useableLocations[m, 1], data.useableLocations[m, 2]);
            useable[m].transform.rotation = new Quaternion(data.useableRotations[m, 0], data.useableRotations[m, 1], data.useableRotations[m, 2], data.useableRotations[m, 3]);

        }

        //load the traps
        for (int h = 0; h < traps.Length; h++)
        {
            //checks whether the consumable was consumed or not
            bool notfound = false;
            for (int n = 0; n < data.traps.Length; n++)
            {
                //if the consumable wasn't consumed, make sure it is in the game world
                if (data.traps[n] == traps[h].name)
                {
                    notfound = true;
                }
            }
            //if the consumable was not found in the list, remove it from the game world
            if (!notfound)
            {
                Destroy(GameObject.Find(traps[h].name));
                if(traps[h].name.Contains("Dirt"))
                {
                    GameObject.Find("DirtTrap").collider.enabled = true;
                }
            }
        }
        //load the consumables
        for (int h = 0; h < consumables.Length; h++)
        {
            //checks whether the consumable was consumed or not
            bool notfound = false;
            for (int n = 0; n < data.consumables.GetLength(0); n++ )
            {
                //if the consumable wasn't consumed, make sure it is in the game world
                if(data.consumables[n,0] == consumables[h].transform.position.x && data.consumables[n,1] == consumables[h].transform.position.y && data.consumables[n,2] == consumables[h].transform.position.z)
                {
                    notfound = true;
                }
            }
            //if the consumable was not found in the list, remove it from the game world
            if(!notfound)
            {
                Destroy(GameObject.Find(consumables[h].name));
            }
        }

        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        //load the pickUps
        for (int h = 0; h < pickUps.Length; h++)
        {
 
            pickUps[h].transform.position = new Vector3(data.pickUps[h, 0], data.pickUps[h, 1], data.pickUps[h, 2]);

        }

        //load time played
        player.GetComponent<PlayerInteractions>().timePlayed = data.timePlayed;
        loaded = true;

        LoadUnload.iAmLoaded = true;

        GameObject.Find("Player").GetComponent<LoadUnload>().enabled = true;
        LoadUnload.checkTrigger = true;

    }

    void OnGUI()
    {
        //check if we are saving
        if (saving)
        {
            //display saving notification
            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(-5, 5, 100, 25), "Saving...");
            //if the display time is 0, stop displaying
            if (displayTime == 0)
            {
                saving = false;
                displayTime = 300;
            }
            //remove time from the timer
            else
            {
                displayTime--;
            }
        }
    }

    int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }


}