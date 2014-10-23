using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{

    public static List<Game> savedGames = new List<Game>();
    //it's static so we can call it from anywhere
    public static void Save()
    {
        SaveLoad.savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
<<<<<<< HEAD
        data.playerLocation = new float[3] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
        data.playerRotation = new float[4] { player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w };
        data.playerHealth = player.gameObject.GetComponent<Player>().sanity;
        data.pickUp_names = player.gameObject.GetComponent<PlayerInteractions>().pickUp_names;
        data.pickUp_values = player.gameObject.GetComponent<PlayerInteractions>().pickUp_values;
        data.useable_names = player.gameObject.GetComponent<PlayerInteractions>().useable_names;
        data.useable_values = player.gameObject.GetComponent<PlayerInteractions>().useable_values;

    }

    public void loadGameValues(Game data)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");

        for (int k = 0; k < enemies.Length; k++)
        {
            enemies[k].transform.position = new Vector3(data.gnomeLocations[k,0], data.gnomeLocations[k,1], data.gnomeLocations[k,2]);
            enemies[k].transform.rotation = new Quaternion(data.gnomeRotations[k,0], data.gnomeRotations[k,1], data.gnomeRotations[k,2], data.gnomeRotations[k,3]);
        }
        player.transform.position = new Vector3(data.playerLocation[0], data.playerLocation[1], data.playerLocation[2]);
        player.transform.rotation = new Quaternion(data.playerRotation[0], data.playerRotation[1], data.playerRotation[2], data.playerRotation[3]);
        player.gameObject.GetComponent<Player>().sanity = data.playerHealth;
        player.gameObject.GetComponent<PlayerInteractions>().pickUp_names = data.pickUp_names;
        player.gameObject.GetComponent<PlayerInteractions>().pickUp_values = data.pickUp_values;
        player.gameObject.GetComponent<PlayerInteractions>().useable_names = data.useable_names;
        player.gameObject.GetComponent<PlayerInteractions>().useable_values = data.useable_values;

        for (int i = 0; i < data.pickUp_names.ToArray().Length; i++)
        {
            if ((bool)data.pickUp_values.ToArray()[i])
            {
                Destroy(GameObject.Find(data.pickUp_names.ToArray()[i].ToString()));
            }
        }

        for (int i = 0; i < data.useable_values.ToArray().Length; i++)
        {
            if ((bool)data.useable_values.ToArray()[i] && data.useable_names.ToArray()[i].ToString().Contains("Door"))
            {
                GameObject.Find(data.useable_names.ToArray()[i].ToString()).tag = "Door";
            }
        }

    }

=======
    }
>>>>>>> parent of 7b05c24... Saving and Loading Started
}