using UnityEngine;
using System.Collections;

public class BoatEnding : MonoBehaviour
{

    private float fade = 0;
    private float blackFade = 0;
    public Font bark;
    public GUIText endingText;
    public GUITexture black;
    public AudioSource sound;
    private float delta = .1f;
    private float blackDelta = .2f;
    private bool onlyOnce = true;

    private bool playAudioOnce = true;

    private int step = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!sound.isPlaying && playAudioOnce)
        {
            sound.Play();
            playAudioOnce = false;
        }

        if ((blackFade >= 1) && GameObject.Find("OffAtEnd") != null)
        {

            GameObject.Find("Main Camera").transform.parent = null;
            GameObject.Find("OffAtEnd").SetActive(false);
            GameObject.Find("EndingScripts").GetComponent<AudioListener>().enabled = true;

            EndParent.endParent.GetTime();
        }
    }

    void OnGUI()
    {
        GUI.skin.font = bark;
        GUI.skin.box.alignment = TextAnchor.LowerCenter;
        Color changing;
        black.enabled = true;


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
                endingText.text = "As you drive out into the sea, the storm settles and the skies clear. \n It's beautiful.";
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
                endingText.text = "There is a bright light approaching from the distance. \n Your vision begins to blur. ";
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
                endingText.text = "As your vision clears you realize you’re in a morgue. \n You try to scream but nothing comes out.";
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
                endingText.text = "There’s a reflection in the nearby metal cadaver compartment door. \n A gnome.";
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
