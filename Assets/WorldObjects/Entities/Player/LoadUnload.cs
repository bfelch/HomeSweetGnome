using UnityEngine;
using System.Collections;

public class LoadUnload : MonoBehaviour 
{
    private GameObject[] inHedge;
    private GameObject[] inTunnels;
    private GameObject[] inShed;

	private bool tunnelLoaded = false;

	public weatherScript weather;
	public Moonlight moonlight;

	// Use this for initialization
	void Start () {
	    inHedge = new GameObject[]{GameObject.Find("Dock"), GameObject.Find("EstateWall"), GameObject.Find("Greenhouse"), GameObject.Find("shedModel")};
		inTunnels = new GameObject[] {GameObject.Find("EstateWall"), GameObject.Find("Shed"), GameObject.Find("Greenhouse"), GameObject.Find("Hedgemaze")};
        inShed = new GameObject[] {GameObject.Find("Greenhouse"), GameObject.Find("Hedgemaze"), GameObject.Find("Dock")};

        for (int i = 0; i < inShed.Length; i++)
        {
            inShed[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "HedgeUnload")
        {
            for (int i = 0; i < inHedge.Length; i++)
            {
                inHedge[i].SetActive(false);
            }
        }

        if(col.name == "HedgeLoad")
        {
            for (int i = 0; i < inHedge.Length; i++)
            {
                inHedge[i].SetActive(true);
            }
        }

        if (col.name == "TunnelUnload" && tunnelLoaded == false)
        {
			Debug.Log("Unload");
            for (int i = 0; i < inTunnels.Length; i++)
            {
                inTunnels[i].SetActive(false);
            }

			tunnelLoaded = true;

			//No weather
			weather.StopWeather();

			//Change lighting
			moonlight.lightFadeOut = true;
        }

        if (col.name == "TunnelLoad" && tunnelLoaded == true)
        {
			Debug.Log("Load");
            for (int i = 0; i < inTunnels.Length; i++)
            {
                inTunnels[i].SetActive(true);
            }

			tunnelLoaded = false;

			//Weather
			weather.StartWeather();
			
			//Change lighting
			moonlight.lightFadeIn = true;
        }

        if (col.name == "ShedUnload")
        {
            for (int i = 0; i < inShed.Length; i++)
            {
                inShed[i].SetActive(false);
            }
        }

        if (col.name == "ShedLoad")
        {
            for (int i = 0; i < inShed.Length; i++)
            {
                inShed[i].SetActive(true);
            }
        }

    }
}
