using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGames : MonoBehaviour {
    public GUIText deathTextSleep;
    public GUIText deathTextFall;
    public GUIText winTextEscaped;
    public GUIText winTextExperiment;
    public bool playerSlept;
    public bool playerFell;
    public bool playerEscaped;
    public bool experimentComplete;
    public bool experimentWin = false;
    private float fadeIn = 0;
    private float pauseFadeTime = 4;
    private bool pauseFade = false;
    private bool switchFade = false;
    private float time = 0;
    private bool gotTime = false;
    public static GameObject[] dockGnomes;
    public PlayerInteractions playerInt;

    private int experimentColorFade = 0;
    public Material sunnySky;
	public GameObject wind;

    public static GameObject endingText; 

    //horizontal look
    public MouseLook mouseLook;
    //vertical look
    public MouseLook cameraLook;

	public static Dictionary<string, GameObject> allPickUps;
    
	void Awake()
	{
			getAllItems ();
	}
	// Use this for initialization
	void Start () {

        deathTextSleep = GameObject.Find("DeathTextSleep").guiText;
        deathTextSleep.enabled = false;

        deathTextFall = GameObject.Find("DeathTextFall").guiText;
        deathTextFall.enabled = false;
        endingText = GameObject.Find("EndingText");

        winTextEscaped = GameObject.Find("WinTextEscape").guiText;
        winTextEscaped.enabled = false;

        winTextExperiment = GameObject.Find("WinTextExperiment").guiText;
        winTextExperiment.enabled = false;

        playerSlept = false;
        playerFell = false;
        time = GetComponent<PlayerInteractions>().timePlayed;
        dockGnomes = new GameObject[] { GameObject.Find("DockGnome1"), GameObject.Find("DockGnome2"), GameObject.Find("DockGnome3") };
        for (int i = 0; i < dockGnomes.Length; i++)
        {
            dockGnomes[i].SetActive(false);
        }

        playerInt = GameObject.Find("Player").GetComponent<PlayerInteractions>();

	}
	
	// Update is called once per frame
	void Update () {
        if (experimentComplete) {
            Light light = GameObject.Find("Moonlight").GetComponent<Light>();
            //GlobalFog fog = GameObject.Find("Main Camera").GetComponent<GlobalFog>();
            light.color = Color.Lerp(new Color(.16f, .16f, .2f, 1f), Color.white, experimentColorFade / 500f);
            //fog.globalDensity = Mathf.Lerp(.7f, 0f, experimentColorFade / 300f);
            experimentColorFade++;
        }
	}



    IEnumerator WaitToReloadMenu(float waitTime)
    {
        //Wait before loading the main menu
        yield return new WaitForSeconds(waitTime);

        //Load the main menu
        Application.LoadLevel("MainMenu");
    }
    IEnumerator ExperimentWin(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        experimentComplete = false;
        experimentWin = true;
        mouseLook.enabled = false;
        cameraLook.enabled = false;
        

    }

    public void Escape()
    {
        playerEscaped = true;
        GetTime();
        //StartCoroutine(WaitToReload(8.0F));
    }

    public void Experiment()
    {
        experimentComplete = true;
        GetTime();

        GameObject lightRain = GameObject.Find("Light Rain");
        GameObject heavyRain = GameObject.Find("Heavy Rain");
        GameObject rain = GameObject.Find("Rain");

        lightRain.particleSystem.Stop();
        heavyRain.particleSystem.Stop();

		StartCoroutine(SoundController.FadeAudio(10.0F, SoundController.Fade.Out, rain.audio, 0.07F));

        Skybox skybox = Camera.main.gameObject.AddComponent<Skybox>();
        skybox.material = sunnySky;

        GameObject weather = GameObject.Find("Weather");
        weather.SetActive(false);

        GameObject gnomes = GameObject.Find("Gnomes");
        GameObject gargoyles = GameObject.Find("Gargoyles");

		gnomes.SetActive(false);
        gargoyles.SetActive(false);
        wind.SetActive(false);

        AudioSource eerie = GameObject.Find("GlobalSoundController").GetComponents<AudioSource>()[0];
		AudioSource birdSound = GameObject.Find("GlobalSoundController").GetComponents<AudioSource>()[1];

		StartCoroutine(SoundController.FadeAudio(2.0F, SoundController.Fade.Out, eerie, 0.4F));

		birdSound.Play();
		StartCoroutine(SoundController.FadeAudio(12.0F, SoundController.Fade.In, birdSound, 0.2F));

        StartCoroutine(ExperimentWin(5.0F));

    }


    void OnGUI()
    {
        if (experimentComplete)
        {
            endingText.GetComponent<ExperimentEnding>().enabled = true;
        }
        else
            winTextExperiment.enabled = false;

        if (playerEscaped)
        {
            endingText.GetComponent<GateEnding>().enabled = true;
        }

        if (playerSlept)
        {
            endingText.GetComponent<GnomeEnding>().enabled = true;
        }

        if (playerFell)
        {
            endingText.GetComponent<FallEnding>().enabled = true;
        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fall")
        {
            playerFell = true;
            this.GetComponent<EndGames>().GetTime();
            //StartCoroutine(WaitToReloadMenu(5.0F));
        }
        if (other.name == "GateEndGame")
        {
            Escape();
        }
    }

    public void GetTime()
    {
        //if(!gotTime)
        {
            gotTime = true;
            time = (int)(time + Time.timeSinceLevelLoad);
            playerInt.timePlayed = time;
        }

    }

    public static string getTimeString(float time)
    {
        int second = (int)time;
        int minute = second / 60;
        int hour = minute / 60;
        second = second % 60;

        return hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
    }

	public static void getAllItems()
	{
		allPickUps = new Dictionary<string, GameObject>();
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag ("PickUp");
		for (int i = 0; i < pickUps.Length; i++) 
		{
			allPickUps.Add(pickUps[i].name, pickUps[i]);
		}
	}
}
