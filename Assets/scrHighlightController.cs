using UnityEngine;
using System.Collections;

public class scrHighlightController : MonoBehaviour 
{
	Material outline1;
	Material outline2;

	// Use this for initialization
	void Start () 
	{
		outline1 = Resources.Load("Outline1") as Material;
		outline2 = Resources.Load("Outline2") as Material;

		StartCoroutine(DelayedStart(2.0F));
	}

	public void HighlightOrange(GameObject objToHightlight)
	{
		//Get objects mesh renderer
		MeshRenderer currentMesh = objToHightlight.GetComponent<MeshRenderer>();
		//Save current mesh material
		Material currentMat = currentMesh.material;
		//Create new array of materials of size 2
		Material[] mats = new Material[2];
		//Set materials to the current one and the outline
		mats[0] = currentMat;
		mats[1] = outline1;
		//set the materials to the currentMesh
		currentMesh.materials = mats;
	}

	public void HightlightWhite(GameObject objToHighlight)
	{
		MeshRenderer currentMesh;
		//get the mesh renderer
		if(objToHighlight.gameObject.name == "GargoyleHead")
		{
			currentMesh = objToHighlight.GetComponentInChildren<MeshRenderer>();
		}
		else
		{
			currentMesh = objToHighlight.GetComponent<MeshRenderer>();
		}
		//Save current mesh material
		Material currentMat = currentMesh.material;
		//Create new array of materials of size 2
		Material[] mats = new Material[2];
		//Set materials to the current one and the outline
		mats[0] = currentMat;
		mats[1] = outline2;
		//set the materials to the currentMesh
		currentMesh.materials = mats;
	}

	public void Unhighlight(GameObject objToUnhighlight)
	{
		//get the mesh renderer
		MeshRenderer currentMesh = objToUnhighlight.GetComponent<MeshRenderer>();
		//save the first material
		Material current = currentMesh.materials[0];
		//create new array of materials of size 1
		Material[] mats = new Material[1];
		//set the material 
		mats[0] = current;
		//set the material to the current mats
		currentMesh.materials = mats;
	}
	
	public IEnumerator DelayedStart(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);
		
		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().HighlightOrange(GameObject.Find("LeftGateLock"));
		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().HighlightOrange(GameObject.Find("RightGateLock"));

		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().HighlightOrange(GameObject.Find("MotorHatch"));
	}
}
