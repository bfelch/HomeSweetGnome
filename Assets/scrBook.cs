using UnityEngine;
using System.Collections;

public class scrBook : MonoBehaviour 
{
	public bool showBook;
	public bool notUseable = false;
	//player movement
	public CharacterMotor charMotor;
	//horizontal look
	public MouseLook mouseLook;
	//vertical look
	public MouseLook cameraLook;

	public GameObject book;
	public GameObject darkness;

	// Use this for initialization
	void Start () 
	{
		showBook = true;
		ToggleBook();
	}

	void Update()
	{
		if(showBook)
		{
			if(Input.GetKeyUp(KeyCode.F))
			{
				//enable darkness scripted event
				darkness.SetActive(true);

				ToggleBook();
			}
		}
	}

	public void ToggleBook()
	{
		showBook = !showBook;

		//activate/deactivate book
		book.SetActive(showBook);
		
		//toggle movements, looking, cursor
		charMotor.enabled = !showBook;
		mouseLook.enabled = !showBook;
		cameraLook.enabled = !showBook;
		Screen.lockCursor = !showBook;
	}
}
