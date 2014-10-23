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
    public float sprintTime;
    public float maxSprintTime;
    private Animation blinkBottom;
    private Animation blinkTop;

    public GameObject gui;
    public GUISlot[] guiSlots;
    public CharacterMotor charMotor;
    public MouseLook mouseLook;
    public MouseLook cameraLook;

    public bool showGUI;


    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        restTime = maxRestTime = .75f;
        sprintTime = maxSprintTime = 1.25f;

        ToggleGUI(showGUI);

        if(PlayerPrefs.GetInt("LoadGame")==1)
        {
            GameObject.Find("Save").gameObject.GetComponent<SaveLoad>().Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Quit the game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        Sanity();
        Sprint();
        GUIControl();
    }

    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            sanity -= 0.2f;
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0)
        {
            charMotor.movement.maxForwardSpeed = 12;
            sprintTime -= Time.deltaTime;
        }
        else
        {
            charMotor.movement.maxForwardSpeed = 6;
            restTime += Time.deltaTime;
            if (restTime >= .75 && sprintTime <= 1.25)
            {
                sprintTime += Time.deltaTime * 4;
                restTime = 0;
            }
        }
    }


    void Sanity()
    {
        sanity -= .002f;
        if (sanity < 0)
        {
            Application.LoadLevel("MainMenu");
        }
        if (sanity > maxSanity)
        {
            sanity = maxSanity;
        }
    }

    void GUIControl()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            ToggleGUI(!showGUI);
        }
    }

    void ToggleGUI(bool activeGUI)
    {
        showGUI = activeGUI;
        gui.SetActive(showGUI);

        if (showGUI && guiSlots != null)
        {
            foreach (GUISlot slot in guiSlots)
            {
                slot.ResetRotation();
            }
        }

        charMotor.enabled = !showGUI;
        mouseLook.enabled = !showGUI;
        cameraLook.enabled = !showGUI;
        Screen.lockCursor = !showGUI;
    }
}