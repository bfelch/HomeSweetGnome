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
	public GameObject darkness;
	private bool darkReady = true;
	public bool bookOpen = false;

	// Use this for initialization
	void Start () 
	{

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
		
		//toggle movements, looking, cursor
		charMotor.enabled = false;
		mouseLook.enabled = false;
		cameraLook.enabled = false;
		Screen.lockCursor = false;

		//For one time darkness event
		if(darkReady)
		{
			darkReady = false;
			//enable darkness scripted event
			darkness.SetActive(true);
		}
	}

	public void CloseBook()
	{
		bookOpen = false;
		//activate/deactivate book
		book.SetActive(false);
		
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
