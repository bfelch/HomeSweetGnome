using UnityEngine;
using System.Collections;

public class DarkTrap : MonoBehaviour 
{
	private int gnomesTrapped = 0;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Gnome" && !other.GetComponent<Gnome>().trapped)
		{
			gnomesTrapped++;
		}

		if(gnomesTrapped >= 2)
		{
			GameObject.Find("Darkness").GetComponent<scrDarkness>().EndDarkEvent();
		}
	}
}
