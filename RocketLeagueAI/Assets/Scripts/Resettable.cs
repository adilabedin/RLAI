using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This component allows us to make any object we attach it to able to reset to its starting location and rotation by calling the Reset function or by pressing a button. 
 */
public class Resettable : MonoBehaviour
{
    //create our location and rotataion variables to store our spawn position and rotation
    private Vector3 spawnLocation;
    private Quaternion spawnRotation;

    //store a reference to the rigidbody attached to this gameobject so that we can stop it from moving
    private Rigidbody rigidbody;
    public Bounds areaBounds;
    public GameObject Stadium;

    //store the name of the input axis that we can use to reset this object (To see the list of input axes go to: Edit > Project Settings > Input)
    public string resetInputAxis = "Submit"; 
    
    //At the start of the game...
    void Start()
    {
        //get the rigidbody component attached to the same gameobject
        rigidbody = GetComponent<Rigidbody>();

        //get the starting position and rotation of the object and store them in our variables
        spawnLocation = transform.position;
        spawnRotation = transform.rotation;

        areaBounds = Stadium.GetComponent<Collider>().bounds;
    }

    //On every frame...
    private void Update()
    {
        //if our resetInputAxis variable has been set to nothing in the unity inspector, stop running this "Update" function(return)
        if (resetInputAxis == "")
            return;

        //if our input axis is activated (is greater than 0) then reset!
        if (Input.GetAxis(resetInputAxis) > 0)
        {
            Reset();
        }
    }

    //this is where the actual reset happens. When we reset...
    public void Reset()
    {

        //check if we got a rigidbody component on the gameobject
        if (rigidbody != null)
        {
            //if we do... stop the physics simulation from moving it (that is, reduce its velocity to zero)
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        //set the position and rotation of our transform component back to what we had saved in the "Start" function
        transform.position = GetRandomSpawnPos();
        transform.rotation = spawnRotation;
    }

    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = UnityEngine.Random.Range(-areaBounds.extents.x * 1,
                areaBounds.extents.x * 1);

            var randomPosZ = UnityEngine.Random.Range(-areaBounds.extents.z * 1,
                areaBounds.extents.z * 1);

            randomSpawnPos = Stadium.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }

    //this give us a way to call our "Reset" function after a certain amount of seconds.
    public void ResetAfterSeconds(float seconds)
    {
        //Invoke allows us to call a function after a certain amount of seconds
        Invoke("Reset", seconds);
    }

    //when this object is destroyed for whatever reason...
    public void OnDestroy()
    {
        //cancel any Reset call that are waiting to run
        CancelInvoke("Reset");
    }
}
