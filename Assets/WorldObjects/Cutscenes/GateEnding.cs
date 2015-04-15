using UnityEngine;
using System.Collections;

public class GateEnding : MonoBehaviour {

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
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.skin.font = bark;
        GUI.skin.box.alignment = TextAnchor.LowerCenter;
        Color changing;
        black.enabled = true;
        
        if(!(blackFade > 1))
        {
            
            changing = new Color(black.color.r, black.color.g, black.color.b, blackFade);
            //set the new color
            black.color = changing;
            //update the alpha value
            blackFade += blackDelta * Time.deltaTime;
        }
        else if (GameObject.Find("OffAtEnd") != null)
        {
            GameObject.Find("Main Camera").transform.parent = null;
            GameObject.Find("OffAtEnd").SetActive(false);
            EndParent.endParent.GetTime();

        }

        switch (step)
        {
            case 0:
                endingText.text = "As you run out of the gate you hear a car horn blaring. \nIt’s so loud. Where is it coming from? \n";
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
                endingText.text = "You start to get a splitting headache. \n Bright lights flash. You close your eyes.";
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
                endingText.text = "You open your eyes. You aren’t where you were a second ago. \n You’re lying next to a crashed car, head still pounding.";
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
                endingText.text = "As your vision clears you see something lying in mud. \n You here a faint voice, “Hey buddy are you okay!?”";
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
                endingText.text = "The mud washes off the object. \n A gnome.";
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
                    EndParent.enterName = true;
                    step++;
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
