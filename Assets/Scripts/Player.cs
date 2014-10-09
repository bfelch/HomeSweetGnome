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
    private CharacterMotor charMotor;
    private Animation blinkBottom;
    private Animation blinkTop;

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


        Screen.lockCursor = true;

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
    }

    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Enemy"))
        {
            sanity -= 2;
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0)
        {
            charMotor.movement.maxForwardSpeed = 50;
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
            Application.LoadLevel("HSGMenu");
        }
        if (sanity > maxSanity)
        {
            sanity = maxSanity;
        }
    }

   

 
    

   
}
 

