using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {
	public float waterLevel, floatHeight;
	public Vector3 buoyancyCentreOffset;
	public float bounceDamp;
    public GameObject boat;
	
    void Update()
    {
        //Debug.Log(boat.transform.position.y);

        if (boat.transform.position.y > -23)
        {
            boat.transform.position = new Vector3(boat.transform.position.x, -23f, boat.transform.position.z);
        }
        if (boat.transform.position.y < -25)
        {
            boat.transform.position = new Vector3(boat.transform.position.x, -25f, boat.transform.position.z);
        }

       
    }
	

	void FixedUpdate () {
		Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
		float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
		
		if (forceFactor > 0f) {
			Vector3 uplift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp);
			rigidbody.AddForceAtPosition(uplift, actionPoint);
		}
	}
}
