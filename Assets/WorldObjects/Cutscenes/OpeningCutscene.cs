using UnityEngine;
using System.Collections;

public class OpeningCutscene : MonoBehaviour
{

    private float fade = 0;
    private float blackFade = 0;
    public Font bark;
    public GUIText endingText;
    private float delta = .1f;
    private float blackDelta = .2f;
    private bool onlyOnce = true;
    public AudioSource sound;
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
    }

    void OnGUI()
    {
        GUI.skin.font = bark;
        GUI.skin.box.alignment = TextAnchor.LowerCenter;
        Color changing;

        switch (step)
        {
            case 0:
                endingText.text = "You must rest.";
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
                endingText.text = "Your mind is filled with flashes of memories. \nBeeping, white linens, bright lights, a car horn, gnomes.";
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
                endingText.text = "You see a house, a window. \nIs that a figure moving inside?";
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
                endingText.text = "You are confused. You are lost.\nYou need to get out of here.";
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
                endingText.text = "You reach a door. You walk down stairs. \n You are falling. ";
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
            case 5:
                endingText.text = "Everything goes black. ";
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
                    Application.LoadLevel("HomeSweetGnome");
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
