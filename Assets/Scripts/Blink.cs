using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {
    public GameObject topLid;
    public GameObject bottomLid;

    float blinkTimer = 5.0f;
    float openTimer = 10.0f;
    private bool blink = false;
    private bool rechargeBlink = false;
	// Use this for initialization
	void Start () {
        topLid = GameObject.Find("UpperEyeLid");
        bottomLid = GameObject.Find("LowerEyeLid");


	}
	
	// Update is called once per frame
	void Update () {
        BlinkMechanics();
        OpenEyes();
	}

    void BlinkMechanics()
    {
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0)
        {
            blinkTimer = 5;
            blink = true;
        }

        if (blink)
        {
            topLid.animation.Play("BlinkTop");
            bottomLid.animation.Play("BlinkBottom");
            blink = false;
        }
    }

    void OpenEyes()
    {
        if(openTimer <= 10 && !Input.GetKey(KeyCode.Q))
        {
            openTimer += Time.deltaTime;
            if(openTimer >= 5)
            {
                rechargeBlink = false;
            }
        }
        if(Input.GetKey(KeyCode.Q) && !rechargeBlink)
        {
            if(openTimer > 0)
            {
                openTimer -= Time.deltaTime;
                blinkTimer = 5;
            }
            if(openTimer <= 0)
            {
                topLid.animation.Play("BlinkTop");
                bottomLid.animation.Play("BlinkBottom");
                rechargeBlink = true;
            }
        }

    }
}
