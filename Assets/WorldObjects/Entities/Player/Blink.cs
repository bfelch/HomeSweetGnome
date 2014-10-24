using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{
    public GameObject topLid;
    public GameObject bottomLid;
    public GameObject player;

    public float blinkTimer = 5.0f;
    public float openTimer = 10.0f;
    public float maxOpenTimer = 10.0f;
    public bool blink = false;
    private bool rechargeBlink = false;
    private bool playerDied = false;
    private bool gameEnded = false;

    private bool showGUI = false;
    //need player health and max health to adjust blink speed
    private float playerSanity;
    private float playerSanityMax;


    // Use this for initialization
    void Start()
    {
        //get lids and players health
        topLid = GameObject.Find("UpperEyeLid");
        bottomLid = GameObject.Find("LowerEyeLid");
        player = GameObject.Find("Player");
        playerSanity = gameObject.GetComponent<Player>().sanity;
        playerSanityMax = gameObject.GetComponent<Player>().maxSanity;

        if(PlayerPrefs.GetInt("LoadGame") != 1)
        {
            player.animation.Play("OpeningCut");
            topLid.animation.Play("OpeningUpperBlink");
            bottomLid.animation.Play("OpeningBottomBlink");
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerSanity = gameObject.GetComponent<Player>().sanity;
        playerDied = gameObject.GetComponent<Player>().playerDied;

        if (!playerDied)
        {
            BlinkMechanics();
            OpenEyes();
        }
        else
        {
            if(!gameEnded)
            {
                FallAsleep();
            }
        }
    }

    public void BlinkMechanics()
    {
        //adjust the speed depending on player's health
        topLid.animation["BlinkTopNew"].speed = ((playerSanity + 5) / playerSanityMax);
        bottomLid.animation["BlinkBottomNew"].speed = ((playerSanity + 5) / playerSanityMax);

        //decrease the blink timer
        blinkTimer -= Time.deltaTime;

        //once timer hits zero, blink and reset timer
        if (blinkTimer <= 0)
        {
            blinkTimer = 5.08f;
            topLid.animation.Play("BlinkTopNew");
            bottomLid.animation.Play("BlinkBottomNew");
            blink = true;
        }

        //set blink to false right after blinking and keep it false
        if (blinkTimer <= 5.0f)
        {
            blink = false;
        }
    }

    void OpenEyes()
    {
        if (openTimer <= 10 && !Input.GetKey(KeyCode.Q))
        {
            openTimer += Time.deltaTime;
            if (openTimer >= 5)
            {
                rechargeBlink = false;
            }
        }
        if (Input.GetKey(KeyCode.Q) && !rechargeBlink)
        {
            if (openTimer > 0)
            {
                openTimer -= Time.deltaTime;
                blinkTimer = 5;
            }
            if (openTimer <= 0)
            {
                topLid.animation.Play("BlinkTopNew");
                bottomLid.animation.Play("BlinkBottomNew");
                rechargeBlink = true;
            }
        }

    }

    void FallAsleep()
    {
        Debug.Log("Fall Asleep");
        if (!topLid.animation.isPlaying)
        {
            gameEnded = true;
            topLid.animation.Play("ClosingTop");
            bottomLid.animation.Play("ClosingBottom");
        }
    }
}
