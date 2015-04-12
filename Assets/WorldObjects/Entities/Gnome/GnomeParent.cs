using UnityEngine;
using System.Collections;

public class GnomeParent : MonoBehaviour 
{
    //the player object
    public GameObject target;

    //the gnome level prefabs
    public GameObject gnomeLvl1;
    public GameObject gnomeLvl2;

    //the player sanity values
    private float playerSanity;
    private float playerSanityMax;

    //the model number that the gnome is currently one
    public int modelNumber = 1;

    //arrray to hold all the gnome game objects
    private GameObject[] gnomes;

    //the player blink component
    public Blink blink;

	// Use this for initialization
	void Start () 
	{
        //retrieve the player sanity values
        playerSanity = target.GetComponent<Player>().sanity;
        playerSanityMax = target.GetComponent<Player>().maxSanity;

        //get all the gnomes
        gnomes = GameObject.FindGameObjectsWithTag("Gnome");

        //get the blink component from the player
        blink = target.GetComponent<Blink>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        //check if the model needs to be changed
        ChangeModel();
	}

    private void ChangeModel()
    {
        //retrieve current player sanity values
        playerSanity = target.GetComponent<Player>().sanity;
        playerSanityMax = target.GetComponent<Player>().maxSanity;
        //calculate sanity percentage
        float sanityPercentage = playerSanity / playerSanityMax;
        //check if the sanity is below 50%
        if(sanityPercentage < .4 && modelNumber != 2)
        {
            //set the model number to 2
            modelNumber = 2;
            gnomes = GameObject.FindGameObjectsWithTag("Gnome");
            //loop through the gnomes to change all their model
            for (int i = 0; i < gnomes.Length; i++)
            {
				if(this.gameObject.name == "GnomeShed")
				{
					//TODO
				}
				else
				{
					Vector3 newPos;
					//Set position of gnome
					if(gnomes[i].GetComponent<Gnome>().fallen == true)
					{
						newPos = gnomes[i].GetComponent<Gnome>().spawnPosition;
					}
					else
					{
						newPos = new Vector3(gnomes[i].transform.position.x, gnomes[i].transform.position.y, gnomes[i].transform.position.z);
					}

					//create the level 2 gnome with the same position as the level one gnome
					GameObject thisModel = Instantiate(gnomeLvl2, newPos, gnomes[i].transform.rotation) as GameObject;
					thisModel.gameObject.name = "GnomeLvl2";
					//set the level2 gnome parent to be the Gnome parent
					thisModel.transform.parent = transform;
					//update the gnome level
					Gnome.gnomeLevel = 2;
					if(gnomes[i].GetComponent<Gnome>().trapped == true)
					{
						thisModel.GetComponent<Gnome>().trapped = true;
					}
					//destroy the level 1 gnome
					Destroy(gnomes[i]);
					//set the value in the gnomes array to be the level 2 gnome
					gnomes[i] = thisModel;
				}
            }
            //blink if model is changing
            blink.blinkTimer = 0;
            blink.BlinkMechanics();
        }
        //check if the snity is above 50%
        else if (modelNumber != 1 && sanityPercentage > .7)
        {
            //set the model number to 2
            modelNumber = 1;
            gnomes = GameObject.FindGameObjectsWithTag("Gnome");
            //loop through the gnomes to change all their model
            for (int i = 0; i < gnomes.Length; i++)
            {
                //save the current position of the gnome
                Vector3 newPos = new Vector3(gnomes[i].transform.position.x, gnomes[i].transform.position.y, gnomes[i].transform.position.z);
                //create the level 1 gnome with the same position as the level 2 gnome
                GameObject thisModel = Instantiate(gnomeLvl1, newPos, gnomes[i].transform.rotation) as GameObject;
				thisModel.gameObject.name = "GnomeLvl1";
                //set the level 1 gnome parent to be the Gnome parent
                thisModel.transform.parent = transform;
				//update the gnome level
				Gnome.gnomeLevel = 1;

				//Transfer trapped propety
				if(gnomes[i].GetComponent<Gnome>().trapped == true)
				{
					thisModel.GetComponent<Gnome>().trapped = true;
				}
                //destroy the level 2 gnome
                Destroy(gnomes[i].gameObject);
                //set the value in the gnomes array to be the level 1 gnome
                gnomes[i] = thisModel;
            }
            //blink if model is changing
            blink.blinkTimer = 0;
            blink.BlinkMechanics();
        }
    }
}
