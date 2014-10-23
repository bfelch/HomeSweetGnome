using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{

    public static List<Game> savedGames = new List<Game>();
    //it's static so we can call it from anywhere
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        Game data = new Game();
        saveGameValues(data);

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            Game data = (Game)bf.Deserialize(file);
            file.Close();

            loadGameValues(data);
            //sceneModification(data);
        }
        else
        {
            Debug.LogError("Couldn't Load File.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "Save")
        {
            Save();
        }
        else if (gameObject.name == "Load")
        {
            Load();
        }
    }
    public void saveGameValues(Game data)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");

        data.gnomeLocations = new float[enemies.Length, 3];
        data.gnomeRotations = new float[enemies.Length, 4];

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
            enemies[k].transform.position = new Vector3(data.gnomeLocations[k, 0], data.gnomeLocations[k, 1], data.gnomeLocations[k, 2]);
            enemies[k].transform.rotation = new Quaternion(data.gnomeRotations[k, 0], data.gnomeRotations[k, 1], data.gnomeRotations[k, 2], data.gnomeRotations[k, 3]);
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

}