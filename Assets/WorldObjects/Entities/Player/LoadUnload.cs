using UnityEngine;
using System.Collections;

/*
 * Description: This script handles dynamically loading and unloading of objects within the game world
*/
public class LoadUnload : MonoBehaviour 
{
	private string[][] shedTrigger; //Objects to be disabled while in the hedgemaze
	private string[][] hedgeTrigger; //Objects to be disabled while in the tunnels
	private string[][] greenTrigger; //Objects to be disabled while in the shed
    private string[][] mansionGreen; //Objects to be disabled while in the shed
    private string[][] mansionHedge; //Objects to be disabled while in the shed
    private string[][] tunnelsTrigger; //Objects to be disabled while in the shed
    private static string[] all;

    private bool shedTrigSwitch = false;
    private bool shedTrigExit = false;
    private bool hedgeTrigSwitch = false;
    private bool greenTrigSwitch = false;
    private bool manGreenTrigSwitch = false;
    private bool manHedgeTrigSwitch = false;
    private bool tunnelsTrigSwitch = false;
    private bool tunnelsTrigExit = false;

    public static bool checkTrigger = false;
    public static bool iAmLoaded = false;

	public weatherScript weather; //Weather script
	public Moonlight moonlight; //Moonlight script

	// Use this for initialization
	void Start() 
	{

        if (PlayerPrefs.GetInt("LoadGame") != 1)
        {
            iAmLoaded = true;
        }

        all = new string[]
        {
            "Mansion",
            "Greenhouse",
            "Hedgemaze",
            "Dock",
            "Shed",
            "EstateWall"

        };

        shedTrigger = new string[][]
		{
			new string[]{
                "Dock",
			    "Greenhouse",
                "Mansion",
                "Hedgemaze"
            },
            new string[]{
                "Shed",
                "EstateWall"
            },
            new string[]{
                "Greenhouse",
                "Hedgemaze",
                "Mansion"
            }

		};

        //Get objects to be disabled while in the hedgemaze
        hedgeTrigger = new string[][]
		{
			new string[]{
                "Dock",
			    "EstateWall",
			    "Greenhouse",
			    "Shed",
                "Mansion"
            },
            new string[]
            {
                "Greenhouse",
                "Mansion"
            },
		};

        greenTrigger = new string[][] 
		{
			new string[] 
		    {
                "Dock",
			    "EstateWall",
			    "Hedgemaze",
			    "Shed"
            },
            new string[] 
		    {
                "Hedgemaze"
            }
		};

		//Get objects to be disabled while in the tunnels
        mansionGreen = new string[][]
		{
			new string[]
            {
                "EstateWall",
			    "Shed",
			    "Hedgemaze",
                "Dock"
            },
            new string[]
            {
                "Hedgemaze"
            }
		};
 
		//Get objects to be disabled while in the shed
        mansionHedge = new string[][]
		{
			new string[]
            {
                "Greenhouse",
			    "EstateWall",
			    "Dock",
                "Shed"
            },
            new string[]
            {
                "Greenhouse"
            }
		};

        tunnelsTrigger = new string[][] 
		{
			new string[]
            {
                "Greenhouse",
                "Mansion",
                "Hedgemaze",
                "Shed",
                "EstateWall"
            },
            new string[]
            {
                "Greenhouse",
                "Mansion",
                "Hedgemaze"
            },

            new string[]
            {
                "Dock"
            }
		};

       
    }
	
	//Update is called once per frame
	void Update () 
	{
        if(shedTrigSwitch)
        {
            //Issue here with loading
            destroyStructures(shedTrigger[0]);
            shedTrigSwitch = false;

            
            for (int i = 0; i < shedTrigger[1].Length; i++)
            {
                if (GameObject.Find(shedTrigger[1][i]) == null)
                {
                    GameObject go = Instantiate(Resources.Load("Structures/" + shedTrigger[1][i])) as GameObject;
                    go.transform.parent = GameObject.Find("Structures").transform;
                    go.name = shedTrigger[1][i];
                }
            }

        }
        if (hedgeTrigSwitch)
        {
            destroyStructures(hedgeTrigger[0]);
            hedgeTrigSwitch = false;
        }
        if (greenTrigSwitch)
        {
            destroyStructures(greenTrigger[0]);
            greenTrigSwitch = false;
        }
        if (manGreenTrigSwitch)
        {
            destroyStructures(mansionGreen[0]);
            manGreenTrigSwitch = false;
        }
        if (manHedgeTrigSwitch)
        {
            destroyStructures(mansionHedge[0]);
            manHedgeTrigSwitch = false;
        }
        if (tunnelsTrigSwitch)
        {
            destroyStructures(tunnelsTrigger[0]);
            tunnelsTrigSwitch = false;

            GameObject go = Instantiate(Resources.Load("Structures/" + tunnelsTrigger[2][0])) as GameObject;
            go.transform.parent = GameObject.Find("Structures").transform;
            go.name = tunnelsTrigger[2][0];

            GameObject.Find("DockGnome1").SetActive(false);
            GameObject.Find("DockGnome2").SetActive(false);
            GameObject.Find("DockGnome3").SetActive(false);


        }
        if (shedTrigExit)
        {
            destroyStructures(shedTrigger[1]);
            shedTrigExit = false;
        }
        if (tunnelsTrigExit)
        {
            destroyStructures(tunnelsTrigger[2]);
            tunnelsTrigExit = false;
        }

	}

