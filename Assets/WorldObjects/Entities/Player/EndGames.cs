using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGames : MonoBehaviour {
    public GUIText deathTextSleep;
    public GUIText deathTextFall;
    public GUIText winTextEscaped;
    public GUIText winTextExperiment;
    public bool playerSlept;
    public bool playerFell;
    public bool playerEscaped;
    public bool experimentComplete;
    public bool enterName = false;
    public bool showLeaderboards = false;
    private float fadeIn = 0;
    private float pauseFadeTime = 4;
    private bool pauseFade = false;
    private bool switchFade = false;
    private float time = 0;
    private bool gotTime = false;
    public static GameObject[] dockGnomes;
    public PlayerInteractions playerInt;

	// Use this for initialization
	void Start () {
        deathTextSleep = GameObject.Find("DeathTextSleep").guiText;
        deathTextSleep.enabled = false;

        deathTextFall = GameObject.Find("DeathTextFall").guiText;
        deathTextFall.enabled = false;

        winTextEscaped = GameObject.Find("WinTextEscape").guiText;
        winTextEscaped.enabled = false;

        winTextExperiment = GameObject.Find("WinTextExperiment").guiText;
        winTextExperiment.enabled = false;

        playerSlept = false;
        playerFell = false;
        time = GetComponent<PlayerInteractions>().timePlayed;
        dockGnomes = new GameObject[] { GameObject.Find("DockGnome1"), GameObject.Find("DockGnome2"), GameObject.Find("DockGnome3") };
        for (int i = 0; i < dockGnomes.Length; i++)
        {
            dockGnomes[i].SetActive(false);
        }

        playerInt = GameObject.Find("Player").GetComponent<PlayerInteractions>();
	}
	
	// Update is called once per frame
	void Update () {
	}


    IEnumerator WaitToReload(float waitTime)
    {
        //Wait before loading the main menu
        yield return new WaitForSeconds(waitTime);

        enterName = true;
        //Load the main menu
        //Application.LoadLevel("MainMenu");
    }

    public void Escape()
    {
        playerEscaped = true;
        GetTime();
        StartCoroutine(WaitToReload(8.0F));
    }

    public void Experiment()
    {
        experimentComplete = true;
        GetTime();
        StartCoroutine(WaitToReload(5.0F));
    }


    void OnGUI()
    {
        if (experimentComplete)
        {
            int second = (int)time;
            int minute = second / 60;
            int hour = minute / 60;
            winTextExperiment.text = "Experiment Success! \n You completed the experiment in " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
            //create a new color with the changed alpha value
            Color changing = new Color(winTextExperiment.color.r, winTextExperiment.color.g, winTextExperiment.color.b, fadeIn);

            winTextExperiment.enabled = true;
            //set the new color
            winTextExperiment.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
        }

        if (playerEscaped)
        {

            int second = (int)time;
            int minute = second / 60;
            int hour = minute / 60;
            winTextEscaped.text = "You escaped. \n You made it out in " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");

            //create a new color with the changed alpha value
            Color changing = new Color(winTextEscaped.color.r, winTextEscaped.color.g, winTextEscaped.color.b, fadeIn);
            winTextEscaped.enabled = true;
            GetComponent<FadeToBlack>().enabled = true;
            //set the new color
            winTextEscaped.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
        }

        if (playerSlept)
        {

            int second = (int)time;
            int minute = second / 60;
            int hour = minute / 60;
            deathTextSleep.text = "You fell asleep... \n You stayed awake for " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");

            //create a new color with the changed alpha value
            Color changing = new Color(deathTextSleep.color.r, deathTextSleep.color.g, deathTextSleep.color.b, fadeIn);
            deathTextSleep.enabled = true;

            //set the new color
            deathTextSleep.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
        }

        if (playerFell)
        {

            int second = (int)time;
            int minute = second / 60;
            int hour = minute / 60;
            deathTextFall.text = "You fell to your death... \n You lasted for " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");

            //create a new color with the changed alpha value
            Color changing = new Color(deathTextFall.color.r, deathTextFall.color.g, deathTextFall.color.b, fadeIn);
            deathTextFall.enabled = true;

            //set the new color
            deathTextFall.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;
        }

        if(enterName)
        {
            playerInt.removePlus = true;
            GUI.skin.box.alignment = TextAnchor.UpperCenter;
            GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height/2 - 60, 400, Screen.height));
            Screen.lockCursor = false;
            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(0,0, 400, 30), "Please Enter Your Name:");
            GUI.backgroundColor = Color.white;

            PlayerInteractions.playerName = GUI.TextField(new Rect(100, 40, 200, 40), PlayerInteractions.playerName, 15);

            if (GUI.Button(new Rect(100, 90, 200, 30), "Submit"))
            {
                SaveLoad.saveLeaderboard();
                showLeaderboards = true;
                enterName = false;
            }
            GUI.EndGroup();
        }
        if (showLeaderboards)
        {
            GUI.skin.box.alignment = TextAnchor.UpperCenter;
            GUI.BeginGroup(new Rect(Screen.width / 2 - 250, Screen.height/2 - 125, 500, Screen.height));
            GUI.Box(new Rect(0,0, 500, 30), "Leaderboards");
            SaveLoad.loadLeaderboards();
            
            int height = 40;
            for (int i = 0; i < 5; i++ )
            {
                GUI.Box(new Rect(0, height, 500, 30), (i + 1) + ". " + SaveLoad.leaderboardNames[i] + " - " + getTimeString(SaveLoad.leaderboardTimes[i]));
                height += 30;
            }

            if (PlayerInteractions.playerName == "NotOnLeaderboard")
            {
                GUI.Box(new Rect(0, 200, 500, 50), "Unfortunately, you didn't make it onto the leaderboards. \n Better luck next time!");

            }
            if (GUI.Button(new Rect(150, 250, 200, 30), "Main Menu"))
            {
                showLeaderboards = false;
                enterName = false;
                Application.LoadLevel("MainMenu");
            }
            GUI.EndGroup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fall")
        {
            playerFell = true;

            StartCoroutine(WaitToReload(5.0F));
        }
        if (other.name == "GateEndGame")
        {
            Escape();
        }
    }

    public void GetTime()
    {
        //if(!gotTime)
        {
            gotTime = true;
            time = (int)(time + Time.timeSinceLevelLoad);
            playerInt.timePlayed = time;
        }

    }

    string getTimeString(float time)
    {

        int second = (int)time;
        int minute = second / 60;
        int hour = minute / 60;

        return hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
    }
}
