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
		if(Input.GetKeyUp(KeyCode.E) && bookOpen)
		{
			CloseBook();

		}
	}

	public void OpenBook()
	{
		StartCoroutine(BookTimer(0.2F));

		//activate/deactivate book
		book.SetActive(true);
        this.GetComponent<AudioSource>().Play();

		//toggle movements, looking, cursor
		charMotor.enabled = false;
		mouseLook.enabled = false;
		cameraLook.enabled = false;
		Screen.lockCursor = false;

		//For one time darkness event
		if(darkReady)
		{
			darkReady = false;

			//Chandelier Light Switch Indicator
			GameObject.Find("ChandelierSwitch").GetComponent<Useable>().chandReady = true;

			scrHighlightController highlighter = GameObject.Find("Highlighter").GetComponent<scrHighlightController>();
			//Unhighlight Book
			highlighter.Unhighlight(this.gameObject);

			//Highlight Chandellier Switch
			highlighter.Highlight(GameObject.Find("ChandSwitch"), scrHighlightController.outline2);
			highlighter.Highlight(GameObject.Find("ChandSwitchBase"), scrHighlightController.outline2);
		}
	}

	public void CloseBook()
	{
		bookOpen = false;
		//activate/deactivate book
		book.SetActive(false);
        this.GetComponent<AudioSource>().Play();

		//toggle movements, looking, cursor
		charMotor.enabled = true;
		mouseLook.enabled = true;
		cameraLook.enabled = true;
		Screen.lockCursor = true;
	}

	public IEnumerator BookTimer(float waitTime)
	{
		//Wait time
		yield return new WaitForSeconds(waitTime);
		
		bookOpen = true;
	}
}
