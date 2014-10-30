using UnityEngine;
using System.Collections;

public class GnomeParent : MonoBehaviour 
{
    public GameObject target;

    public GameObject gnomeLvl1;
    public GameObject gnomeLvl2;
    public GameObject gnomeLvl3;
    private GameObject currentModel;

    private float playerSanity;
    private float playerSanityMax;
    public int modelNumber = 1;

    private GameObject[] gnomes;

    public Blink blink;

	// Use this for initialization
	void Start () 
	{
        playerSanity = target.GetComponent<Player>().sanity;
        playerSanityMax = target.GetComponent<Player>().maxSanity;

        //get all the gnomes
        gnomes = GameObject.FindGameObjectsWithTag("Enemy");
        blink = target.GetComponent<Blink>();

	}
	
	// Update is called once per frame
	void Update () 
	{
        ChangeModel();
	}

    private void ChangeModel()
    {
        //caluclate sanityPercentage
        playerSanity = target.GetComponent<Player>().sanity;
        playerSanityMax = target.GetComponent<Player>().maxSanity;
        float sanityPercentage = playerSanity / playerSanityMax;

        //check the model number and sanity percetange and change the model accordingly
        if(sanityPercentage < .7 && sanityPercentage > .4 && modelNumber != 2)
        {
            modelNumber = 2;
            for (int i = 0; i < gnomes.Length; i++)
            {
                Vector3 newPos = new Vector3(gnomes[i].transform.position.x, gnomes[i].transform.position.y, gnomes[i].transform.position.z);
                GameObject thisModel = Instantiate(gnomeLvl2, newPos, gnomes[i].transform.rotation) as GameObject;
                thisModel.transform.parent = transform;
				thisModel.GetComponent<Gnome>().gnomeLevel = 2;
                Destroy(gnomes[i]);
                gnomes[i] = thisModel;
            }
            //blink if model is changing
            blink.blinkTimer = 0;
            blink.BlinkMechanics();

        }
        else if (modelNumber != 1 && sanityPercentage > .7)
        {
            modelNumber = 1;
            for (int i = 0; i < gnomes.Length; i++)
            {
                Vector3 newPos = new Vector3(gnomes[i].transform.position.x, gnomes[i].transform.position.y, gnomes[i].transform.position.z);
                GameObject thisModel = Instantiate(gnomeLvl1, newPos, gnomes[i].transform.rotation) as GameObject;
                thisModel.transform.parent = transform;
				thisModel.GetComponent<Gnome>().gnomeLevel = 1;
                Destroy(gnomes[i].gameObject);
                gnomes[i] = thisModel;
            }
            //blink if model is changing
            blink.blinkTimer = 0;
            blink.BlinkMechanics();
        }
    }
}
