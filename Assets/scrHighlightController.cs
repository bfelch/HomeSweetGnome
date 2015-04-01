using UnityEngine;
using System.Collections;

public class scrHighlightController : MonoBehaviour 
{
	public static Material outline1;
	public static Material outline2;
	public static Material outlinePurple;
	public static Material outlineBlue;
	public static Material outlineGreen;
	public static Material outlineRed;

	// Use this for initialization
	void Start () 
	{
		outline1 = Resources.Load("Outline1") as Material;
		outline2 = Resources.Load("Outline2") as Material;
		outlinePurple = Resources.Load("OutlinePurple") as Material;
		outlineBlue = Resources.Load("OutlineBlue") as Material;
		outlineGreen = Resources.Load("OutlineGreen") as Material;
		outlineRed = Resources.Load("OutlineRed") as Material;

		StartCoroutine(DelayedStart(2.0F));
	}

	public void Highlight(GameObject objToHightlight, Material mat)
	{
		//Get objects mesh renderer
		MeshRenderer currentMesh = objToHightlight.GetComponent<MeshRenderer>();
		//Save current mesh material
		Material currentMat = currentMesh.material;
		//Create new array of materials of size 2
		Material[] mats = new Material[2];
		//Set materials to the current one and the outline
		mats[0] = currentMat;
		mats[1] = mat;
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

		this.Highlight(GameObject.Find("PurpleKeyhole"), outlinePurple);
		this.Highlight(GameObject.Find("BlueKeyhole"), outlineBlue);
		this.Highlight(GameObject.Find("GreenKeyhole"), outlineGreen);
		this.Highlight(GameObject.Find("RedKeyhole"), outlineRed);

		this.Highlight(GameObject.Find("MotorHatch"), outline2);
	}
}
