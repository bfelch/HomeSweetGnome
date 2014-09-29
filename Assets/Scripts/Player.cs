using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float sanity;
    public float maxSanity;
    float healthBarLength;
    private float restTime;
    private float maxRestTime;
    private float sprintTime;
    private float maxSprintTime;
    private CharacterMotor charMotor;

    private Texture2D background;
    private Texture2D foreground;
    private Rect fatigue = new Rect(5, 5, 200, 22);
    private Rect sprint = new Rect(215, 5, 200, 22);

    private bool showGUI = false;

    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        restTime = maxRestTime = .75f;
        sprintTime = maxSprintTime = 1.25f;
        charMotor = gameObject.GetComponent<CharacterMotor>();

        background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);

        background.SetPixel(0, 0, Color.clear);
        foreground.SetPixel(0, 0, Color.red);

        background.Apply();
        foreground.Apply();

        Screen.lockCursor = true;

    }

    // Update is called once per frame
    void Update()
    {
        Sanity();
        Sprint();
        GUIControl();
    }

    void OnGUI()
    {
        if (showGUI)
        {
            GUI.BeginGroup(fatigue);
            {
                GUI.DrawTexture(new Rect(0, 0, fatigue.width, fatigue.height), background, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(0, 0, fatigue.width * sanity / maxSanity, fatigue.height), foreground, ScaleMode.StretchToFill);
                GUI.backgroundColor = Color.clear;
                GUI.TextArea(new Rect(0, 0, fatigue.width, fatigue.height), "Fatigue");
            }
            GUI.EndGroup(); ;

            GUI.BeginGroup(sprint);
            {
                GUI.DrawTexture(new Rect(0, 0, sprint.width, sprint.height), background, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(0, 0, sprint.width * sprintTime / maxSprintTime, sprint.height), foreground, ScaleMode.StretchToFill);
                GUI.backgroundColor = Color.clear;
                GUI.TextArea(new Rect(0, 0, sprint.width, sprint.height), "Sprint");
            }
            GUI.EndGroup(); ;

        }


        GUI.color = Color.white;
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        // Draw the label at the center of the screen 
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "+", centeredStyle);
    }
    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Enemy"))
        {
            sanity -= 2;
            showGUI = true;
        }
        else{showGUI = false;}

        openDoor(hit);
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0)
        {
            charMotor.movement.maxForwardSpeed = 50;
            sprintTime -= Time.deltaTime;
            Debug.Log(sprintTime);
        }
        else
        {
            charMotor.movement.maxForwardSpeed = 6;
            restTime += Time.deltaTime;
            if (restTime >= .75 && sprintTime <= 1.25)
            {
                sprintTime += Time.deltaTime;
                restTime = 0;
                Debug.Log(sprintTime);
            }
        }
    }


    void Sanity()
    {
        sanity -= .002f;
        if (sanity < 0)
        {
            Destroy(gameObject);
        }
        if (sanity > maxSanity)
        {
            sanity = maxSanity;
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

 
    void openDoor (ControllerColliderHit hit)
    {
        float pushPower = 2.0f;

        Rigidbody body = hit.collider.attachedRigidbody;
        // no rigidbody
        if (body == null || body.isKinematic) { return; }
        // We dont want to push objects below us
        //if (hit.moveDirection.y < -0.3) { return; }
        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        var pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.
        // Apply the push
        body.velocity = pushDir * pushPower;
        Debug.Log(body.velocity);
    }
}
 

