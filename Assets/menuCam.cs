using UnityEngine;
using System.Collections;

public class menuCam : MonoBehaviour 
{
	public GameObject cam;
	public GameObject keyText;
	public GameObject keyTex;

	private int currentMenu = 0; //Menu index
	private bool camMoving = false;

	private int buttonIndex = 0; //Button inder

	void Update()
	{
		if(!animation.IsPlaying("titleToMain"))
		{
			camMoving = false;

			//Display gui elements
			keyText.guiText.enabled = true;
			keyTex.guiTexture.enabled = true;

		}

		if(Input.anyKeyDown && currentMenu == 0 && camMoving == false)
		{
			//Hide gui elements
			keyText.guiText.enabled = false;
			keyTex.guiTexture.enabled = false;

			animation["titleToMain"].speed = 1.0f;
			animation["titleToMain"].time = 0;
			cam.animation.Play("titleToMain");
			camMoving = true;

			currentMenu = 1;

			keyText.guiText.text = "[Enter] SELECT";
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && currentMenu == 1 && camMoving == false)
		{
			//Hide gui elements
			keyText.guiText.enabled = false;
			keyTex.guiTexture.enabled = false;

			animation["titleToMain"].speed = -1.0f;
			animation["titleToMain"].time = animation["titleToMain"].length;
			cam.animation.Play("titleToMain");
			camMoving = true;

			currentMenu = 0;

			GameObject.Find("Key Text").guiText.text = "Press Any Key";
		}
	}
}
