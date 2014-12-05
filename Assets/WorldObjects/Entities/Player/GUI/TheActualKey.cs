using UnityEngine;
using System.Collections;

public class TheActualKey : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        localVelocity.x = 0;
        localVelocity.z = 0;

        rigidbody.velocity = transform.TransformDirection(localVelocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.name + ", " + collision.gameObject.name);
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        collision.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("still colliding");
        rigidbody.velocity *= 0;
        collision.rigidbody.velocity *= 0;
    }
}
