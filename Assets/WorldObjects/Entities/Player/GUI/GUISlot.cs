using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUISlot : MonoBehaviour
{
    //changes to rotation when activated
    public float startRotation;
    public float rotationBound;
    public float rotationSpeed;

    //prevents backwards gui slots
    private float lowerBound;
    private float upperBound;

    //rotation direction
    private bool clockwise;

    public bool hovering;
    public bool isKeyRing;
    public bool isEnergyBar;

    // Use this for initialization
    void Start()
    {
        //sets bounds
        lowerBound = 360f - rotationBound;
        upperBound = rotationBound;
        hovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        //current rotation
        float currentAngle;
        currentAngle = transform.localRotation.eulerAngles.y;
        //flips energy bar so texture is right direction
        if (isEnergyBar) {
            currentAngle = (currentAngle + 180f) % 360f;
        }

        //rotate clockwise
        if (clockwise)
        {
            //if key ring, rotate on different axis
            if (isKeyRing) {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed));
            } else {
                transform.Rotate(new Vector3(0, rotationSpeed, 0));
            }
        }
        //rotate counterclockwise
        else
        {
            //if key ring, rotate on different axis
            if (isKeyRing) {
                transform.Rotate(new Vector3(0, 0, rotationSpeed));
            } else {
                transform.Rotate(new Vector3(0, -rotationSpeed, 0));
            }
        }

        //if passing bounds, change direction
        if (currentAngle < lowerBound && currentAngle > 180f)
        {
            clockwise = true;
        }
        else if (currentAngle > upperBound && currentAngle < 180f)
        {
            clockwise = false;
        }
    }

    public void ResetRotation()
    {
        //pick new rotation bounds
        rotationBound = Random.Range(10f, 20f);
        //pick new rotation speed
        rotationSpeed = Random.Range(.1f, .2f);
        //pick new rotation between bounds
        startRotation = Random.Range(-rotationBound, rotationBound);
        //set rotation
        transform.localRotation = Quaternion.identity;
        if (isKeyRing) {
            //set default rotation of key ring
            transform.Rotate(new Vector3(90, 0, startRotation));
        } else if (isEnergyBar) {
            //set default rotation of energy bar
            transform.Rotate(new Vector3(0, startRotation + 180f, 0));
        } else {
            //set default rotation for all other slots
            transform.Rotate(new Vector3(0, startRotation, 0));
        }
        ToggleSlotHover(false);
    }

    void OnMouseEnter()
    {
        ToggleSlotHover(true);
    }

    void OnMouseExit()
    {
        ToggleSlotHover(false);
    }

    void ToggleSlotHover(bool hovering) {
        this.hovering = hovering;

        //set material in relation to hover
        if (this.hovering && this.renderer.material.color != Color.red) {
            this.renderer.material.color = Color.red;
        } else if (!this.hovering && this.renderer.material.color != Color.white) {
            this.renderer.material.color = Color.white;
        }
    }
}