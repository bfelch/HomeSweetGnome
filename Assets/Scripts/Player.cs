using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float sanity;
    public float maxSanity;
    float healthBarLength;
    public float restTime;
    public float sprintTime;
    private CharacterMotor charMotor;


    // Use this for initialization
    void Start()
    {
        //Sanity instead of health. As they touch you, sanity falls. It also slowly falls over time. Must find items to raise it.
        //The lower it gets, the more hazards are in the level.
        sanity = 100;
        maxSanity = 100;
        restTime = .75f;
        sprintTime = 1.25f;
        charMotor = gameObject.GetComponent<CharacterMotor>();

    }

    // Update is called once per frame
    void Update()
    {
        Sanity();
        Sprint();
    }

    void OnGUI()
    {
        healthBarLength = (Screen.width / 3) * (sanity / (float)maxSanity);
        GUI.color = Color.red;
        GUI.Box(new Rect(550, 10, healthBarLength, 30), "Sanity");
    }
    //Note: Bug: enemies will not pathfind close enough to you to actually register the collision.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Enemy")
        {
            //Destroy(gameObject);
            sanity -= 2;
        }
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
}