    void OnTriggerEnter(Collider col)
    {
        if (iAmLoaded)
        {
            //Hedge Load
            if (col.name == "ShedTrigger")
            {
                //Remove non-hedge objects
                shedTrigSwitch = true;
            }
            if (col.name == "HedgeTrigger")
            {
                //Remove non-hedge objects
                hedgeTrigSwitch = true;
            }
            if (col.name == "GreenTrigger")
            {
                //Remove non-hedge objects
                greenTrigSwitch = true;
            }
            if (col.name == "MansionGreen")
            {
                //Remove non-hedge objects
                manGreenTrigSwitch = true;
            }
            if (col.name == "MansionHedge")
            {
                //Remove non-hedge objects
                manHedgeTrigSwitch = true;
            }
            if (col.name == "TunnelsTrigger")
            {
                //Remove non-hedge objects
                tunnelsTrigSwitch = true;
            }

            //Tunnel Load (Entrance)
            if (col.name == "TunnelLoadEntrance")
            {

                weather.StopWeather(); //Stop weather

                moonlight.lightFadeIn = false; //Light is only fading out
                moonlight.lightFadeOut = true; //Remove moonlight
                GameObject.Find("Main Camera").GetComponent<GlobalFog>().enabled = false;

            }

            //Tunnel Unload (Entrance)
            if (col.name == "TunnelUnloadEntrance")
            {

                weather.StartWeather(); //Start weather

                moonlight.lightFadeOut = false; //Light is only fading in
                moonlight.lightFadeIn = true; //Add moonlight
                GameObject.Find("Main Camera").GetComponent<GlobalFog>().enabled = true;

            }

            //Tunnel Load (Exit)
            if (col.name == "TunnelLoadExit")
            {
                weather.StopWeather(); //Weather

                moonlight.lightFadeIn = false; //Light is only fading out
                moonlight.lightFadeOut = true; //Remove moonlight
                GameObject.Find("Main Camera").GetComponent<GlobalFog>().enabled = false;


            }

            //Tunnel Unload (Exit)
            if (col.name == "TunnelUnloadExit")
            {
                weather.StartWeather(); //Start weather

                moonlight.lightFadeOut = false; //Light is only fading in
                moonlight.lightFadeIn = true; //Add moonlight
                GameObject.Find("Main Camera").GetComponent<GlobalFog>().enabled = true;

            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (checkTrigger)
        {
            //Hedge Load
            if (col.name == "ShedTrigger")
            {
                //Remove non-hedge objects
                shedTrigSwitch = true;

            }
            if (col.name == "HedgeTrigger")
            {
                //Remove non-hedge objects
                hedgeTrigSwitch = true;
            }
            if (col.name == "GreenTrigger")
            {
                //Remove non-hedge objects
                greenTrigSwitch = true;
            }
            if (col.name == "MansionGreen")
            {
                //Remove non-hedge objects
                manGreenTrigSwitch = true;
            }
            if (col.name == "MansionHedge")
            {
                //Remove non-hedge objects
                manHedgeTrigSwitch = true;
            }
            if (col.name == "TunnelsTrigger")
            {
                //Remove non-hedge objects
                tunnelsTrigSwitch = true;
            }

            checkTrigger = false;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.name == "ShedTrigger")
        {
            for (int i = 0; i < shedTrigger[2].Length; i++)
            { 
                GameObject go = Instantiate(Resources.Load("Structures/" + shedTrigger[2][i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = shedTrigger[2][i];
            }
            shedTrigExit = true;
        }

        if (col.name == "HedgeTrigger")
        {
            for (int i = 0; i < hedgeTrigger[1].Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + hedgeTrigger[1][i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = hedgeTrigger[1][i];
            }
        }
        if (col.name == "GreenTrigger")
        {
            for (int i = 0; i < greenTrigger[1].Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + greenTrigger[1][i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = greenTrigger[1][i];
            }
        }
        if (col.name == "MansionGreen")
        {
            for (int i = 0; i < mansionGreen[1].Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + mansionGreen[1][i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = mansionGreen[1][i];
            }
        }
        if (col.name == "MansionHedge")
        {
            for (int i = 0; i < mansionHedge[1].Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + mansionHedge[1][i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = mansionHedge[1][i];
            }
        }
        if (col.name == "TunnelsTrigger")
        {
            for (int i = 0; i < tunnelsTrigger[1].Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + tunnelsTrigger[1][i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = tunnelsTrigger[1][i];
            }
            tunnelsTrigExit = true;
        }
    }
    void destroyStructures(string[] structs)
    {
        for (int i = 0; i < structs.Length; i++)
        {
            Destroy(GameObject.Find(structs[i]));
        }

    }

    public static void showEverything()
    {
        for (int i = 0; i < all.Length; i++)
        {
            if (GameObject.Find(all[i]) == null)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + all[i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = all[i];
            }
        }
    }
}
