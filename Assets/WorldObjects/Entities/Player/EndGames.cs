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
    private float fadeIn = 0;
    private float pauseFadeTime = 4;
    private bool pauseFade = false;
    private bool switchFade = false;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    IEnumerator WaitToReload(float waitTime)
    {
        //Wait before loading the main menu
        yield return new WaitForSeconds(waitTime);

        //Load the main menu
        Application.LoadLevel("MainMenu");
    }

    public void Escape()
    {
        playerEscaped = true;
        StartCoroutine(WaitToReload(7.0F));
    }

    public void Experiment()
    {
        experimentComplete = true;

        StartCoroutine(WaitToReload(5.0F));
    }


    void OnGUI()
    {
        if (experimentComplete)
        {
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
            //create a new color with the changed alpha value
            Color changing = new Color(winTextEscaped.color.r, winTextEscaped.color.g, winTextEscaped.color.b, fadeIn);
            winTextEscaped.enabled = true;
            GetComponent<FadeToBlack>().enabled = true;
            //set the new color
            winTextEscaped.color = changing;
            //update the alpha value
            fadeIn += .03f * Time.deltaTime;
        }

        if (playerSlept)
        {
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
            //create a new color with the changed alpha value
            Color changing = new Color(deathTextFall.color.r, deathTextFall.color.g, deathTextFall.color.b, fadeIn);
            deathTextFall.enabled = true;

            //set the new color
            deathTextFall.color = changing;
            //update the alpha value
            fadeIn += .1f * Time.deltaTime;

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
}
