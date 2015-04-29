using UnityEngine;
using System.Collections;

public class TreeWind : MonoBehaviour 
{
	Vector4 Wind = new Vector4(0.85F, 0.075F, 0.4F, 0.5F);
	float WindFrequency = 0.25F;

	void Start()
	{
		Shader.SetGlobalColor("_Wind", Wind);
	}

	void Update() 
	{
		// simple wind animation
        Color WindRGBA = Wind * ((Mathf.Sin(Time.time * WindFrequency)));
		WindRGBA.a = Wind.w;
		Shader.SetGlobalColor("_Wind", WindRGBA);
	}
}
