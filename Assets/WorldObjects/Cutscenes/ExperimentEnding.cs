﻿using UnityEngine;
using System.Collections;

public class ExperimentEnding : MonoBehaviour
{

    private float fade = 0;
    private float blackFade = 0;
    public Font bark;
    public GUIText endingText;
    public GUITexture black;
    private float delta = .1f;
    private float blackDelta = .2f;
    private bool onlyOnce = true;

    private int step = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUI.skin.font = bark;
        GUI.skin.box.alignment = TextAnchor.LowerCenter;
        Color changing;
        this.GetComponent<GUITexture>().enabled = true;


        if (!(blackFade > 1))
        {

            changing = new Color(black.color.r, black.color.g, black.color.b, blackFade);
            //set the new color
            black.color = changing;
            //update the alpha value
            blackFade += blackDelta * Time.deltaTime;
        }

        switch (step)
        {
            case 0:
                endingText.text = "As you mix the ingredients in the bowl, \n an explosion of light knocks you onto the floor.";
                //create a new color with the changed alpha value
                changing = new Color(endingText.color.r, endingText.color.g, endingText.color.b, fade);

                endingText.enabled = true;
                //set the new color
                endingText.color = changing;
                //update the alpha value
                fade += delta * Time.deltaTime;

                if (onlyOnce)
                {
                    StartCoroutine(WaitToFadeOut());
                    onlyOnce = false;
                }

                if (fade <= 0)
                {
                    step++;
                    delta = -delta;
                    onlyOnce = true;
                }
                break;
            case 1:
                endingText.text = "You try to get up but you're stuck. \n You can’t move.";
                //create a new color with the changed alpha value
                changing = new Color(endingText.color.r, endingText.color.g, endingText.color.b, fade);

                endingText.enabled = true;
                //set the new color
                endingText.color = changing;
                //update the alpha value
                fade += delta * Time.deltaTime;

                if (onlyOnce)
                {
                    StartCoroutine(WaitToFadeOut());
                    onlyOnce = false;
                }

                if (fade <= 0)
                {
                    step++;
                    delta = -delta;
                    onlyOnce = true;
                }
                break;

            case 2:
                endingText.text = "Your eyes are heavy, you rest. \n When your eyes open, you realize you’re in a hospital room.";
                //create a new color with the changed alpha value
                changing = new Color(endingText.color.r, endingText.color.g, endingText.color.b, fade);

                endingText.enabled = true;
                //set the new color
                endingText.color = changing;
                //update the alpha value
                fade += delta * Time.deltaTime;

                if (onlyOnce)
                {
                    StartCoroutine(WaitToFadeOut());
                    onlyOnce = false;

                }

                if (fade <= 0)
                {
                    step++;
                    delta = -delta;
                    onlyOnce = true;

                }
                break;
            case 3:
                endingText.text = "There’s a table with flowers, balloons, and various get well-soon stuff.";
                //create a new color with the changed alpha value
                changing = new Color(endingText.color.r, endingText.color.g, endingText.color.b, fade);

                endingText.enabled = true;
                //set the new color
                endingText.color = changing;
                //update the alpha value
                fade += delta * Time.deltaTime;

                if (onlyOnce)
                {
                    StartCoroutine(WaitToFadeOut());
                    onlyOnce = false;
                }

                if (fade <= 0)
                {
                    step++;
                    delta = -delta;
                    onlyOnce = true;

                }
                break;
            case 4:
                endingText.text = "It’s storming outside. \n The lightning flashes and you see a gnome outside the window";
                //create a new color with the changed alpha value
                changing = new Color(endingText.color.r, endingText.color.g, endingText.color.b, fade);

                endingText.enabled = true;
                //set the new color
                endingText.color = changing;
                //update the alpha value
                fade += delta * Time.deltaTime;

                if (onlyOnce)
                {
                    StartCoroutine(WaitToFadeOut());
                    onlyOnce = false;
                }

                if (fade <= 0)
                {
                    Application.LoadLevel("MainMenu");

                }
                break;
        }
    }


    public IEnumerator WaitToFadeOut()
    {
        //Wait time
        yield return new WaitForSeconds(5f);
        delta = -delta;

    }
}