using UnityEngine;
using System.Collections;
using System.IO;

/* Controller for main menu.  (Handles menu transitions) */
public class scrMenuController : MonoBehaviour 
{
	public GameObject mainCam; //The main camera
	public GameObject keyText; //Onscreen key text

    private GameObject startMenu;
    private GameObject mainMenu;
    private GameObject leaderMenu;
    private GameObject creditsMenu;

	public int currentMenu = 0; //Menu index
	private bool camMoving = false;

	private int buttonIndex = 0; //Button index

    void Awake()
    {
        startMenu = GameObject.Find("Start Menu");
        mainMenu = GameObject.Find("Main Menu");
        leaderMenu = GameObject.Find("Leaderboard Menu");
        creditsMenu = GameObject.Find("Credits Menu");

		//Set leaderboard text
		if (File.Exists(Application.persistentDataPath + "/leaderboards.dat"))
		{
			SaveLoad.loadLeaderboards();
		}
		else
		{
			SaveLoad.CreateLeaderboard();
			SaveLoad.loadLeaderboards();
		}
		
		DisplayLeaderboards();
    }

	void DisplayLeaderboards()
	{
		for (int i = 0; i < 5; i++)
		{
			GameObject.Find((i+1).ToString()).GetComponent<TextMesh>().text = (i + 1) + ". " + SaveLoad.leaderboardNames[i] + " - " + EndGames.getTimeString(SaveLoad.leaderboardTimes[i]);
		}
	}

	void Start()
    {
        startMenu.SetActive(true);
        mainMenu.SetActive(false);
        leaderMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

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
                //Display appropriate menu
                mainMenu.SetActive(true);

                keyText.guiText.enabled = false; //Hide GUI text

                animation["titleToMain"].speed = 1.0f; //Play animation fowards
                animation["titleToMain"].time = 0; //Start from beginning of animation
                mainCam.animation.Play("titleToMain"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 1; //Set current menu index

				keyText.guiText.text = "[Left Arrow] Credits || [Right Arrow] Leaderboard"; //Set new GUI text
            }
        }
    }

	public IEnumerator GameStartTimer(float waitTime)
	{	
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);
		
		//Start Game
		Application.LoadLevel("OpeningCut");
	}

	public void PlayTransition()
	{
		if(camMoving == false)
		{
			if(currentMenu == 1)
			{
                //Display appropriate menu
                mainMenu.SetActive(false);

				keyText.guiText.enabled = false; //Hide GUI text

				animation["mainToShed"].speed = 1.0f; //Play animation fowards
				animation["mainToShed"].time = 0; //Start from beginning of animation
				mainCam.animation.Play("mainToShed"); //Play
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
                //Display appropriate menu
                mainMenu.SetActive(false);

				keyText.guiText.enabled = false; //Hide GUI text
				
				animation["mainQuick"].speed = 1.0f; //Play animation fowards
				animation["mainQuick"].time = 0; //Start from beginning of animation
				mainCam.animation.Play("mainQuick"); //Play
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
                //Display appropriate menu
                mainMenu.SetActive(false);
                leaderMenu.SetActive(true);

                keyText.guiText.enabled = false; //Hide GUI text
				animation["mainToLeaderboards"].speed = 1.0f; //Play animation fowards
				animation["mainToLeaderboards"].time = 0; //Start from beginning of animation
				mainCam.animation.Play("mainToLeaderboards"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 2; //Set current menu index
                keyText.guiText.text = "[Left Arrow] Back To Main"; //Set new GUI text
            }
            else if(currentMenu == 2)
            {
                //Display appropriate menu
                leaderMenu.SetActive(false);
                mainMenu.SetActive(true);

                keyText.guiText.enabled = false; //Hide GUI text
				animation["mainToLeaderboards"].speed = -1.0f; //Play animation backwards
				animation["mainToLeaderboards"].time = animation["mainToLeaderboards"].length; ; //Start from end of animation
				mainCam.animation.Play("mainToLeaderboards"); //Play

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
                //Display appropriate menu
                mainMenu.SetActive(false);
                creditsMenu.SetActive(true);

                keyText.guiText.enabled = false; //Hide GUI text
                animation["mainToCredits"].speed = 1.0f; //Play animation fowards
                animation["mainToCredits"].time = 0; //Start from beginning of animation
                mainCam.animation.Play("mainToCredits"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 3; //Set current menu index
                keyText.guiText.text = "[Right Arrow] Back To Main"; //Set new GUI text
            }
            else if(currentMenu == 3)
            {
                //Display appropriate menu
                creditsMenu.SetActive(false);
                mainMenu.SetActive(true);

                keyText.guiText.enabled = false; //Hide GUI text
                animation["mainToCredits"].speed = -1.0f; //Play animation backwards
                animation["mainToCredits"].time = animation["mainToCredits"].length; //Start from end of animation
                mainCam.animation.Play("mainToCredits"); //Play

                camMoving = true; //Cam is moving
                currentMenu = 1; //Set current menu index
				keyText.guiText.text = "[Left Arrow] Credits || [Right Arrow] Leaderboard"; //Set new GUI text
            }
        }
    }
}
