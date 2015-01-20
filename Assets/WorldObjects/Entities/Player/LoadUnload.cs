using UnityEngine;
using System.Collections;

/*
 * Description: This script handles dynamically loading and unloading of objects within the game world
*/
public class LoadUnload : MonoBehaviour 
{
	private string[] inHedge; //Objects to be disabled while in the hedgemaze
	private string[] inTunnels; //Objects to be disabled while in the tunnels
	private string[] inShed; //Objects to be disabled while in the shed

	private bool tunnelLoaded = false; //Flag to check if tunnel is loaded
	private bool hedgeLoaded = false; //Flag to check if hedge is loaded
	private bool shedLoaded = true; //Flag to check if shed if loaded

	public weatherScript weather; //Weather script
	public Moonlight moonlight; //Moonlight script

	// Use this for initialization
	void Start() 
	{
		//Get objects to be disabled while in the hedgemaze
		inHedge = new string[] 
		{
			"Dock",
			"EstateWall",
			"Greenhouse",
			"Shed",
            "Mansion"
		};
		//Get objects to be disabled while in the tunnels
		inTunnels = new string[] 
		{
			"EstateWall",
			"Shed",
			"Greenhouse",
			"Hedgemaze",
            "Mansion"
		};
		//Get objects to be disabled while in the shed
		inShed = new string[] 
		{
			"Greenhouse",
			"Hedgemaze",
			"Dock",
            "Mansion"
		};

        //Instantiate(Resources.Load("Structures/Mansion"));
		//Player starts inside the shed
		
        for (int i = 0; i < inShed.Length; i++)
        {
            Destroy(GameObject.Find(inShed[i])); 
        }
    }
	
	//Update is called once per frame
	void Update () 
	{

	}

    void OnTriggerEnter(Collider col)
    {
		//Hedge Load
        if(col.name == "HedgeLoad" && hedgeLoaded == false)
        {
			//Add hedge objects

			//Remove non-hedge objects
            
            for (int i = 0; i < inHedge.Length; i++)
            {
                Destroy(GameObject.Find(inHedge[i]));
            }
            

            hedgeLoaded = true; //Hedge is loaded
        }

		//Hedge Unload
        if(col.name == "HedgeUnload" && hedgeLoaded == true)
        {
			//Remove hedge objects

			//Add non-hedge objects
            
            for (int i = 0; i < inHedge.Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + inHedge[i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = inHedge[i];
            }
            

            hedgeLoaded = false; //Hedge is not loaded
        }

		//Tunnel Load (Entrance)
		if (col.name == "TunnelLoadEntrance" && tunnelLoaded == false)
		{
			//Add tunnel objects

			//Remove non-tunnel objects
            
            for (int i = 0; i < inTunnels.Length; i++)
            {
                Destroy(GameObject.Find(inTunnels[i]));
            }
            

            weather.StopWeather(); //Stop weather

			moonlight.lightFadeIn = false; //Light is only fading out
			moonlight.lightFadeOut = true; //Remove moonlight

			tunnelLoaded = true; //Tunnel is loaded
		}

		//Tunnel Unload (Entrance)
        if (col.name == "TunnelUnloadEntrance" && tunnelLoaded == true)
        {
			//Remove tunnel objects

			//Add non-tunnel objects
            
            for (int i = 0; i < inTunnels.Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/" + inTunnels[i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = inTunnels[i];
            }
            

            weather.StartWeather(); //Start weather

			moonlight.lightFadeOut = false; //Light is only fading in
			moonlight.lightFadeIn = true; //Add moonlight

			tunnelLoaded = false; //Tunnel is not loaded
        }

		//Tunnel Load (Exit)
		if (col.name == "TunnelLoadExit" && tunnelLoaded == false)
		{	
			weather.StopWeather(); //Weather

			moonlight.lightFadeIn = false; //Light is only fading out
			moonlight.lightFadeOut = true; //Remove moonlight

			tunnelLoaded = true; //Tunnel is loaded
		}

		//Tunnel Unload (Exit)
		if (col.name == "TunnelUnloadExit" && tunnelLoaded == true)
		{
			weather.StartWeather(); //Start weather

			moonlight.lightFadeOut = false; //Light is only fading in
			moonlight.lightFadeIn = true; //Add moonlight

			tunnelLoaded = false; //Tunnel is not loaded
		}

		//Shed Load
        if (col.name == "ShedLoad" && shedLoaded == false)
        {
			//Add shed objects

			//Remove non-shed objects
            
            for (int i = 0; i < inShed.Length; i++)
            {
                Destroy(GameObject.Find(inShed[i]));
            }
            

            shedLoaded = true; //Shed is loaded
        }

		//Shed Unload
        if (col.name == "ShedUnload" && shedLoaded == true)
        {
			//Remove shed objects

			//Add non-shed objects
            
            for (int i = 0; i < inShed.Length; i++)
            {
                GameObject go = Instantiate(Resources.Load("Structures/"+ inShed[i])) as GameObject;
                go.transform.parent = GameObject.Find("Structures").transform;
                go.name = inShed[i];
            }
            

            shedLoaded = false; //Shed is not loaded
        }
    }
}
