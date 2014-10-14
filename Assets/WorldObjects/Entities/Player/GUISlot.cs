using UnityEngine;
using System.Collections;

public class GUISlot : MonoBehaviour {

    public float rotationBound;
    public float rotationSpeed;

    private float lowerBound;
    private float upperBound;

    private bool clockwise;

	// Use this for initialization
    void Start() {
        lowerBound = 360f - rotationBound;
        upperBound = rotationBound;
    }
	
	// Update is called once per frame
	void Update () {

        float currentAngle = transform.rotation.eulerAngles.y;

        if (clockwise) {
            transform.Rotate(new Vector3(0, rotationSpeed, 0));
        } else {
            transform.Rotate(new Vector3(0, -rotationSpeed, 0));
        }

        if (currentAngle < lowerBound && currentAngle > 180f) {
            clockwise = true;
        } else if (currentAngle > upperBound && currentAngle < 180f) {
            clockwise = false;
        }
	}
}
