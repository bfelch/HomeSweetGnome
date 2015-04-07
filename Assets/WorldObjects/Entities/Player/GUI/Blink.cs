using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{
    public GameObject topLid;
    public GameObject bottomLid;
    public GameObject topLidSlug;
    public GameObject bottomLidSlug;
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
    private bool refind = true;

    //need player health and max health to adjust blink speed
    private float playerSanity;
    private float playerSanityMax;

    private float maxBlinkDisplayTimer = 3f;
    private float blinkDisplayTimer = 0f;
    public Texture2D blinkDisplay;
    private bool holdEyes = false;

	private bool holdEyesOpen = false;

    private Vector3 topLidSlugPos;
    private Vector3 bottomLidSlugPos;
    private float slugPosModifier = .2f;

    private float topLidDiff = .24821f;
    private float bottomLidDiff = .2255691f;

	private float tempPlayerSanity;

    // Use this for initialization
    void Start()
    {
        //get lids and players health
        topLid = GameObject.Find("UpperEyeLid");
        bottomLid = GameObject.Find("LowerEyeLid");
        topLidSlug = GameObject.Find("UpperSlugLid");
        bottomLidSlug = GameObject.Find("LowerSlugLid");
        topLidSlugPos = topLidSlug.transform.localPosition;
        bottomLidSlugPos = bottomLidSlug.transform.localPosition;
        player = GameObject.Find("Player");
        playerSanity = gameObject.GetComponent<Player>().sanity;
		tempPlayerSanity = playerSanity;
        playerSanityMax = gameObject.GetComponent<Player>().maxSanity;

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
        playerSlept = gameObject.GetComponent<EndGames>().playerSlept;


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

        if(Input.GetKeyDown(KeyCode.R))
        {
            ShutEyes();
			//blink = true;
			StartCoroutine("BlinkDelay", 0.3F);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            OpenShutEyes();
        }
    }

    public void BlinkMechanics()
    {
        if (refind)
        {
            topLidSlugPos = topLidSlug.transform.localPosition;
            bottomLidSlugPos = bottomLidSlug.transform.localPosition;
            refind = false;
        }

		if(!holdEyesOpen)
		{
			if(tempPlayerSanity > playerSanity)
			{
				tempPlayerSanity -= 1.0F;

		        Vector3 curTopSlugPos = topLidSlugPos;
		        curTopSlugPos.y -= (1 - (tempPlayerSanity / playerSanityMax)) * slugPosModifier;

		        Vector3 curBottomSlugPos = bottomLidSlugPos;
		        curBottomSlugPos.y += (1 - (tempPlayerSanity / playerSanityMax)) * slugPosModifier;

		        topLidSlug.transform.localPosition = curTopSlugPos;
		        bottomLidSlug.transform.localPosition = curBottomSlugPos;
			}
			else
			{
				Vector3 curTopSlugPos = topLidSlugPos;
				curTopSlugPos.y -= (1 - (playerSanity / playerSanityMax)) * slugPosModifier;
				
				Vector3 curBottomSlugPos = bottomLidSlugPos;
				curBottomSlugPos.y += (1 - (playerSanity / playerSanityMax)) * slugPosModifier;
				
				topLidSlug.transform.localPosition = curTopSlugPos;
				bottomLidSlug.transform.localPosition = curBottomSlugPos;
			}
		}
		
        //decrease the blink timer
        blinkTimer -= Time.deltaTime;

        //once timer hits zero, blink and reset timer
        if (blinkTimer <= 0)
        {
            blinkTimer = 5.08f;

			//adjust the speed depending on player's health
			topLid.animation["BlinkTopNew"].speed = ((playerSanity + 5) / playerSanityMax);
			bottomLid.animation["BlinkBottomNew"].speed = ((playerSanity + 5) / playerSanityMax);

            topLid.animation.Play("BlinkTopNew");
            bottomLid.animation.Play("BlinkBottomNew");

			StartCoroutine("BlinkDelay", 0.01F);
			//blink = true;
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
        
        if(Input.GetKeyDown(KeyCode.F) && !rechargeBlink)
        {
			if(tempPlayerSanity >= playerSanity)
			{
				//do nothing
			}
			else
			{
				tempPlayerSanity = playerSanity;
			}

			holdEyesOpen = true;
		}
		if(Input.GetKeyUp(KeyCode.F))
		{
			holdEyesOpen = false;
		}

		//am I holding my eyes open and my blink is recharged?
		if (Input.GetKey(KeyCode.F) && !rechargeBlink)
		{
            //are we greater than 0
            if (openTimer > 0)
            {
				if(tempPlayerSanity < 100)
				{
					tempPlayerSanity += 3.0F;
					Vector3 curTopSlugPos = topLidSlugPos;
					curTopSlugPos.y -= (1 - (tempPlayerSanity / playerSanityMax)) * slugPosModifier;
					
					Vector3 curBottomSlugPos = bottomLidSlugPos;
					curBottomSlugPos.y += (1 - (tempPlayerSanity / playerSanityMax)) * slugPosModifier;

					topLidSlug.transform.localPosition = curTopSlugPos;
					bottomLidSlug.transform.localPosition = curBottomSlugPos;
				}

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
				holdEyesOpen = false;
                rechargeBlink = true;
                blinkDisplayTimer = maxBlinkDisplayTimer;
            }
		}
    }

    void ShutEyes()
    {
        if(!holdEyes)
        {
            topLid.animation.Play("CloseEyeTop");
            bottomLid.animation.Play("CloseBottomEye");
            holdEyes = true;
        }
        blinkTimer = 5;
    }

    void OpenShutEyes()
    {
        topLid.animation.Play("OpenEyeTop");
        bottomLid.animation.Play("OpenBottomEye");
        holdEyes = false;

		StopCoroutine("BlinkDelay");
        blink = false;
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
            //save original GUI color
            Color original = GUI.color;
            //calculate changing GUI color with changing alpha value
            Color changing;
            if (blinkDisplayTimer / maxBlinkDisplayTimer < .25f) {
                //if below 25%, fade in
                changing = new Color(GUI.color.r, GUI.color.g, GUI.color.b, (blinkDisplayTimer * 4) / maxBlinkDisplayTimer);
            } else if (blinkDisplayTimer / maxBlinkDisplayTimer < .75f) {
                //if between 25% and 75%, keep visible
                changing = original;
            } else {
                //if above 75%, fade out
                changing = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Abs(((blinkDisplayTimer * 4) / maxBlinkDisplayTimer) - 4));
            }
            //set GUI color to changing color
            GUI.color = changing;
            //draw the texture
            GUI.DrawTexture(new Rect(10, 10, 100, 100), blinkDisplay);
            //set GUI color back to original
            GUI.color = original;
        }
    }

	IEnumerator BlinkDelay(float waitTime)
	{
        Debug.Log("Blink Delay Before");
		yield return new WaitForSeconds(waitTime);
        Debug.Log("Blink Delay");
		blink = true;
	}
}
