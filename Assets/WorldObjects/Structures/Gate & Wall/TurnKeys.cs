using UnityEngine;
using System.Collections;

public class TurnKeys : MonoBehaviour
{

    public static bool turnKeyOne = false;
    public static bool turnKeyTwo = false;
    public static bool turnKeyThree = false;
    public static bool turnKeyFour = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (turnKeyOne)
        {
            GameObject keyOne = GameObject.Find("KeyOne");
            keyOne.transform.localEulerAngles = Vector3.Lerp(keyOne.transform.localEulerAngles, new Vector3(-90, keyOne.transform.localEulerAngles.y, keyOne.transform.localEulerAngles.z), Time.deltaTime*.5f);

            if (keyOne.transform.localEulerAngles.x <= 285)
                turnKeyOne = false;
        }
        if (turnKeyTwo)
        {
            GameObject keyTwo = GameObject.Find("KeyTwo");
            keyTwo.transform.localEulerAngles = Vector3.Lerp(keyTwo.transform.localEulerAngles, new Vector3(-90, keyTwo.transform.localEulerAngles.y, keyTwo.transform.localEulerAngles.z), Time.deltaTime * .5f);

            if (keyTwo.transform.localEulerAngles.x <= 285)
                turnKeyTwo = false;
        }
        if (turnKeyThree)
        {
            GameObject keyThree = GameObject.Find("KeyThree");
            keyThree.transform.localEulerAngles = Vector3.Lerp(keyThree.transform.localEulerAngles, new Vector3(-90, keyThree.transform.localEulerAngles.y, keyThree.transform.localEulerAngles.z), Time.deltaTime * .5f);

            Debug.Log(keyThree.transform.localEulerAngles.x);
            if (keyThree.transform.localEulerAngles.x <= 285)
                turnKeyThree = false;
        }
        if (turnKeyFour)
        {
            GameObject keyFour = GameObject.Find("KeyFour");
            keyFour.transform.localEulerAngles = Vector3.Lerp(keyFour.transform.localEulerAngles, new Vector3(-90, keyFour.transform.localEulerAngles.y, keyFour.transform.localEulerAngles.z), Time.deltaTime * .5f);

            if (keyFour.transform.localEulerAngles.x <= 285)
                turnKeyFour = false;
        }

    }
}
