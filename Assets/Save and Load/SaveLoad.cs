using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    private bool saving = false;
    private int displayTime = 300;
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
            saving = true;
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
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
        ItemSlot[] held = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.slots;
        KeyRing keyRing = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.keyRing;

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
            Debug.Log(enemies[k].transform.position);
        }
        data.playerLocation = new float[3] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
        data.playerRotation = new float[4] { player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w };
        data.playerHealth = player.gameObject.GetComponent<Player>().sanity;

        data.heldItems = new string[held.Length];
        for (int i = 0; i < held.Length; i++)
        {
            if (held[i].heldItem != null)
            {
                data.heldItems[i] = held[i].heldItem.name;
            }
            else
                data.heldItems[i] = null;
        }

        data.keys = new string[keyRing.keys.Count];
        for (int j = 0; j < keyRing.keys.Count; j++)
        {
          data.keys[j] = keyRing.keys.ToArray()[j].name;
        }

        data.useableLocations = new float[useable.Length, 3];
        data.useableRotations = new float[useable.Length, 4];
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

        data.consumables = new float[consumables.Length, 3];
        for(int n = 0; n < consumables.Length; n++)
        {
            data.consumables[n, 0] = consumables[n].transform.position.x;
            data.consumables[n, 1] = consumables[n].transform.position.y;
            data.consumables[n, 2] = consumables[n].transform.position.z;
        }
    }

    public void loadGameValues(Game data)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject[] useable = GameObject.FindGameObjectsWithTag("Useable");
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
        ItemSlot[] held = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.slots;
        KeyRing keyRing = GameObject.Find("Player").GetComponent<PlayerInteractions>().playerGUI.keyRing;


        for (int k = 0; k < enemies.Length; k++)
        {
            enemies[k].GetComponent<NavMeshAgent>().enabled = false;
            enemies[k].transform.position = new Vector3(data.gnomeLocations[k, 0], data.gnomeLocations[k, 1], data.gnomeLocations[k, 2]);
            enemies[k].transform.rotation = new Quaternion(data.gnomeRotations[k, 0], data.gnomeRotations[k, 1], data.gnomeRotations[k, 2], data.gnomeRotations[k, 3]);
            enemies[k].GetComponent<NavMeshAgent>().enabled = true;

        }
        player.transform.position = new Vector3(data.playerLocation[0], data.playerLocation[1], data.playerLocation[2]);
        player.transform.rotation = new Quaternion(data.playerRotation[0], data.playerRotation[1], data.playerRotation[2], data.playerRotation[3]);
        player.gameObject.GetComponent<Player>().sanity = data.playerHealth;

        for (int i = 0; i < held.Length; i++)
        {
            if (data.heldItems[i] == null)
                held[i].heldItem = null;
            else
            {
                held[i].heldItem = GameObject.Find(data.heldItems[i]).GetComponent<Item>();
                held[i].heldItem.name = data.heldItems[i];
                GameObject.Find(data.heldItems[i]).SetActive(false);
            }
        }

        for (int j = 0; j < data.keys.Length; j++)
        {
            keyRing.AddKey(GameObject.Find(data.keys[j]).GetComponent<Item>());
            keyRing.keys.ToArray()[j].name = data.keys[j];
            GameObject.Find(data.keys[j]).SetActive(false);
        }

        for (int m = 0; m < useable.Length; m++)
        {
            useable[m].transform.position = new Vector3(data.useableLocations[m, 0], data.useableLocations[m, 1], data.useableLocations[m, 2]);
            useable[m].transform.rotation = new Quaternion(data.useableRotations[m, 0], data.useableRotations[m, 1], data.useableRotations[m, 2], data.useableRotations[m, 3]);
   
        }

        for (int h = 0; h < consumables.Length; h++)
        {
            bool notfound = false;
            for (int n = 0; n < data.consumables.GetLength(0); n++ )
            {
                if(data.consumables[n,0] == consumables[h].transform.position.x && data.consumables[n,1] == consumables[h].transform.position.y && data.consumables[n,2] == consumables[h].transform.position.z)
                {
                    notfound = true;
                }
            }
            if(!notfound)
            {
                Destroy(GameObject.Find(consumables[h].name));
            }
        }

    }

    void OnGUI()
    {
        if (saving)
        {
            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(-5, 5, 100, 25), "Saving...");
            if (displayTime == 0)
            {
                saving = false;
                displayTime = 300;
            }
            else
            {
                displayTime--;
            }
        }
    }


}