using UnityEngine;
using System.Collections;

public class scrBed : MonoBehaviour 
{
	//player movement
	public CharacterMotor charMotor;
	//horizontal look
	public MouseLook mouseLook;
	//vertical look
	public MouseLook cameraLook;

	public GUITexture blackTex;
	public GameObject player;
	private float blackFade = 0;
	private float blackDelta = 0.2F;
	public static bool resting = false;
	private bool fade = false;

	public void UseBed()
	{
		if(player.GetComponent<Player>().sanity <= 30.0F)
		{
			blackFade = 0;
			blackDelta = .2f;
			
			resting = true;
			fade = true;

			//toggle movements, looking, cursor
			mouseLook.enabled = false;
			cameraLook.enabled = false;
			GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
			charMotor.canControl = false;
			charMotor.jumping.enabled = false;
		}
	}

	void OnGUI()
	{
		if(resting)
		{
			Color changing;
			blackTex.enabled = true;
			
			if(fade)
			{
				changing = new Color(blackTex.color.r, blackTex.color.g, blackTex.color.b, blackFade);
				//set the new color
				blackTex.color = changing;
				//update the alpha value
				blackFade += blackDelta * Time.deltaTime;

				if(blackFade >= 1)
				{
					fade = false;

					//Sanity full
					player.GetComponent<Player>().sanity = 100;

					//Play sound
				}
			}
			else
			{
				changing = new Color(blackTex.color.r, blackTex.color.g, blackTex.color.b, blackFade);
				//set the new color
				blackTex.color = changing;
				//update the alpha value
				blackFade -= blackDelta * Time.deltaTime;
				
				if(blackFade <= 0)
				{
					resting = false;

					//toggle movements, looking, cursor
					mouseLook.enabled = true;
					cameraLook.enabled = true;
					GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
					charMotor.canControl = true;
					charMotor.jumping.enabled = true;
				}
			}
		}
	}
}
