using UnityEngine;
using System.Collections;

public class menuMouse : MonoBehaviour 
{
    private menuCam menuAnim;

	void Start()
	{
        menuAnim = GameObject.Find("Main Camera").GetComponent<menuCam>();

		renderer.material.color = Color.gray;

		//Ensure game is not frozen
		Time.timeScale = 1.0F;
		//Show mouse
		Screen.lockCursor = false;
	}

	void OnMouseDown()
	{
        renderer.material.color = Color.red;
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
