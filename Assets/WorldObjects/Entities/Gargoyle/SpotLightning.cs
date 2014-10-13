using UnityEngine;
using System.Collections;

/*
	Script for handling nearby lightning flashes.  It will pick a directional light, 
	set random time intervals, and create a realistic environment lightning effect.
*/
public class SpotLightning: MonoBehaviour
{
    public GameObject spot1; //Light1
    public GameObject spot2; //Light2

    float oldTime = 0.0f;
    float newTime = .01f; //When the lightning flash happens next
    int slot = 0; //Flash 3 times witin 10 slots
    int direction = 0; //Lightning direction

    // Use this for initialization
    void Start()
    {
        //dir1 = GameObject.Find("Dir1");
        //dir2 = GameObject.Find("Dir2");
    }

    // Update is called once per frame
    void Update()
    {
        checkFlashTime();
    }

    void checkFlashTime()
    {
        if (Time.time > oldTime + newTime)
        {
            //newTime = Random.Range(15, 20); //Set next lightning flash
            newTime = 2; //for testing
            oldTime = Time.time;

            direction = Random.Range(0, 2); //Pick a light direction
            InvokeRepeating("flash", 1.0f, 0.20f); //Start flash sequence
        }
    }

    void flash()
    {
        //Preset flash pattern
        if (slot == 5 || slot == 8 || slot == 10)
        {
            //Turn on light
            switch (direction)
            {
                case 0:
                    light.intensity = 1.5f;
                    break;
                case 1:
                    spot1.light.intensity = 1.5f;
                    break;
                case 2:
                    spot2.light.intensity = 1.5f;
                    break;
                default:
                    //do nothing
                    break;
            }
        }
        else
        {
            //Turn off light
            switch (direction)
            {
                case 0:
                    light.intensity = 0;
                    break;
                case 1:
                    spot1.light.intensity = 0;
                    break;
                case 2:
                    spot2.light.intensity = 0;
                    break;
                default:
                    //do nothing
                    break;
            }
        }

        slot++;

        //Reset
        if (slot >= 10)
        {
            slot = 0;
            CancelInvoke("flash");
        }
    }
}
