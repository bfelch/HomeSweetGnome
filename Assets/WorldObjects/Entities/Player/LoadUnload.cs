using UnityEngine;
using System.Collections;

public class LoadUnload : MonoBehaviour {

    private GameObject[] inHedge;
    private GameObject[] inTunnels;
    private GameObject[] inShed;
	// Use this for initialization
	void Start () {
	    inHedge = new GameObject[]{GameObject.Find("Dock"), GameObject.Find("EstateWall"), GameObject.Find("Greenhouse"), GameObject.Find("shedModel")};
	    inTunnels = new GameObject[] {GameObject.Find("EstateWall"), GameObject.Find("shedModel"), GameObject.Find("Greenhouse"), GameObject.Find("Hedgemaze")};
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

        if (col.name == "TunnelUnload")
        {
            for (int i = 0; i < inTunnels.Length; i++)
            {
                inTunnels[i].SetActive(false);
            }
        }

        if (col.name == "TunnelLoad")
        {
            for (int i = 0; i < inTunnels.Length; i++)
            {
                inTunnels[i].SetActive(true);
            }
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
