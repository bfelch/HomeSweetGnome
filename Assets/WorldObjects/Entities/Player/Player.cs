using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float sanity;
    public float maxSanity;
    public float minSanity;
    float healthBarLength;
    private float restTime;
    private float maxRestTime;
    public float sprintTime;
    public float maxSprintTime;
    public bool playerDied;
    public GUIText deathText;
    private Animation blinkBottom;
    private Animation blinkTop;
    public CharacterMotor charMotor;

    private bool crouching;
    private float yScale;

    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        minSanity = 0;
        restTime = maxRestTime = .75f;
        sprintTime = maxSprintTime = 1.25f;
        playerDied = false;

        yScale = this.transform.localScale.y;

        deathText = GameObject.Find("DeathText").guiText;
        deathText.enabled = false;

        if(PlayerPrefs.GetInt("LoadGame") == 1)
        {
            GameObject.Find("Save").GetComponent<SaveLoad>().Load();
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
        Crouch();
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

    void Crouch()
    {
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && !crouching){
            crouching = true;
            charMotor.movement.maxForwardSpeed = 3;
            gameObject.transform.localScale = new Vector3(1, yScale / 2, 1);
            Camera.main.transform.localScale = new Vector3(1, 2 * yScale, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            crouching = false;
            gameObject.transform.localScale = new Vector3(1, yScale, 1);
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(pos.x, pos.y + .6f, pos.z);
            Camera.main.transform.localScale = new Vector3(1, yScale, 1);
        }
    }


    void Sanity()
    {
        sanity -= .002f;
        if (sanity < 0)
        {
            //Application.LoadLevel("MainMenu");
            playerDied = true;
        }

        if (sanity > maxSanity)
        {
            sanity = maxSanity;
        }
        else if(sanity < minSanity)
        {
            sanity = 0;
        }
    }

    void OnGUI()
    {
        if (playerDied)
        {
            deathText.enabled = true;
        }
    }
}