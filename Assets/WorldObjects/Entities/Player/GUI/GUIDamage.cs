using UnityEngine;
using System.Collections;

public class GUIDamage : MonoBehaviour 
{
    public Texture damageOne;
	public bool enterCollider = false;
    public float damageTimer = 20;
    private float maxDamageTimer = 20;
    private bool deathSleep = false;
    private bool deathFall = false;

    public void OnTriggerEnter(Collider col)
    {
        //check if we are in the enemy trigger
        if(col.gameObject.tag == "Gnome")
        {
            //set enter collider equal to true because we are hitting an enemy
            enterCollider = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        //if we leave the enemy's trigger, set enterCollider to false
		if(col.gameObject.tag == "Gnome")
		{
        	enterCollider = false;
		}
    }
	
	// Update is called once per frame
	void Update () 
	{
        //check if the damage timer is greater than 0
        if (damageTimer >= 0)
        {
            //check if we are in the enemy collider
            if (enterCollider)
            {
                //decrease the damageTimer
                damageTimer -= 5 * Time.deltaTime;
            }
            else
            {
                //add onto the damageTimer when we are outside the enemy's collider
                damageTimer += 5 * Time.deltaTime;
            }
        }
        //if we've gone below 0, set damageTimer to 0 to keep from getting negative alpha value
        if(damageTimer < 0 && enterCollider == true)
        {
            damageTimer = 0;
        }
        //if we've gone above max, keep it at maximum 
        if (damageTimer >= maxDamageTimer)
        {
            damageTimer = maxDamageTimer;
        }

        //get the death variables from player
        deathFall = GameObject.Find("Player").GetComponent<EndGames>().playerFell;
        deathSleep = GameObject.Find("Player").GetComponent<EndGames>().playerSlept;
	}

    void OnGUI()
    {
        //if we are not dead, continue
        if (!deathFall && !deathSleep)
        {
            //save original GUI color
            Color original = GUI.color;
            //calculate changing GUI color with changing alpha value
            Color changing = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - damageTimer / maxDamageTimer);
            //set GUI color to changing color
            GUI.color = changing;
            //draw the texture
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), damageOne);
            //set GUI color back to original
            GUI.color = original;
        }
    }
}
