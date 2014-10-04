using UnityEngine;
using System.Collections;

public class GUIControl : MonoBehaviour {
    private bool showGUI = false;
    private float fatigueValue;
    private float maxFatigueValue;
    private float sprintValue;
    private float maxSprintValue;
    private float openTimer;
    private float maxOpenTimer;

    private bool characterController;

    private Texture2D background;
    private Texture2D foreground;

    private Rect blinkGUI = new Rect(Screen.width-50,225, 22, 150);
    private Rect fatigue = new Rect(25, 50, 22, 325);
    private Rect sprint = new Rect(Screen.width-50, 50, 22, 150);

   

	// Use this for initialization
	void Start () {

        background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);

        background.SetPixel(0, 0, Color.clear);
        foreground.SetPixel(0, 0, Color.red);

        background.Apply();
        foreground.Apply();

        characterController = gameObject.GetComponent<CharacterMotor>().enabled;
	}
	
	// Update is called once per frame
	void Update () {

        fatigueValue = gameObject.GetComponent<Player>().sanity;
        maxFatigueValue = gameObject.GetComponent<Player>().maxSanity;
        sprintValue = gameObject.GetComponent<Player>().sprintTime;
        maxSprintValue = gameObject.GetComponent<Player>().maxSprintTime;
        openTimer = gameObject.GetComponent<Blink>().openTimer;
        maxOpenTimer = gameObject.GetComponent<Blink>().maxOpenTimer;

        GUIControls();
        RestrictMovment();
	}

    void OnGUI()
    {
        if (showGUI)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            GUI.BeginGroup(fatigue);
            {
                GUI.DrawTexture(new Rect(0, 0, fatigue.width, fatigue.height), background, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(0, 0, fatigue.width, fatigue.height * fatigueValue / maxFatigueValue), foreground, ScaleMode.StretchToFill);
                GUI.backgroundColor = Color.clear;
                GUI.TextArea(new Rect(0, 0, fatigue.width, fatigue.height), VerticalText("Fatigue"));
            }
            GUI.EndGroup(); ;

            GUI.BeginGroup(sprint);
            {
                GUI.DrawTexture(new Rect(0, 0, sprint.width, sprint.height), background, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(0, 0, sprint.width, sprint.height * sprintValue / maxSprintValue), foreground, ScaleMode.StretchToFill);
                GUI.backgroundColor = Color.clear;
                GUI.TextArea(new Rect(0, 0, sprint.width, sprint.height), VerticalText("Sprint"));
            }
            GUI.EndGroup(); ;

            GUI.BeginGroup(blinkGUI);
            {
                GUI.DrawTexture(new Rect(0, 0, blinkGUI.width, blinkGUI.height), background, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(0, 0, blinkGUI.width, blinkGUI.height * openTimer / maxOpenTimer), foreground, ScaleMode.StretchToFill);
                GUI.backgroundColor = Color.clear;
                GUI.TextArea(new Rect(0, 0, blinkGUI.width, blinkGUI.height), VerticalText("No Blink"));
            }
            GUI.EndGroup(); ;

        }


        GUI.color = Color.white;
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        // Draw the label at the center of the screen 
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "+", centeredStyle);
    }

    string VerticalText (string input)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder(input.Length*2);
        for (var i = 0; i < input.Length; i++) {
            sb.Append(input[i]).Append("\n");
        }
        return sb.ToString();
    }

    void GUIControls()
    {
        if (Input.GetMouseButton(0)){showGUI = true; }
        if (Input.GetMouseButtonUp(0)) { showGUI = false; }
    }

    void RestrictMovment()
    {
        if (showGUI) {gameObject.GetComponent<CharacterMotor>().enabled = false;}
        else{ gameObject.GetComponent<CharacterMotor>().enabled = true;}
    }
}
