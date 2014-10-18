using UnityEngine;
using System.Collections;

public class GnomeParent : MonoBehaviour {

    public GameObject target;

    public GameObject gnomeLvl1;
    public GameObject gnomeLvl2;
    public GameObject gnomeLvl3;
    private GameObject currentModel;

    private float playerSanity;
    private float playerSanityMax;

	// Use this for initialization
	void Start () {

        playerSanity = target.GetComponent<Player>().sanity;
        playerSanityMax = target.GetComponent<Player>().maxSanity;

        currentModel = Instantiate(gnomeLvl1, new Vector3(3,17,74), transform.rotation) as GameObject;
        currentModel.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
        ChangeModel();
	}

    private void ChangeModel()
    {
        playerSanity = target.GetComponent<Player>().sanity;
        playerSanityMax = target.GetComponent<Player>().maxSanity;
        float sanityPercentage = playerSanity / playerSanityMax;

        if(sanityPercentage > .7)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        if(sanityPercentage < .7)
        {
            Vector3 newPos = new Vector3(currentModel.transform.position.x, 21.4f, currentModel.transform.position.z);
            Quaternion newRot = currentModel.transform.rotation;
            Destroy(currentModel);
            GameObject thisModel = Instantiate(gnomeLvl2, newPos, newRot) as GameObject;
            thisModel.transform.parent = transform;
            currentModel = thisModel;
            Debug.Log("Less thant 70%");

        }
        if(sanityPercentage < .4)
        {
            Vector3 newPos = new Vector3(currentModel.transform.position.x, 25.7f, currentModel.transform.position.z);
            GameObject thisModel = Instantiate(gnomeLvl3, newPos, currentModel.transform.rotation) as GameObject;
            thisModel.transform.parent = transform;
            Destroy(currentModel);
            currentModel = thisModel;
            Debug.Log("Less thant 40%");

        }

        //Destroy(currentModel);
        //thisModel.transform.parent = transform;
        //currentModel = thisModel;

    }
}
