using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    [SerializeField] private string horizontalInputName, verticalInputName;

    [HideInInspector]
    public SoccerBall soccerBall;

    Rigidbody m_Rigidbody;
    Rigidbody m_Ball;

    public GameObject Ball;

    private Vector3 spawnLocation;
    private Quaternion spawnRotation;

    //private Vector3 ballSpawnLocation;
    //private Quaternion ballSpawnRotation;

    //public event Action OnReset;
    
    //public string resetInputAxis = "Submit";

    public string boostInputAxis = "Boost";
    public float boostDuration = 9999f;
    public float normalSpeed = 20f;
    public float boostSpeed = 35f;
    public float boostJolt = 5f;
    public bool carIsOnTheGround = true;
    //public float resetCooldown = 0;
    //private float nextResetTime = 0;

    public float JumpSpeed = 10f;

    private float horizontalInput;
    private float verticalInput;

    private bool RollRightInput;
    private bool RollLeftInput;

    public float rotationSpeed = 10f;

    public Transform ball;
    public Transform goal1;
    public Transform goal2;

    public void ScoredAGoal()
    {
        AddReward(5f);
        print("RewardForScoring");

        EndEpisode();

    }

    public override void Initialize()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Ball = Ball.GetComponent<Rigidbody>();

        spawnLocation = m_Rigidbody.transform.position;
        spawnRotation = m_Rigidbody.transform.rotation;

        //ballSpawnLocation = m_Ball.transform.position;
        //ballSpawnRotation = m_Ball.transform.rotation;

    }


    public override void OnActionReceived(float[] vectorAction)
    {
        var jump = (int)vectorAction[0];
        var boost = (int)vectorAction[1];
        var turnH = (int)vectorAction[2];
        var turnV = (int)vectorAction[3];
        var airRollLeft = (int)vectorAction[4];
        var airRollRight = (int)vectorAction[5];
        var resetCar = (int)vectorAction[6];


        switch (jump)
        {
            case 1:
                Jump();
                break;
        }

        switch (boost)
        {
            case 1:
                Boost();
                break;
        }

        switch (turnH)
        {
            case 1:
                RotateLeft();
                break;
            case 2:
                RotateRight();
                break;
        }

        switch (turnV)
        {
            case 1:
                RotateUp();
                break;
            case 2:
                RotateDown();
                break;
        }

        switch (airRollLeft)
        {
            case 1:
                AirRollLeft();
                break;
        }

        switch (airRollRight) { 
            case 1:
                AirRollRight();
                break;
        }

        switch (resetCar)
        {
            case 1:
                Reset();
                break;
        }

        AddReward(-2f / MaxStep);
    }

    public override void OnEpisodeBegin()
    {

    }

    public override void Heuristic(float[] actionsOut)
    {
        Array.Clear(actionsOut, 0, actionsOut.Length);

        //jump
        if (Input.GetAxis("jumpKey") > 0)
        {
            actionsOut[0] = 1;
        }
        //Boost
        if (Input.GetAxis("Boost") > 0)
        {
            actionsOut[1] = 1;
        }
        //Rotate Horizontally
        if (Input.GetAxis("Horizontal") > 0)
        {
            actionsOut[2] = 1;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            actionsOut[2] = 2;
        }
        //Rotate Vertically
        if (Input.GetAxis("Vertical") > 0)
        {
            actionsOut[3] = 1;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            actionsOut[3] = 2;
        }
        //AirRoll / Barrell Roll
        if (Input.GetAxis("AirRollLeft") > 0)
        {
            actionsOut[4] = 1;
        }
        if (Input.GetAxis("AirRollRight") > 0)
        {
            actionsOut[5] = 1;
        }
        //Reset Car
        if (Input.GetAxis("ResetCar") > 0)
        {
            actionsOut[6] = 1;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //distance to ball
        sensor.AddObservation(Vector3.Distance(m_Ball.transform.position, transform.position));
        //direction to ball
        sensor.AddObservation((m_Ball.transform.position - transform.position).normalized);

        //distance to Goal1
        sensor.AddObservation(Vector3.Distance(goal1.transform.position, transform.position));
        //direction to Goal1
        sensor.AddObservation((goal1.transform.position - transform.position).normalized);

        //distance to Goal2
        sensor.AddObservation(Vector3.Distance(goal2.transform.position, transform.position));
        //direction to Goal2
        sensor.AddObservation((goal2.transform.position - transform.position).normalized);
    }

    private void Jump()
    {
        if (carIsOnTheGround)
        {
            m_Rigidbody.AddForce(new Vector3(0, 4000, 0), ForceMode.Impulse);
            carIsOnTheGround = false;
        }
    }

    public void Reset()
    {

        //check if we got a rigidbody component on the gameobject
        if (m_Rigidbody != null)
        {
            //if we do... stop the physics simulation from moving it (that is, reduce its velocity to zero)
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }

        //set the position and rotation of our transform component back to what we had saved in the "Start" function
        transform.position = spawnLocation;
        transform.rotation = spawnRotation;
    }

    void Boost()
    {
        m_Rigidbody.velocity = transform.forward * boostJolt;
    }

    void Update()
    {
        PlayerInput();
        CloserToGoalReward();
    }

    private void CloserToGoalReward()
    {
        if ((m_Ball.velocity.x) >= 0)
        {

        }
        else
        {
            AddReward(-0.1f);
            print("WrongWay");
        }
    }

    private void OnCollisionEnter(Collision collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Ground"))
        {
            carIsOnTheGround = true;
        }

        if (collidedObj.gameObject.CompareTag("Goal"))
        {
            AddReward(-0.1f);
            print("NegativeRewardWhenInGoal");
        }

        if (collidedObj.gameObject.CompareTag("Ball"))
        {
            AddReward(0.5f);
            print("rewardTouch");
        }

    }


    void FixedUpdate()
    {


    }

    private void PlayerInput()
    {
        // Grab player input
        verticalInput = Input.GetAxis(verticalInputName);
        horizontalInput = Input.GetAxis(horizontalInputName);

        if (Input.GetAxis("AirRollRight") > 0){
            RollRightInput = true;
        }
        if (Input.GetAxis("AirRollLeft") > 0)
        {
            RollLeftInput = true;
        }
           
    }

    private void RotateUp()
    {
        if (carIsOnTheGround == false)
        {
            if (verticalInput > 0)
            {
                this.transform.Rotate(Vector3.right, rotationSpeed);
            }
        }
    }

    private void RotateDown()
    {
        if (carIsOnTheGround == false)
        {
            if (verticalInput < 0)
            {
                this.transform.Rotate(Vector3.left, rotationSpeed);
            }
        }
    }

    private void RotateLeft()
    {
        if (horizontalInput > 0)
        {
            this.transform.Rotate(Vector3.up, rotationSpeed);
        }

    }

    private void RotateRight()
    {
        if (horizontalInput < 0)
        {
            this.transform.Rotate(Vector3.down, rotationSpeed);
        }
    }

    private void AirRollLeft()
    {
        if (carIsOnTheGround == false)
        {
            if (RollRightInput)
            {
                this.transform.Rotate(Vector3.back, rotationSpeed);
            }
        }
    }

    private void AirRollRight()
    {
        if (carIsOnTheGround == false)
        {
                if (RollLeftInput)
            {
                this.transform.Rotate(Vector3.forward, rotationSpeed);
            }
        }

    }

}
