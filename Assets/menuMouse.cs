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
        renderer.material.color = Color.red;
	}

    void OnMouseUp()
    {
        if (this.gameObject.name == ("PlayGame"))
        {
            Application.LoadLevel("HomeSweetGnome");
            PlayerPrefs.SetInt("LoadGame", 0);
        }
        else if (this.gameObject.name == ("LoadGame"))
        {
            PlayerPrefs.SetInt("LoadGame", 1);
            Application.LoadLevel("HomeSweetGnome");
        }
        else if (this.gameObject.name == ("Options"))
        {
            Debug.Log("Options Clicked");
        }
        else if (this.gameObject.name == ("Credits"))
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
