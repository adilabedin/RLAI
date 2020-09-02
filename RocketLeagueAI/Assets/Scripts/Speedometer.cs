using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <---- We need to include this if we want to work with UI in this code document

public class Speedometer : MonoBehaviour
{
    //store a reference to a car rigidbody so we can ask it how fast it is going
    public Rigidbody carRigidbody;

    //store a reference to our UI Text component so we can change the text on it.
    private Text speedometerText;

    //At the start of the game...
    void Start()
    {
        //get the UI Text component on this gameobject and store it in our variable
        speedometerText = GetComponent<Text>();
    }

    //On every frame...
    void Update()
    {
        /*
         * the rigidbody stores velocity in meters per second. To get KM per hour we need to multiply by 3.6
         * Round that value
         * convert it to a string (that is, convert it to text)
         * change the UI text to that string.
         */
        speedometerText.text = Mathf.Round(carRigidbody.velocity.magnitude * 3.6f).ToString();
    }
}
