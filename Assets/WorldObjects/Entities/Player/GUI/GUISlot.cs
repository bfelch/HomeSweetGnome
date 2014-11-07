using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUISlot : MonoBehaviour
{
    public float startRotation;
    public float rotationBound;
    public float rotationSpeed;

    private float lowerBound;
    private float upperBound;

    private bool clockwise;

    public bool hovering;
    public bool isKeyRing;

    // Use this for initialization
    void Start()
    {
        lowerBound = 360f - rotationBound;
        upperBound = rotationBound;
        hovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        float currentAngle;

        currentAngle = transform.localRotation.eulerAngles.y;

        if (clockwise)
        {
            if (isKeyRing) {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed));
            } else {
                transform.Rotate(new Vector3(0, rotationSpeed, 0));
            }
        }
        else
        {
            if (isKeyRing) {
                transform.Rotate(new Vector3(0, 0, rotationSpeed));
            } else {
                transform.Rotate(new Vector3(0, -rotationSpeed, 0));
            }
        }

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
        rotationBound = Random.Range(10f, 20f);
        rotationSpeed = Random.Range(.1f, .2f);
        startRotation = Random.Range(-rotationBound, rotationBound);
        transform.localRotation = Quaternion.identity;
        if (isKeyRing) {
            transform.Rotate(new Vector3(90, 0, startRotation));
        } else {
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

    void ToggleSlotHover(bool hovering)
    {
        this.hovering = hovering;

        if (this.hovering)
        {
            this.renderer.material.color = Color.red;
        }
        else
        {
            this.renderer.material.color = Color.white;
        }
    }
}