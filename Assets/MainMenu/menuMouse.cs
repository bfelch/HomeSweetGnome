using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class menuMouse : MonoBehaviour 
{
    private scrMenuController menuController; //Menu controller script
	public Color32 defaultColor;

	//Runs on start
	void Start()
	{
		//Get menu controller script
        menuController = GameObject.Find("Main Camera").GetComponent<scrMenuController>();

        renderer.material.color = new Color32(222, 219, 212, 255);

		//Ensure game is not frozen
		Time.timeScale = 1.0F;

		//Show mouse
		Screen.lockCursor = false;
	}

	void OnMouseDown()
	{
        renderer.material.color = new Color32(151, 146, 132, 255);
	}

    void OnMouseUp()
    {
        if (this.gameObject.name == ("NewGame"))
        {
			PlayerPrefs.SetInt("LoadGame", 0);
			menuController.PlayTransition();
        }
        else if (this.gameObject.name == ("Continue"))
        {
			PlayerPrefs.SetInt("LoadGame", 1);
			menuController.LoadTransition();
        }
        else if (this.gameObject.name == ("Leaderboards"))
        {
			menuController.LeaderboardsTransition();
        }
        else if (this.gameObject.name == ("Credits"))
        {
            menuController.CreditsTransition();
        }
        else if (this.gameObject.name == ("Quit"))
        {
            Application.Quit();
        }
        else if (this.gameObject.name == ("Back"))
        {
            if(menuController.currentMenu == 2)
            {
				menuController.LeaderboardsTransition();
            }
            else if (menuController.currentMenu == 3)
            {
                menuController.CreditsTransition();
            }
        }

		renderer.material.color = new Color32(222, 219, 212, 255);
    }
	
	void OnMouseEnter()
	{
        renderer.material.color = new Color32(191, 187, 179, 255);
	}
	
	void OnMouseExit()
	{
        renderer.material.color = new Color32(222, 219, 212, 255);
	}
}
