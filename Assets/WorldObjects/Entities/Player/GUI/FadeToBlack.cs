using UnityEngine;
using System.Collections;

public class FadeToBlack : MonoBehaviour {

    public Texture black;
    public float timer = 20;
    private float maxTimer = 20;

	// Update is called once per frame
	void Update () 
	{
        //check if the damage timer is greater than 0
        if (timer >= 0)
        {
                //decrease the damageTimer
                timer -= 5 * Time.deltaTime;

        }
      
        //if we've gone above max, keep it at maximum 
        if (timer >= maxTimer)
        {
            timer = maxTimer;
        }

	}

    void OnGUI()
    {
        //save original GUI color
        Color original = GUI.color;
        //calculate changing GUI color with changing alpha value
        Color changing = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - timer / maxTimer);
        //set GUI color to changing color
        GUI.color = changing;
        //draw the texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
        //set GUI color back to original
        GUI.color = original;
    }
}
