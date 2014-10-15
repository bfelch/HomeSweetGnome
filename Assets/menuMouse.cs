using UnityEngine;
using System.Collections;

public class menuMouse : MonoBehaviour 
{
	void Start()
	{
		renderer.material.color = Color.gray;
	}

	void OnMouseDown()
	{
		if(this.gameObject.name == ("PlayGame"))
		{
			Application.LoadLevel("HomeSweetGnome");
		}
		else if(this.gameObject.name == ("LoadGame"))
		{
			Debug.Log("Load Game Clicked");
		}
		else if(this.gameObject.name == ("Options"))
		{
			Debug.Log("Options Clicked");
		}
		else if(this.gameObject.name == ("Credits"))
		{
			Debug.Log("Credits Clicked");
		}
	}
	
	void OnMouseEnter()
	{
		renderer.material.color = Color.white;
	}
	
	void OnMouseExit()
	{
		renderer.material.color = Color.gray;
	}
}
