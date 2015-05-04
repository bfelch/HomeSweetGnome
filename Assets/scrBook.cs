using UnityEngine;
using System.Collections;

public class scrBook : MonoBehaviour 
{
	public bool showBook = false;
	public bool notUseable = false;
	//player movement
	public CharacterMotor charMotor;
	//horizontal look
	public MouseLook mouseLook;
	//vertical look
	public MouseLook cameraLook;

	public GameObject book;
	private bool darkReady = true;
	public static bool bookOpen = false;
	public static bool bookNoGUI = false;

	public GameObject gargoyles;
	public AudioClip gargoyleScreech;

	void Start()
	{
		StartCoroutine(DelayedStart(2.0F));
	}

	public IEnumerator DelayedStart(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);
		
		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Highlight(this.gameObject, scrHighlightController.outline2);
	}

	void Update()
	{
        if ((Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0)) && bookOpen && !PlayerInteractions.delayPause)
		{
			CloseBook();
		}
	}

	public void OpenBook()
	{
		bookNoGUI = true;

		StartCoroutine(BookTimer(0.2F));

		//activate/deactivate book
		book.SetActive(true);
        this.GetComponent<AudioSource>().Play();

		//toggle movements, looking, cursor
		mouseLook.enabled = false;
		cameraLook.enabled = false;
		Screen.lockCursor = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
        //GameObject.Find("Player").GetComponent<Player>().enabled = false;
		charMotor.canControl = false;
		charMotor.jumping.enabled = false;

		//For one time darkness event
		if(darkReady)
		{
			darkReady = false;

			//Chandelier Light Switch Indicator
			GameObject.Find("ChandelierSwitch").GetComponent<Useable>().chandReady = true;
            GameObject.Find("ChandelierSwitch").tag = "Useable";

			scrHighlightController highlighter = GameObject.Find("Highlighter").GetComponent<scrHighlightController>();
			//Unhighlight Book
			highlighter.Unhighlight(this.gameObject);

			GameObject.Find("Player").GetComponent<Blink>().reading = true;

			//Highlight Chandellier Switch
			highlighter.Highlight(GameObject.Find("ChandSwitch"), scrHighlightController.outline2);
			highlighter.Highlight(GameObject.Find("ChandSwitchBase"), scrHighlightController.outline2);

			//Enable gargoyles
			gargoyles.SetActive(true);

			//Play gargoyle one shot
			SoundController.PlayClipAt(gargoyleScreech, new Vector3(0.75F, 49.7F, -60.78F)); 
		}
	}

	public void CloseBook()
	{
		bookNoGUI = false;

		bookOpen = false;
		//activate/deactivate book
		book.SetActive(false);
        this.GetComponent<AudioSource>().Play();

		GameObject.Find("Player").GetComponent<Blink>().reading = false;

        if (!PlayerInteractions.showGUI)
        {
		    //toggle movements, looking, cursor
		    mouseLook.enabled = true;
		    cameraLook.enabled = true;
		    Screen.lockCursor = true;
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
            //GameObject.Find("Player").GetComponent<Player>().enabled = true;
			charMotor.canControl = true;
			charMotor.jumping.enabled = true;
        }
	}

	public IEnumerator BookTimer(float waitTime)
	{
		//Wait time
		yield return new WaitForSeconds(waitTime);
		
		bookOpen = true;
	}
}
