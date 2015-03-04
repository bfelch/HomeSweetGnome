using UnityEngine;
using System.Collections;

public class DarkTrap : MonoBehaviour 
{
	private int gnomesTrapped = 0;

	void OnTriggerEnter(Collider other)
	{
		if(other.name == "DarkGnome(Clone)" || other.name == "GnomeLvl2(Clone)")
		{
			gnomesTrapped++;
		}

		if(gnomesTrapped >= 2)
		{
			GameObject.Find("Darkness").GetComponent<scrDarkness>().EndDarkEvent();
		}
	}
}
