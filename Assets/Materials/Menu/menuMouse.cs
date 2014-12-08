using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class menuMouse : MonoBehaviour 
{
    private menuCam menuAnim;

	void Start()
	{
        menuAnim = GameObject.Find("Main Camera").GetComponent<menuCam>();

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
			menuAnim.PlayTransition();
        }
        else if (this.gameObject.name == ("Continue"))
        {
			PlayerPrefs.SetInt("LoadGame", 1);
			menuAnim.LoadTransition();
        }
        else if (this.gameObject.name == ("Leaderboards"))
        {
			menuAnim.LeaderboardsTransition();
        }
        else if (this.gameObject.name == ("Credits"))
        {
            menuAnim.CreditsTransition();
        }
        else if (this.gameObject.name == ("Quit"))
        {
            Application.Quit();
        }
        else if (this.gameObject.name == ("Back"))
        {
            if(menuAnim.currentMenu == 2)
            {
				menuAnim.LeaderboardsTransition();
            }
            else if (menuAnim.currentMenu == 3)
            {
                menuAnim.CreditsTransition();
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
