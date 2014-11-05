using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{
    public GameObject topLid;
    public GameObject bottomLid;
    public GameObject player;

    //how long between heach blink
    public float blinkTimer = 5.0f;
    //how long have I been holder my eyes open
    public float openTimer = 10.0f;
    //how long can I hold my eyes open
    public float maxOpenTimer = 10.0f;
    //am I blinking
    public bool blink = false;
    //am I recharging my starte
    private bool rechargeBlink = false;
    private bool playerSlept = false;
    private bool gameEnded = false;

    //am I showing the GUI
    private bool showGUI = false;

    //need player health and max health to adjust blink speed
    private float playerSanity;
    private float playerSanityMax;

    private float maxBlinkDisplayTimer = 3f;
    private float blinkDisplayTimer;


    // Use this for initialization
    void Start()
    {
        //get lids and players health
        topLid = GameObject.Find("UpperEyeLid");
        bottomLid = GameObject.Find("LowerEyeLid");
        player = GameObject.Find("Player");
        playerSanity = gameObject.GetComponent<Player>().sanity;
        playerSanityMax = gameObject.GetComponent<Player>().maxSanity;

        blinkDisplayTimer = maxBlinkDisplayTimer;

        //if the game was loaded, do not playing opening scene
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
        //get the player sanity values
        playerSanity = gameObject.GetComponent<Player>().sanity;
        playerSlept = gameObject.GetComponent<Player>().playerSlept;

        if (!playerSlept)
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
        //check if I'm not holding my eyes open and the openTimer is less than 10
        if (openTimer <= 10 && !Input.GetKey(KeyCode.F))
        {
            //add onto the openTimer
            openTimer += Time.deltaTime;
            //if the open timer is greater than 5, you can hold your eyes open again
            if (openTimer >= 5)
            {
                rechargeBlink = false;
            }
        }
        //am I holding my eyes open and my blink is recharged?
        if (Input.GetKey(KeyCode.F) && !rechargeBlink)
        {
            //are we greater than 0
            if (openTimer > 0)
            {
                //decrease from the openTimer
                openTimer -= Time.deltaTime;
                //set blinkTimer to 5 to keep from blinking
                blinkTimer = 5;
            }
            //have we hit 0?
            if (openTimer <= 0)
            {
                //blink
                topLid.animation.Play("BlinkTopNew");
                bottomLid.animation.Play("BlinkBottomNew");
                rechargeBlink = true;
                blinkDisplayTimer = maxBlinkDisplayTimer;
            }
        }

    }

    void FallAsleep()
    {
        if (!topLid.animation.isPlaying)
        {
            gameEnded = true;
            topLid.animation.Play("ClosingTop");
            bottomLid.animation.Play("ClosingBottom");
        }
    }

    void OnGUI() {
        if (!rechargeBlink && blinkDisplayTimer > 0) {
            blinkDisplayTimer -= Time.deltaTime;
            GUI.Box(new Rect(10, 10, 50, 50), "blink");
        }
    }
}
