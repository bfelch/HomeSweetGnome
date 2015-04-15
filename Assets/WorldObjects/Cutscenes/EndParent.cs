using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndParent : MonoBehaviour
{
    public bool playerSlept;
    public bool playerFell;
    public bool playerEscaped;
    public bool experimentComplete;
    public static bool enterName = false;
    public static bool showLeaderboards = false;
    public bool experimentWin = false;
    private float time = 0;
    private bool gotTime = false;
    public PlayerInteractions playerInt;

    public static GameObject endingText;
    public static EndParent endParent;

    // Use this for initialization
    void Start()
    {
        endingText = GameObject.Find("EndingText");
        endParent = this.GetComponent<EndParent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        if (experimentComplete)
        {
            endingText.GetComponent<ExperimentEnding>().enabled = true;
        }

        if (playerEscaped)
        {
            endingText.GetComponent<GateEnding>().enabled = true;
        }

        if (playerSlept)
        {
            endingText.GetComponent<GnomeEnding>().enabled = true;
        }

        if (playerFell)
        {
            endingText.GetComponent<FallEnding>().enabled = true;
        }

        if (enterName)
        {
            playerInt.gameObject.SetActive(true);
            playerInt.gameObject.transform.parent = null;

            playerInt.removePlus = true;
            GUI.skin.box.alignment = TextAnchor.UpperCenter;
            GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 60, 400, Screen.height));
            Screen.lockCursor = false;
            GUI.backgroundColor = Color.clear;
            GUI.Box(new Rect(0, 0, 400, 30), "Please Enter Your Name:");
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
            GUI.BeginGroup(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 125, 500, Screen.height));
            GUI.Box(new Rect(0, 0, 500, 30), "Leaderboards");
            SaveLoad.loadLeaderboards();

            int height = 40;
            for (int i = 0; i < 5; i++)
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

    

    public void GetTime()
    {
        //if(!gotTime)
        {
            gotTime = true;
            time = (int)(time + Time.timeSinceLevelLoad);
            playerInt.timePlayed = time;
        }

    }

    public static string getTimeString(float time)
    {
        int second = (int)time;
        int minute = second / 60;
        int hour = minute / 60;
        second = second % 60;

        return hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
    }

   
}
