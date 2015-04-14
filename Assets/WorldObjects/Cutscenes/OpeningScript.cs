using UnityEngine;
using System.Collections;

public class OpeningScript : MonoBehaviour {

    public MovieTexture openingCutscene;
    public bool onlyOnce = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        
        openingCutscene.Play();
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), openingCutscene, ScaleMode.StretchToFill, false, 0.0f);
        
        if (onlyOnce)
        {
            StartCoroutine(StopVideo());
            onlyOnce = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(StopVideo());
            SkipStopVideo();
        }
        
    }

    IEnumerator StopVideo()
    {
        yield return new WaitForSeconds(54f);
        SkipStopVideo();
    }

    void SkipStopVideo()
    {
        onlyOnce = true;
        openingCutscene.Stop();
        Application.LoadLevel("HomeSweetGnome");
    }
    
}
