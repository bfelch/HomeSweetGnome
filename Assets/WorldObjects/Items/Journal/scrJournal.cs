using UnityEngine;
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
	public static bool journalNoGUI = false;
    private bool closeOnce = false;	
	
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
		if(Input.GetKeyUp(KeyCode.E) && journalOpen && closeOnce)
		{
			CloseJournalPage();
            closeOnce = false;
		}
	}
	
	public void OpenJournalPage()
	{
		journalNoGUI = true;

		StartCoroutine(JournalTimer(0.2F));
		//activate/deactivate book
		journalPage.SetActive(true);
        this.GetComponent<AudioSource>().Play();
        Debug.Log("Playing Open");

		GameObject.Find("Highlighter").GetComponent<scrHighlightController>().Unhighlight(this.gameObject);
		
		//toggle movements, looking, cursor
		mouseLook.enabled = false;
		cameraLook.enabled = false;
		Screen.lockCursor = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
        //GameObject.Find("Player").GetComponent<Player>().enabled = false;
		charMotor.canControl = false;
		charMotor.jumping.enabled = false;

        closeOnce = true;
	}
	
	public void CloseJournalPage()
	{
		journalNoGUI = false;

		StartCoroutine (JournalTimer2 (0.2F));
        this.GetComponent<AudioSource>().Play();

		//activate/deactivate book
		journalPage.SetActive(false);
        Debug.Log("Playing Close");

        if (!PlayerInteractions.showGUI)
        {
			Debug.Log("Close Enable");
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
