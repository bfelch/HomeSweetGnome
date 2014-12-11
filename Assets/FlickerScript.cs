using UnityEngine;
using System.Collections;

public class FlickerScript : MonoBehaviour 
{
	private float oldTime = 0.0f;
	private float newTime = 10.0f; //When the lightning flash happens next
	private int slot = 0; //Flash 3 times witin 10 slots

    private int pattern;
    private int[][] patterns = {new int[]{1,5,8}, new int[]{2,5,7}, new int[]{3,5,8}};

    public Material matLightOn;
    public Material matLightOff;

	// Use this for initialization
	void Start () 
	{
        
	}
	
	// Update is called once per frame
	void Update () 
	{
        checkFlickerTime();
	}

	void checkFlickerTime()
	{
		if(Time.time > oldTime + newTime)
		{
			newTime = Random.Range(5, 15); //Set next flicker time
			oldTime = Time.time;

		    pattern = Random.Range(0,3); //Pick a flicker pattern
            InvokeRepeating("Flicker", 1.0F, 0.1F);
		}
	}

	void Flicker()
	{
        if (System.Array.IndexOf(patterns[pattern], slot) >= 0)
        {
            if (this.gameObject.name == "LightBulb")
            {
                this.light.enabled = false;
                this.renderer.material = matLightOff;
                (GetComponent("Halo") as Behaviour).enabled = false;
            }
        }
        else
        {
            if (this.gameObject.name == "LightBulb")
            {
                this.light.enabled = true;
                this.renderer.material = matLightOn;
                (GetComponent("Halo") as Behaviour).enabled = true;
            }
        }

		slot++;

		//Reset
		if(slot >= 10)
		{
			slot = 0;
			CancelInvoke("Flicker");
		}
	}
}
