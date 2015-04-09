﻿using UnityEngine;
using System.Collections;

public class scrJournal : MonoBehaviour 
{
	public bool showJournal = false;
	public bool notUseable = false;
	//player movement
	public CharacterMotor charMotor;
	//horizontal look
	public MouseLook mouseLook;
	//vertical look
	public MouseLook cameraLook;
	
	public GameObject journalPage;
	
	public static bool journalOpen = false;
		
	
	void Start()
	{
		StartCoroutine(DelayedStart(2.0F));
	}
	
	public IEnumerator DelayedStart(float waitTime)
	{
		//Wait spawn time
		yield return new WaitForSeconds(waitTime);
		
		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Highlight(this.gameObject, scrHighlightController.outlineLightGray);
	}
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.E) && journalOpen)
		{
			CloseJournalPage();
		}
	}
	
	public void OpenJournalPage()
	{
		StartCoroutine(JournalTimer(0.2F));
		//activate/deactivate book
		journalPage.SetActive(true);

		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Unhighlight(this.gameObject);
		
		//toggle movements, looking, cursor
		charMotor.enabled = false;
		mouseLook.enabled = false;
		cameraLook.enabled = false;
		Screen.lockCursor = false;
	}
	
	public void CloseJournalPage()
	{
		StartCoroutine (JournalTimer2 (0.2F));
		//activate/deactivate book
		journalPage.SetActive(false);

        if (!PlayerInteractions.showGUI)
        {
            //toggle movements, looking, cursor
            charMotor.enabled = true;
            mouseLook.enabled = true;
            cameraLook.enabled = true;
            Screen.lockCursor = true;
        }
	}
	
	public IEnumerator JournalTimer(float waitTime)
	{
		//Wait time
		yield return new WaitForSeconds(waitTime);
		
		journalOpen = true;
	}

	public IEnumerator JournalTimer2(float waitTime)
	{
		//Wait time
		yield return new WaitForSeconds(waitTime);
		
		journalOpen = false;
	}
}
