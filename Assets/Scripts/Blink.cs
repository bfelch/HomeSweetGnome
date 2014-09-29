using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {
    public GameObject topLid;
    public GameObject bottomLid;

    float blinkTimer = 5.0f;
    float openTimer = 10.0f;
    float maxOpenTimer = 10.0f;
    public bool blink = false;
    private bool rechargeBlink = false;

    private bool showGUI = false;
    private Rect blinkGUI = new Rect(425,5, 200, 22);

    private Texture2D background;
    private Texture2D foreground;

	// Use this for initialization
	void Start () {
        topLid = GameObject.Find("UpperEyeLid");
        bottomLid = GameObject.Find("LowerEyeLid");

        background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);

        background.SetPixel(0, 0, Color.clear);
        foreground.SetPixel(0, 0, Color.red);

        background.Apply();
        foreground.Apply();
	}
	
	// Update is called once per frame
	void Update () {
        BlinkMechanics();
        OpenEyes();
        GUIControl();
	}

    void BlinkMechanics()
    {
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0)
        {
            blinkTimer = 5.08f;
            topLid.animation.Play("BlinkTop");
            bottomLid.animation.Play("BlinkBottom");
            blink = true;
        }

        if (blinkTimer <= 5.0f)
        {
            blink = false;
        }
    }

    void OpenEyes()
    {
        if(openTimer <= 10 && !Input.GetKey(KeyCode.Q))
        {
            openTimer += Time.deltaTime;
            if(openTimer >= 5)
            {
                rechargeBlink = false;
            }
        }
        if(Input.GetKey(KeyCode.Q) && !rechargeBlink)
        {
            if(openTimer > 0)
            {
                openTimer -= Time.deltaTime;
                blinkTimer = 5;
            }
            if(openTimer <= 0)
            {
                topLid.animation.Play("BlinkTop");
                bottomLid.animation.Play("BlinkBottom");
                rechargeBlink = true;
            }
        }

    }

    void OnGUI()
    {
        if (showGUI)
        {
            GUI.BeginGroup(blinkGUI);
            {
                GUI.DrawTexture(new Rect(0, 0, blinkGUI.width, blinkGUI.height), background, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(0, 0, blinkGUI.width * openTimer / maxOpenTimer, blinkGUI.height), foreground, ScaleMode.StretchToFill);
                GUI.backgroundColor = Color.clear;
                GUI.TextArea(new Rect(0, 0, blinkGUI.width, blinkGUI.height), "No Blink");
            }
            GUI.EndGroup(); ;

        }

    }

    void GUIControl()
    {
        if (Input.GetMouseButton(0))
        {
            showGUI = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            showGUI = false;
        }

    }

}
