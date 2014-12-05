using UnityEngine;
using System.Collections;

public class menuCam : MonoBehaviour 
{
	public GameObject cam;
	public GameObject keyText;

	public int currentMenu = 0; //Menu index
	private bool camMoving = false;

	private int buttonIndex = 0; //Button index

	void Update()
	{
        //Check is all animations are done playing
		if( !animation.IsPlaying("titleToMain") &&
		    !animation.IsPlaying("mainToLeaderboards") &&
            !animation.IsPlaying("mainToCredits"))
		{
			camMoving = false; //Cam is not moving

			//Display gui elements
			keyText.guiText.enabled = true;
		}

		if(Input.anyKeyDown && currentMenu == 0 && camMoving == false)
		{
            TitleTransition();
		}
        else if(Input.GetKey(KeyCode.RightArrow) && currentMenu == 1)
        {
			LeaderboardsTransition();
        }
        else if(Input.GetKey(KeyCode.LeftArrow) && currentMenu == 1)
        {
            CreditsTransition();   
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && currentMenu == 2)
        {
			LeaderboardsTransition();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && currentMenu == 3)
        {
            CreditsTransition();
        }
	}

    public void TitleTransition()
    {
        if (camMoving == false)
        {
            if (currentMenu == 0)
            {
                keyText.guiText.enabled = false; //Hide GUI text

                animation["titleToMain"].speed = 1.0f; //Play animation fowards
                animation["titleToMain"].time = 0; //Start from beginning of animation
                cam.animation.Play("titleToMain"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 1; //Set current menu index
				keyText.guiText.text = "[Left Arrow] Credits || [Right Arrow] Leaderboards"; //Set new GUI text
            }
        }
    }

	public IEnumerator GameStartTimer(float waitTime)
	{	
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);
		
		//Start Game
		Application.LoadLevel("HomeSweetGnome");
	}

	public void PlayTransition()
	{
		if(camMoving == false)
		{
			if(currentMenu == 1)
			{
				keyText.guiText.enabled = false; //Hide GUI text

				animation["mainToShed"].speed = 1.0f; //Play animation fowards
				animation["mainToShed"].time = 0; //Start from beginning of animation
				cam.animation.Play("mainToShed"); //Play
				GameObject.Find("FrontGate").GetComponent<Animation>().Play("OpenFrontGate");

				camMoving = true; //Cam is moving

				//Load the game after animation length
				StartCoroutine(GameStartTimer(animation["mainToShed"].length));
			}
		}
	}

	public void LoadTransition()
	{
		if(camMoving == false)
		{
			if(currentMenu == 1)
			{
				keyText.guiText.enabled = false; //Hide GUI text
				
				animation["mainQuick"].speed = 1.0f; //Play animation fowards
				animation["mainQuick"].time = 0; //Start from beginning of animation
				cam.animation.Play("mainQuick"); //Play
				GameObject.Find("FrontGate").GetComponent<Animation>().Play("OpenFrontGate");
				
				camMoving = true; //Cam is moving
				
				//Load the game after animation length
				StartCoroutine(GameStartTimer(animation["mainQuick"].length));
			}
		}
	}

	public void LeaderboardsTransition()
    {
        if(camMoving == false)
        {
            if(currentMenu == 1)
            {
                keyText.guiText.enabled = false; //Hide GUI text
				animation["mainToLeaderboards"].speed = 1.0f; //Play animation fowards
				animation["mainToLeaderboards"].time = 0; //Start from beginning of animation
				cam.animation.Play("mainToLeaderboards"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 2; //Set current menu index
                keyText.guiText.text = "[Left Arrow] Back To Main"; //Set new GUI text
            }
            else if(currentMenu == 2)
            {
                keyText.guiText.enabled = false; //Hide GUI text
				animation["mainToLeaderboards"].speed = -1.0f; //Play animation backwards
				animation["mainToLeaderboards"].time = animation["mainToLeaderboards"].length; ; //Start from end of animation
				cam.animation.Play("mainToLeaderboards"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 1; //Set current menu index
				keyText.guiText.text = "[Left Arrow] Credits || [Right Arrow] Leaderboards"; //Set new GUI text
            }
        }
    }

    public void CreditsTransition()
    {
        if(camMoving == false)
        {
            if(currentMenu == 1)
            {
                keyText.guiText.enabled = false; //Hide GUI text
                animation["mainToCredits"].speed = 1.0f; //Play animation fowards
                animation["mainToCredits"].time = 0; //Start from beginning of animation
                cam.animation.Play("mainToCredits"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 3; //Set current menu index
                keyText.guiText.text = "[Right Arrow] Back To Main"; //Set new GUI text
            }
            else if(currentMenu == 3)
            {
                keyText.guiText.enabled = false; //Hide GUI text
                animation["mainToCredits"].speed = -1.0f; //Play animation backwards
                animation["mainToCredits"].time = animation["mainToCredits"].length; ; //Start from end of animation
                cam.animation.Play("mainToCredits"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 1; //Set current menu index
				keyText.guiText.text = "[Left Arrow] Credits || [Right Arrow] Leaderboards"; //Set new GUI text
            }
        }
    }
}
