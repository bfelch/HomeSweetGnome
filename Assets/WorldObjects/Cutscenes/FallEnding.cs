using UnityEngine;
using System.Collections;

public class FallEnding : MonoBehaviour
{

    private float fade = 0;
    private float blackFade = 0;
    public Font bark;
    public GUIText endingText;
    public GUITexture black;
    private float delta = .1f;
    private float blackDelta = .5f;
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
                endingText.text = "You have fallen. \n The gnomes have won.";
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
                    Application.LoadLevel("MainMenu");
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
